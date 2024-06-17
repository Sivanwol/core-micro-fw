using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateMediaSizeToLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "size",
                table: "media",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "size",
                table: "media",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

        }
    }
}
