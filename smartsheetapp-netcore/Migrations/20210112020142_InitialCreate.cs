using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartsheetapp_netcore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventCallbackModels",
                columns: table => new
                {
                    nonce = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    webhookId = table.Column<long>(type: "bigint", nullable: false),
                    scope = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    scopeObjectId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCallbackModels", x => x.nonce);
                });

            migrationBuilder.CreateTable(
                name: "StatusChangeCallbackModels",
                columns: table => new
                {
                    nonce = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    webhookId = table.Column<long>(type: "bigint", nullable: false),
                    scope = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    scopeObjectId = table.Column<long>(type: "bigint", nullable: false),
                    newWebhookStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusChangeCallbackModels", x => x.nonce);
                });

            migrationBuilder.CreateTable(
                name: "EventModels",
                columns: table => new
                {
                    eventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    objectType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    eventType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id = table.Column<long>(type: "bigint", nullable: true),
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    rowId = table.Column<long>(type: "bigint", nullable: true),
                    columnId = table.Column<long>(type: "bigint", nullable: true),
                    cellValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    eventCallbackModelnonce = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventModels", x => x.eventId);
                    table.ForeignKey(
                        name: "FK_EventModels_EventCallbackModels_eventCallbackModelnonce",
                        column: x => x.eventCallbackModelnonce,
                        principalTable: "EventCallbackModels",
                        principalColumn: "nonce",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventModels_eventCallbackModelnonce",
                table: "EventModels",
                column: "eventCallbackModelnonce");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventModels");

            migrationBuilder.DropTable(
                name: "StatusChangeCallbackModels");

            migrationBuilder.DropTable(
                name: "EventCallbackModels");
        }
    }
}
