using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shortener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPostsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("2ea1e1f3-27a1-4140-b938-f1f2f2829478"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8775a6e5-a388-4848-b1cf-492be8909fbf", "AQAAAAIAAYagAAAAEFKXHAZdGiv12S7FZFVEaoDxigGuOB3Z/3+zcBFUQAif7CqEywS7esWmjPEKV64wqQ==", "7a2581b5-c199-4502-964a-70c02c6b1d34" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("2ea1e1f3-27a1-4140-b938-f1f2f2829478"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff9ced03-1d0f-4414-b6b6-195743c84cf4", "AQAAAAIAAYagAAAAEDcG1N5U+ZfX8QoKXUXr8dyLKrvonKSFSGZMK5/ONe83zZSIv5Nshj+KLfNN9wAjPQ==", "47c6ffe6-62be-464b-b15e-37009b3b3bdd" });
        }
    }
}
