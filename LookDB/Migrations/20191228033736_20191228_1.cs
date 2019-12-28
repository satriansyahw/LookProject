using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LookDB.Migrations
{
    public partial class _20191228_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertDate",
                schema: "mem",
                table: "DtMember",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2756),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 28, 10, 33, 58, 377, DateTimeKind.Local).AddTicks(1399));

            migrationBuilder.CreateTable(
                name: "DtCertification",
                schema: "mem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(nullable: false),
                    CertName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    CertStart = table.Column<string>(maxLength: 6, nullable: true),
                    CertEnd = table.Column<string>(maxLength: 6, nullable: true),
                    FileSupport = table.Column<string>(maxLength: 50, nullable: true),
                    InsertBy = table.Column<string>(maxLength: 30, nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 12, 28, 10, 37, 36, 507, DateTimeKind.Local).AddTicks(6754)),
                    UpdateBy = table.Column<string>(maxLength: 30, nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ActiveBool = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DtCertification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DtCompany",
                schema: "mem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyNoReg = table.Column<string>(maxLength: 13, nullable: false),
                    CompName = table.Column<string>(maxLength: 30, nullable: false),
                    CompField = table.Column<string>(maxLength: 30, nullable: true),
                    ShortProfile = table.Column<string>(maxLength: 200, nullable: true),
                    CompanyAddr = table.Column<string>(maxLength: 100, nullable: true),
                    CompCity = table.Column<string>(maxLength: 30, nullable: true),
                    CompProv = table.Column<string>(maxLength: 30, nullable: true),
                    NPWP = table.Column<string>(maxLength: 20, nullable: true),
                    ContactName1 = table.Column<string>(maxLength: 30, nullable: true),
                    ContactHP1 = table.Column<string>(maxLength: 15, nullable: true),
                    ContactEmail1 = table.Column<bool>(maxLength: 60, nullable: false),
                    ContactName2 = table.Column<string>(maxLength: 30, nullable: true),
                    ContactHP2 = table.Column<string>(maxLength: 15, nullable: true),
                    ContactEmail2 = table.Column<string>(maxLength: 60, nullable: true),
                    CompPortal = table.Column<string>(maxLength: 100, nullable: true),
                    MemberType = table.Column<string>(maxLength: 1, nullable: true),
                    SubscribeStart = table.Column<string>(maxLength: 10, nullable: true),
                    SubscribeEnd = table.Column<string>(maxLength: 10, nullable: true),
                    EffectiveBool = table.Column<bool>(nullable: false, defaultValue: true),
                    InsertBy = table.Column<string>(maxLength: 18, nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2198)),
                    UpdateBy = table.Column<string>(maxLength: 30, nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ActiveBool = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DtCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DtEducation",
                schema: "mem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(nullable: false),
                    Institution = table.Column<string>(maxLength: 50, nullable: false),
                    InstitutionLocation = table.Column<string>(maxLength: 100, nullable: true),
                    Major = table.Column<string>(maxLength: 50, nullable: true),
                    IPK = table.Column<string>(maxLength: 4, nullable: true),
                    StudyStart = table.Column<string>(maxLength: 6, nullable: false),
                    StudyEnd = table.Column<string>(maxLength: 6, nullable: true),
                    OnStudy = table.Column<bool>(nullable: false, defaultValue: false),
                    FileSupport = table.Column<string>(maxLength: 50, nullable: true),
                    InsertBy = table.Column<string>(maxLength: 30, nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2342)),
                    UpdateBy = table.Column<string>(maxLength: 30, nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ActiveBool = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DtEducation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DtExpertise",
                schema: "mem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(nullable: false),
                    ExpertName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    ExpertLevel = table.Column<short>(maxLength: 6, nullable: false),
                    FileSupport = table.Column<string>(maxLength: 50, nullable: true),
                    InsertBy = table.Column<string>(maxLength: 30, nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2484)),
                    UpdateBy = table.Column<string>(maxLength: 30, nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ActiveBool = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DtExpertise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DtLanguage",
                schema: "mem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(nullable: false),
                    LangName = table.Column<string>(maxLength: 50, nullable: false),
                    LangLevel = table.Column<short>(maxLength: 6, nullable: false),
                    InsertBy = table.Column<string>(maxLength: 30, nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2605)),
                    UpdateBy = table.Column<string>(maxLength: 30, nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ActiveBool = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DtLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DtOrgExperience",
                schema: "mem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(nullable: false),
                    OrgName = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<string>(maxLength: 30, nullable: true),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    OrgStart = table.Column<string>(maxLength: 6, nullable: true),
                    OrgEnd = table.Column<string>(maxLength: 6, nullable: true),
                    OnOrg = table.Column<bool>(nullable: false, defaultValue: false),
                    FileSupport = table.Column<string>(maxLength: 50, nullable: true),
                    InsertBy = table.Column<string>(maxLength: 30, nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2883)),
                    UpdateBy = table.Column<string>(maxLength: 30, nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ActiveBool = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DtOrgExperience", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DtWorkingExperience",
                schema: "mem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(nullable: false),
                    CompName = table.Column<string>(maxLength: 50, nullable: false),
                    CompField = table.Column<string>(maxLength: 100, nullable: true),
                    Dept = table.Column<string>(maxLength: 50, nullable: true),
                    Position = table.Column<string>(maxLength: 30, nullable: true),
                    Specialization = table.Column<string>(maxLength: 30, nullable: true),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    WorkStart = table.Column<string>(maxLength: 6, nullable: true),
                    WorkEnd = table.Column<string>(maxLength: 6, nullable: true),
                    OnWork = table.Column<bool>(nullable: false, defaultValue: false),
                    FileSupport = table.Column<string>(maxLength: 50, nullable: true),
                    InsertBy = table.Column<string>(maxLength: 30, nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(3020)),
                    UpdateBy = table.Column<string>(maxLength: 30, nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ActiveBool = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DtWorkingExperience", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DtWorkingInterest",
                schema: "mem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(nullable: false),
                    Posisi = table.Column<string>(maxLength: 30, nullable: true),
                    Dept = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: true),
                    Location = table.Column<string>(maxLength: 50, nullable: true),
                    Salary = table.Column<int>(maxLength: 50, nullable: false),
                    InsertBy = table.Column<string>(maxLength: 30, nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(3152)),
                    UpdateBy = table.Column<string>(maxLength: 30, nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ActiveBool = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DtWorkingInterest", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DtCompany_CompanyNoReg",
                schema: "mem",
                table: "DtCompany",
                column: "CompanyNoReg",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DtEducation_FileSupport",
                schema: "mem",
                table: "DtEducation",
                column: "FileSupport");

            migrationBuilder.CreateIndex(
                name: "IX_DtExpertise_FileSupport",
                schema: "mem",
                table: "DtExpertise",
                column: "FileSupport");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DtCertification",
                schema: "mem");

            migrationBuilder.DropTable(
                name: "DtCompany",
                schema: "mem");

            migrationBuilder.DropTable(
                name: "DtEducation",
                schema: "mem");

            migrationBuilder.DropTable(
                name: "DtExpertise",
                schema: "mem");

            migrationBuilder.DropTable(
                name: "DtLanguage",
                schema: "mem");

            migrationBuilder.DropTable(
                name: "DtOrgExperience",
                schema: "mem");

            migrationBuilder.DropTable(
                name: "DtWorkingExperience",
                schema: "mem");

            migrationBuilder.DropTable(
                name: "DtWorkingInterest",
                schema: "mem");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertDate",
                schema: "mem",
                table: "DtMember",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 28, 10, 33, 58, 377, DateTimeKind.Local).AddTicks(1399),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 28, 10, 37, 36, 508, DateTimeKind.Local).AddTicks(2756));
        }
    }
}
