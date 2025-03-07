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
                name: "Clients",
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
                name: "Tests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                        principalTable: "Clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    task_message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    right_answer = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    image_path = table.Column<string>(type: "text", nullable: false),
                    test_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.id);
                    table.ForeignKey(
                        name: "fk_tasks_tests_test_id",
                        column: x => x.test_id,
                        principalTable: "Tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_of_choosing_answer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    answers = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_of_choosing_answer", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_of_choosing_answer_tasks_id",
                        column: x => x.id,
                        principalTable: "Tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tasks_test_id",
                table: "Tasks",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "ix_tests_client_id",
                table: "Tests",
                column: "client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task_of_choosing_answer");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
