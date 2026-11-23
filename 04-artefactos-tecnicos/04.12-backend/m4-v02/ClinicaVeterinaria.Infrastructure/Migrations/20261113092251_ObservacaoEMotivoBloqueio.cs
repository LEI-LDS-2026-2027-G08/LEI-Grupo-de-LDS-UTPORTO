using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicaVeterinaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ObservacaoEMotivoBloqueio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotivoBloqueio",
                table: "Utilizador",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "Marcacao",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoBloqueio",
                table: "Utilizador");

            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "Marcacao");
        }
    }
}
