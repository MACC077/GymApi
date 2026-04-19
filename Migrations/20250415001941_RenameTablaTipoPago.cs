using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymControlAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablaTipoPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TiposPagos",
                table: "TiposPagos");

            migrationBuilder.RenameTable(
                name: "TiposPagos",
                newName: "TipoPagos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipoPagos",
                table: "TipoPagos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TipoPagos",
                table: "TipoPagos");

            migrationBuilder.RenameTable(
                name: "TipoPagos",
                newName: "TiposPagos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TiposPagos",
                table: "TiposPagos",
                column: "Id");
        }
    }
}
