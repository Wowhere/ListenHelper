using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace voicio.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HintTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HintText = table.Column<string>(type: "TEXT", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HintTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagText = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HintTagTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagId = table.Column<int>(type: "INTEGER", nullable: false),
                    HintId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HintTagTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HintTagTable_HintTable_HintId",
                        column: x => x.HintId,
                        principalTable: "HintTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HintTagTable_TagTable_TagId",
                        column: x => x.TagId,
                        principalTable: "TagTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HintTagTable_HintId",
                table: "HintTagTable",
                column: "HintId");

            migrationBuilder.CreateIndex(
                name: "IX_HintTagTable_TagId",
                table: "HintTagTable",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HintTagTable");

            migrationBuilder.DropTable(
                name: "HintTable");

            migrationBuilder.DropTable(
                name: "TagTable");
        }
    }
}
