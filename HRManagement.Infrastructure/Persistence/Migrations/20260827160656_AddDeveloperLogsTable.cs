using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRManagement.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDeveloperLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeveloperLogs",
                columns: table => new
                {
                    Id = table.Column<long>(
                        type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1 , 1"),
                    Message = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: true),
                    MessageTemplate = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: true),
                    Level = table.Column<string>(
                        type: "nvarchar(16)",
                        maxLength: 16,
                        nullable: true),
                    TimeStamp = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: false),
                    Exception = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: true),
                    LogEvent = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: true),
                    TraceId = table.Column<string>(
                        type: "nvarchar(32)",
                        maxLength: 32,
                        nullable: true),
                    SpanId = table.Column<string>(
                        type: "nvarchar(16)",
                        maxLength: 16,
                        nullable: true),
                    SourceContext = table.Column<string>(
                        type: "nvarchar(256)",
                        maxLength: 256,
                        nullable: true),
                    RequestPath = table.Column<string>(
                        type: "nvarchar(500)",
                        maxLength: 500,
                        nullable: true)
                },

            constraints: table =>
            {
                table.PrimaryKey(
                    "PK_DeveloperLogs",
                    log => log.Id);
            });

            migrationBuilder.CreateIndex(
              name: "IX_DeveloperLogs_TimeStamp",
              table: "DeveloperLogs",
              column: "TimeStamp");

            migrationBuilder.CreateIndex(
              name: "IX_DeveloperLogs_TraceId",
              table: "DeveloperLogs",
              column: "TraceId");
        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "DeveloperLogs");
        }
    }
}

