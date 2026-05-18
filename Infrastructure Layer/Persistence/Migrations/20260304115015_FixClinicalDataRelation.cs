using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixClinicalDataRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clinical_data_users_ApplicationUserId",
                table: "clinical_data");

            migrationBuilder.DropIndex(
                name: "IX_clinical_data_ApplicationUserId",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "clinical_data");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "clinical_data",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_clinical_data_UserId",
                table: "clinical_data",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_clinical_data_users_UserId",
                table: "clinical_data",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clinical_data_users_UserId",
                table: "clinical_data");

            migrationBuilder.DropIndex(
                name: "IX_clinical_data_UserId",
                table: "clinical_data");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "clinical_data",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "clinical_data",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_clinical_data_ApplicationUserId",
                table: "clinical_data",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_clinical_data_users_ApplicationUserId",
                table: "clinical_data",
                column: "ApplicationUserId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
