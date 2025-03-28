using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomTestCreator.Clients.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_infinite_mode = table.Column<bool>(type: "boolean", nullable: false),
                    is_random_tasks = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_published = table.Column<bool>(type: "boolean", nullable: false),
                    is_time_limited = table.Column<bool>(type: "boolean", nullable: false),
                    verdicts = table.Column<string>(type: "jsonb", nullable: false),
                    client_id = table.Column<Guid>(type: "uuid", nullable: true),
                    limit_time_hours = table.Column<int>(type: "integer", nullable: false),
                    limit_time_minutes = table.Column<int>(type: "integer", nullable: false),
                    limit_time_seconds = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tests", x => x.id);
                    table.ForeignKey(
                        name: "fk_tests_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    task_message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    right_answer = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    image_path = table.Column<string>(type: "text", nullable: false),
                    discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    test_id = table.Column<Guid>(type: "uuid", nullable: true),
                    answers = table.Column<string>(type: "jsonb", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tasks", x => x.id);
                    table.ForeignKey(
                        name: "fk_tasks_tests_test_id",
                        column: x => x.test_id,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tasks_test_id",
                table: "tasks",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "ix_tests_client_id",
                table: "tests",
                column: "client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "tests");

            migrationBuilder.DropTable(
                name: "clients");
        }
    }
}
