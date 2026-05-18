using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PcosAnalyzer.API.Extensions;
using PcosAnalyzer.API.Middlewares;
using Persistence.Data.Context;
using Persistence.Data.ContextSeed;
using Persistence.Data.HallperClass;
using Services.Extensions;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        // Never strip null fields — the AI may return meaningful nulls (e.g. ultrasound_heatmap_base64)
        opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PCOS Analyzer API", Version = "v1" });
    c.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddBusinessServices(builder.Configuration);
builder.Services.AddScoped<IDataSeeding, DataSeeding>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SmartPcosDbContext>();
    try
    {
        await dbContext.Database.MigrateAsync();
    }
    catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 2714)
    {
        // Schema already exists but EF migration history is incomplete.
        // Register every migration as applied so EF stops trying to re-create tables.
        await MarkAllMigrationsAppliedAsync(dbContext);
    }
}

await app.SeedDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PCOS Analyzer API V1");
    c.RoutePrefix = string.Empty;
});

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task MarkAllMigrationsAppliedAsync(SmartPcosDbContext dbContext)
{
    var conn = dbContext.Database.GetDbConnection();
    if (conn.State != System.Data.ConnectionState.Open)
        await conn.OpenAsync();

    using (var createCmd = conn.CreateCommand())
    {
        createCmd.CommandText = @"
            IF NOT EXISTS (
                SELECT 1 FROM sys.objects
                WHERE object_id = OBJECT_ID(N'[__EFMigrationsHistory]') AND type = 'U'
            )
            CREATE TABLE [__EFMigrationsHistory] (
                [MigrationId] nvarchar(150) NOT NULL,
                [ProductVersion] nvarchar(32) NOT NULL,
                CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
            )";
        await createCmd.ExecuteNonQueryAsync();
    }

    foreach (var migration in dbContext.Database.GetMigrations())
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            "IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = @id) " +
            "INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (@id, @ver)";
        var pId = cmd.CreateParameter(); pId.ParameterName = "@id"; pId.Value = migration;
        var pVer = cmd.CreateParameter(); pVer.ParameterName = "@ver"; pVer.Value = "8.0.0";
        cmd.Parameters.Add(pId);
        cmd.Parameters.Add(pVer);
        await cmd.ExecuteNonQueryAsync();
    }
}
