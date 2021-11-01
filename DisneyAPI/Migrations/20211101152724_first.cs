using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DisneyAPI.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "disney");

            migrationBuilder.CreateTable(
                name: "Characters",
                schema: "disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Story = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                schema: "disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieOrSeries",
                schema: "disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imagen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieOrSeries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterMovieOrSerie",
                schema: "disney",
                columns: table => new
                {
                    CharactersId = table.Column<int>(type: "int", nullable: false),
                    MovieOrSeriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterMovieOrSerie", x => new { x.CharactersId, x.MovieOrSeriesId });
                    table.ForeignKey(
                        name: "FK_CharacterMovieOrSerie_Characters_CharactersId",
                        column: x => x.CharactersId,
                        principalSchema: "disney",
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterMovieOrSerie_MovieOrSeries_MovieOrSeriesId",
                        column: x => x.MovieOrSeriesId,
                        principalSchema: "disney",
                        principalTable: "MovieOrSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenreMovieOrSerie",
                schema: "disney",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    MovieOrSeriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreMovieOrSerie", x => new { x.GenreId, x.MovieOrSeriesId });
                    table.ForeignKey(
                        name: "FK_GenreMovieOrSerie_Genders_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "disney",
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreMovieOrSerie_MovieOrSeries_MovieOrSeriesId",
                        column: x => x.MovieOrSeriesId,
                        principalSchema: "disney",
                        principalTable: "MovieOrSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterMovieOrSerie_MovieOrSeriesId",
                schema: "disney",
                table: "CharacterMovieOrSerie",
                column: "MovieOrSeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreMovieOrSerie_MovieOrSeriesId",
                schema: "disney",
                table: "GenreMovieOrSerie",
                column: "MovieOrSeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterMovieOrSerie",
                schema: "disney");

            migrationBuilder.DropTable(
                name: "GenreMovieOrSerie",
                schema: "disney");

            migrationBuilder.DropTable(
                name: "Characters",
                schema: "disney");

            migrationBuilder.DropTable(
                name: "Genders",
                schema: "disney");

            migrationBuilder.DropTable(
                name: "MovieOrSeries",
                schema: "disney");
        }
    }
}
