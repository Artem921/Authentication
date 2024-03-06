using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApplicationContext.Migrations
{
    /// <inheritdoc />
    public partial class adduser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("47bceb23-b06f-4f13-ae57-fb0890688772"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role" },
                values: new object[,]
                {
                    { new Guid("22fb035d-48d0-4639-9cfa-5207ad460b7e"), "admin@mail.ru", "123", "Administrator" },
                    { new Guid("eb4a49e5-fb34-44ca-8d3d-359acf503407"), "vova@mail.ru", "123", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22fb035d-48d0-4639-9cfa-5207ad460b7e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("eb4a49e5-fb34-44ca-8d3d-359acf503407"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role" },
                values: new object[] { new Guid("47bceb23-b06f-4f13-ae57-fb0890688772"), "admin@mail.ru", "123", "Administrator" });
        }
    }
}
