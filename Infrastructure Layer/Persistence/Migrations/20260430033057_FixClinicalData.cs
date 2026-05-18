using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixClinicalData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DifficultyLosingWeight",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "HipInch",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "PhysicalSymptoms",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "RecentMedicalReports",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "WaistInch",
                table: "clinical_data");

            migrationBuilder.RenameColumn(
                name: "Bmi",
                table: "clinical_data",
                newName: "BMI");

            migrationBuilder.RenameColumn(
                name: "SystolicBP",
                table: "clinical_data",
                newName: "RR");

            migrationBuilder.RenameColumn(
                name: "RespiratoryRate",
                table: "clinical_data",
                newName: "MarriageStatus");

            migrationBuilder.RenameColumn(
                name: "RegularPhysicalActivity",
                table: "clinical_data",
                newName: "RegExercise");

            migrationBuilder.RenameColumn(
                name: "MenstrualCycleRegular",
                table: "clinical_data",
                newName: "Pregnant");

            migrationBuilder.RenameColumn(
                name: "MaritalStatusYears",
                table: "clinical_data",
                newName: "FollicleNoR");

            migrationBuilder.RenameColumn(
                name: "IsPregnant",
                table: "clinical_data",
                newName: "FastFood");

            migrationBuilder.RenameColumn(
                name: "FastFoodFrequency",
                table: "clinical_data",
                newName: "FollicleNoL");

            migrationBuilder.RenameColumn(
                name: "DiastolicBP",
                table: "clinical_data",
                newName: "CycleLength");

            migrationBuilder.RenameColumn(
                name: "AverageCycleLength",
                table: "clinical_data",
                newName: "Cycle");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "clinical_data",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "Height",
                table: "clinical_data",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "clinical_data",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "BMI",
                table: "clinical_data",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<double>(
                name: "AMH",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AvgFSizeL",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AvgFSizeR",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "BP_Diastolic",
                table: "clinical_data",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BP_Systolic",
                table: "clinical_data",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "BetaHCG_I",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BetaHCG_II",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Endometrium",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FSH",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FSH_LH",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Hb",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Hip",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LH",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PRG",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PRL",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RBS",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TSH",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VitD3",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Waist",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WaistHipRatio",
                table: "clinical_data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AMH",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "AvgFSizeL",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "AvgFSizeR",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "BP_Diastolic",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "BP_Systolic",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "BetaHCG_I",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "BetaHCG_II",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "Endometrium",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "FSH",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "FSH_LH",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "Hb",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "Hip",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "LH",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "PRG",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "PRL",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "RBS",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "TSH",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "VitD3",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "Waist",
                table: "clinical_data");

            migrationBuilder.DropColumn(
                name: "WaistHipRatio",
                table: "clinical_data");

            migrationBuilder.RenameColumn(
                name: "BMI",
                table: "clinical_data",
                newName: "Bmi");

            migrationBuilder.RenameColumn(
                name: "RegExercise",
                table: "clinical_data",
                newName: "RegularPhysicalActivity");

            migrationBuilder.RenameColumn(
                name: "RR",
                table: "clinical_data",
                newName: "SystolicBP");

            migrationBuilder.RenameColumn(
                name: "Pregnant",
                table: "clinical_data",
                newName: "MenstrualCycleRegular");

            migrationBuilder.RenameColumn(
                name: "MarriageStatus",
                table: "clinical_data",
                newName: "RespiratoryRate");

            migrationBuilder.RenameColumn(
                name: "FollicleNoR",
                table: "clinical_data",
                newName: "MaritalStatusYears");

            migrationBuilder.RenameColumn(
                name: "FollicleNoL",
                table: "clinical_data",
                newName: "FastFoodFrequency");

            migrationBuilder.RenameColumn(
                name: "FastFood",
                table: "clinical_data",
                newName: "IsPregnant");

            migrationBuilder.RenameColumn(
                name: "CycleLength",
                table: "clinical_data",
                newName: "DiastolicBP");

            migrationBuilder.RenameColumn(
                name: "Cycle",
                table: "clinical_data",
                newName: "AverageCycleLength");

            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "clinical_data",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "Height",
                table: "clinical_data",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "clinical_data",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<float>(
                name: "Bmi",
                table: "clinical_data",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<bool>(
                name: "DifficultyLosingWeight",
                table: "clinical_data",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "HipInch",
                table: "clinical_data",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "PhysicalSymptoms",
                table: "clinical_data",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecentMedicalReports",
                table: "clinical_data",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "WaistInch",
                table: "clinical_data",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
