using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Context;
using System.Text.Json;

namespace Persistence.Data.ContextSeed
{
    public class DataSeeding(SmartPcosDbContext _businessDb) : IDataSeeding
    {
        public async Task SeedAllAsync()
        {
            try
            {
                // Check and apply pending migrations
                var pendingMigrations = await _businessDb.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    await _businessDb.Database.MigrateAsync();
                }
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                // Seed ContactUsMessages
                if (!_businessDb.Set<ContactUsMessage>().Any())
                {
                    using var contactData = File.OpenRead(@"..\Infrastructure Layer\Persistence\Data\SeedData\contact_us_messages.json");
                    var messages = await JsonSerializer.DeserializeAsync<List<ContactUsMessage>>(contactData);
                    if (messages is not null && messages.Any())
                    {
                        await _businessDb.ContactUsMessages.AddRangeAsync(messages);
                    }
                }

                // Seed ClinicalData
                if (!_businessDb.Set<ClinicalData>().Any())
                {
                    using var clinicalData = File.OpenRead(@"..\Infrastructure Layer\Persistence\Data\SeedData\clinical_data.json");
                    var records = await JsonSerializer.DeserializeAsync<List<ClinicalData>>(clinicalData);
                    if (records is not null && records.Any())
                    {
                        await _businessDb.ClinicalData.AddRangeAsync(records);
                    }
                }

                // Seed HormoneLabResults
                if (!_businessDb.Set<HormoneLabResult>().Any())
                {
                    using var hormoneData = File.OpenRead(@"..\Infrastructure Layer\Persistence\Data\SeedData\hormone_lab_results.json");
                    var results = await JsonSerializer.DeserializeAsync<List<HormoneLabResult>>(hormoneData);
                    if (results is not null && results.Any())
                    {
                        await _businessDb.HormoneLabResults.AddRangeAsync(results);
                    }
                }

                // Seed ApplicationUsers
                if (!_businessDb.Set<ApplicationUser>().Any())
                {
                    using var userData = File.OpenRead(@"..\Infrastructure Layer\Persistence\Data\SeedData\users.json");
                    var users = await JsonSerializer.DeserializeAsync<List<ApplicationUser>>(userData);

                    if (users is not null && users.Any())
                    {
                        // Note: These users will be added without passwords. 
                        // For real auth, you would use UserManager to hash passwords.
                        await _businessDb.Set<ApplicationUser>().AddRangeAsync(users);
                    }
                }

                // 4. Seed UltrasoundImages
                if (!_businessDb.Set<UltrasoundImage>().Any())
                {
                    using var ultrasoundData = File.OpenRead(@"..\Infrastructure Layer\Persistence\Data\SeedData\ultrasound_images.json");
                    var imageRecords = await JsonSerializer.DeserializeAsync<List<UltrasoundImage>>(ultrasoundData);
                    if (imageRecords is not null)
                    {
                        await _businessDb.Set<UltrasoundImage>().AddRangeAsync(imageRecords);
                    }
                }
                // Save all changes to database
                await _businessDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle or log exception
                Console.WriteLine(ex.Message);
            }
        }
    }
}