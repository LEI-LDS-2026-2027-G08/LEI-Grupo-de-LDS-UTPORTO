using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClinicaVeterinaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class I062DadosReferencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clinica",
                columns: new[] { "Id", "DataUltimaAlteracao", "Email", "Morada", "NIF", "Nome", "Telefone" },
                values: new object[] { (byte)1, new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "geral@clinica-g08.test", "Rua do Exemplo, 100, 4470-000 Maia", "501234560", "Clínica Veterinária G08", "229000000" });

            migrationBuilder.InsertData(
                table: "ConfiguracaoSistema",
                columns: new[] { "Chave", "DataUltimaAlteracao", "Descricao", "Valor" },
                values: new object[] { "DuracaoPadraoConsultaMinutos", new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Duração por omissão de uma consulta, em minutos (EP-009 e EP-010).", "30" });

            migrationBuilder.InsertData(
                table: "Especie",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Cão" },
                    { 2, "Gato" }
                });

            migrationBuilder.InsertData(
                table: "Raca",
                columns: new[] { "Id", "EspecieId", "Nome" },
                values: new object[,]
                {
                    { 1, 1, "Labrador Retriever" },
                    { 2, 1, "Pastor Alemão" },
                    { 3, 1, "Sem raça definida" },
                    { 4, 2, "Europeu Comum" },
                    { 5, 2, "Siamês" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clinica",
                keyColumn: "Id",
                keyValue: (byte)1);

            migrationBuilder.DeleteData(
                table: "ConfiguracaoSistema",
                keyColumn: "Chave",
                keyValue: "DuracaoPadraoConsultaMinutos");

            migrationBuilder.DeleteData(
                table: "Raca",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Raca",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Raca",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Raca",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Raca",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Especie",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Especie",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
