using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LookDB.Migrations
{
    public partial class AddDBCompanyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FakeTable_UserName",
                schema: "mem",
                table: "FakeTable");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_ActiveBool",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_Address",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_AddressCity",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_AddressProv",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_BackName",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_DateBirth",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_Email",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_FrontName",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_FullName",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_HP",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_IDCardNo",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_InsertBy",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_Marital",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_Photo",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_PlaceBirth",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_Sex",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.DropIndex(
                name: "IX_DtMember_UpdateBy",
                schema: "mem",
                table: "DtMember");

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                schema: "mem",
                table: "DtMember",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(900)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertDate",
                schema: "mem",
                table: "DtMember",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 27, 9, 36, 11, 860, DateTimeKind.Local).AddTicks(5310),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 26, 17, 12, 7, 915, DateTimeKind.Local).AddTicks(8651));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                schema: "mem",
                table: "DtMember",
                type: "varbinary(900)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertDate",
                schema: "mem",
                table: "DtMember",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 26, 17, 12, 7, 915, DateTimeKind.Local).AddTicks(8651),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 27, 9, 36, 11, 860, DateTimeKind.Local).AddTicks(5310));

            migrationBuilder.CreateIndex(
                name: "IX_FakeTable_UserName",
                schema: "mem",
                table: "FakeTable",
                column: "UserName");

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
        }
    }
}
