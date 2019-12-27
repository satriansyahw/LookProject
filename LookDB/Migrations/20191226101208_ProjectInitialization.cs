using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LookDB.Migrations
{
    public partial class ProjectInitialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mem");

            migrationBuilder.CreateTable(
                name: "DtMember",
                schema: "mem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberNoReg = table.Column<string>(maxLength: 13, nullable: false),
                    FrontName = table.Column<string>(maxLength: 30, nullable: false),
                    BackName = table.Column<string>(maxLength: 30, nullable: true),
                    FullName = table.Column<string>(maxLength: 60, nullable: true),
                    IDCardNo = table.Column<string>(maxLength: 20, nullable: false),
                    HP = table.Column<string>(maxLength: 15, nullable: false),
                    Email = table.Column<string>(maxLength: 60, nullable: false),
                    DateBirth = table.Column<string>(maxLength: 10, nullable: false),
                    PlaceBirth = table.Column<string>(maxLength: 30, nullable: true),
                    Sex = table.Column<string>(maxLength: 1, nullable: false),
                    Marital = table.Column<bool>(nullable: false),
                    Address = table.Column<string>(maxLength: 100, nullable: true),
                    AddressCity = table.Column<string>(maxLength: 30, nullable: true),
                    AddressProv = table.Column<string>(maxLength: 30, nullable: true),
                    Photo = table.Column<byte[]>(nullable: true),
                    InsertBy = table.Column<string>(maxLength: 18, nullable: true),
                    InsertDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 12, 26, 17, 12, 7, 915, DateTimeKind.Local).AddTicks(8651)),
                    UpdateBy = table.Column<string>(maxLength: 30, nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ActiveBool = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DtMember", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FakeTable",
                schema: "mem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FakeTable", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_ActiveBool",
                schema: "mem",
                table: "DtMember",
                column: "ActiveBool");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_Address",
                schema: "mem",
                table: "DtMember",
                column: "Address");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_AddressCity",
                schema: "mem",
                table: "DtMember",
                column: "AddressCity");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_AddressProv",
                schema: "mem",
                table: "DtMember",
                column: "AddressProv");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_BackName",
                schema: "mem",
                table: "DtMember",
                column: "BackName");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_DateBirth",
                schema: "mem",
                table: "DtMember",
                column: "DateBirth");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_Email",
                schema: "mem",
                table: "DtMember",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_FrontName",
                schema: "mem",
                table: "DtMember",
                column: "FrontName");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_FullName",
                schema: "mem",
                table: "DtMember",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_HP",
                schema: "mem",
                table: "DtMember",
                column: "HP");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_IDCardNo",
                schema: "mem",
                table: "DtMember",
                column: "IDCardNo");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_InsertBy",
                schema: "mem",
                table: "DtMember",
                column: "InsertBy");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_Marital",
                schema: "mem",
                table: "DtMember",
                column: "Marital");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_MemberNoReg",
                schema: "mem",
                table: "DtMember",
                column: "MemberNoReg",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_Photo",
                schema: "mem",
                table: "DtMember",
                column: "Photo");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_PlaceBirth",
                schema: "mem",
                table: "DtMember",
                column: "PlaceBirth");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_Sex",
                schema: "mem",
                table: "DtMember",
                column: "Sex");

            migrationBuilder.CreateIndex(
                name: "IX_DtMember_UpdateBy",
                schema: "mem",
                table: "DtMember",
                column: "UpdateBy");

            migrationBuilder.CreateIndex(
                name: "IX_FakeTable_UserName",
                schema: "mem",
                table: "FakeTable",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DtMember",
                schema: "mem");

            migrationBuilder.DropTable(
                name: "FakeTable",
                schema: "mem");
        }
    }
}
