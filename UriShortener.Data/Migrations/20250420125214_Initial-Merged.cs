using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UriShortener.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMerged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "NVARCHAR(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    Pwd = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "DATETIME2(0)", precision: 0, nullable: false),
                    EmailVerificationToken = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    EmailVerificationTokenIssuedAt = table.Column<DateTime>(type: "DATETIME2(0)", precision: 0, nullable: true),
                    EmailVerificationTokenUsedAt = table.Column<DateTime>(type: "DATETIME2(0)", precision: 0, nullable: true),
                    EmailVerificationTokenExpiresAt = table.Column<DateTime>(type: "DATETIME2(0)", precision: 0, nullable: true),
                    PasswordResetToken = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    PasswordResetTokenIssuedAt = table.Column<DateTime>(type: "DATETIME2(0)", precision: 0, nullable: true),
                    PasswordResetTokenUsedAt = table.Column<DateTime>(type: "DATETIME2(0)", precision: 0, nullable: true),
                    PasswordResetTokenExpiresAt = table.Column<DateTime>(type: "DATETIME2(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShortenedURIs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2(0)", precision: 0, nullable: false),
                    ValidFor = table.Column<DateTime>(type: "DATETIME2(0)", precision: 0, nullable: false),
                    CreatorId = table.Column<int>(type: "INT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortenedURIs", x => x.Key);
                    table.ForeignKey(
                        name: "FK_ShortenedURIs_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UsersRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "PremiumUser" },
                    { 3, "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedURIs_CreatorId",
                table: "ShortenedURIs",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortenedURIs_Key",
                table: "ShortenedURIs",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Pwd",
                table: "Users",
                column: "Pwd",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersRoles_UserId",
                table: "UsersRoles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortenedURIs");

            migrationBuilder.DropTable(
                name: "UsersRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
