using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StageBuilder.Migrations
{
    public partial class addStages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "stages",
                columns: table => new
                {
                    stageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    data = table.Column<string>(nullable: true),
                    userId = table.Column<int>(nullable: false),
                    gameId = table.Column<int>(nullable: false),
                    createdDate = table.Column<DateTime>(nullable: false),
                    lastUpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stages", x => x.stageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stages");
        }
    }
}
