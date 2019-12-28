using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LookDB.Migrations
{
    public partial class _20191228 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PlaceBirth",
                schema: "mem",
                table: "DtMember",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertDate",
                schema: "mem",
                table: "DtMember",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 28, 10, 33, 58, 377, DateTimeKind.Local).AddTicks(1399),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 27, 9, 36, 11, 860, DateTimeKind.Local).AddTicks(5310));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PlaceBirth",
                schema: "mem",
                table: "DtMember",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertDate",
                schema: "mem",
                table: "DtMember",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 27, 9, 36, 11, 860, DateTimeKind.Local).AddTicks(5310),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 28, 10, 33, 58, 377, DateTimeKind.Local).AddTicks(1399));
        }
    }
}
