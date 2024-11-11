using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedefaultdatefunc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "SalesOrders",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "SalesOrders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
