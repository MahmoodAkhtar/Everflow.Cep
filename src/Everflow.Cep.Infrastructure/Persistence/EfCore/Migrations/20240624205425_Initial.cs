using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Everflow.Cep.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvitedUserId = table.Column<int>(type: "int", nullable: false),
                    InvitedToEventId = table.Column<int>(type: "int", nullable: false),
                    SentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Events_InvitedToEventId",
                        column: x => x.InvitedToEventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invitations_Users_InvitedUserId",
                        column: x => x.InvitedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[,]
                {
                    { 1, "user1@example.com", "User One", "P@ssword1" },
                    { 2, "user2@example.com", "User Two", "P@ssword2" },
                    { 3, "user3@example.com", "User Three", "P@ssword3" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CreatedByUserId", "Description", "EndDateTime", "Name", "StartDateTime", "Status" },
                values: new object[,]
                {
                    { 1, 1, "Event One description", new DateTime(2024, 6, 22, 20, 54, 25, 336, DateTimeKind.Utc).AddTicks(7915), "Event One", new DateTime(2024, 6, 21, 20, 54, 25, 336, DateTimeKind.Utc).AddTicks(7844), "Finished" },
                    { 2, 1, "Event Two description", new DateTime(2024, 6, 25, 6, 54, 25, 336, DateTimeKind.Utc).AddTicks(7919), "Event Two", new DateTime(2024, 6, 24, 10, 54, 25, 336, DateTimeKind.Utc).AddTicks(7918), "CloseToInvitation" },
                    { 3, 1, "Event Three description", new DateTime(2024, 6, 24, 22, 54, 25, 336, DateTimeKind.Utc).AddTicks(7921), "Event Three", new DateTime(2024, 6, 24, 15, 54, 25, 336, DateTimeKind.Utc).AddTicks(7920), "OpenToInvitation" },
                    { 4, 1, "Event Four description", new DateTime(2024, 6, 25, 1, 54, 25, 336, DateTimeKind.Utc).AddTicks(7922), "Event Four", new DateTime(2024, 6, 24, 21, 54, 25, 336, DateTimeKind.Utc).AddTicks(7922), "Draft" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CreatedByUserId",
                table: "Events",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InvitedToEventId",
                table: "Invitations",
                column: "InvitedToEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InvitedUserId",
                table: "Invitations",
                column: "InvitedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
