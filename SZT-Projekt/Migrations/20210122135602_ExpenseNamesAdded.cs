using Microsoft.EntityFrameworkCore.Migrations;

namespace SZT_Projekt.Migrations
{
    public partial class ExpenseNamesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpenseNameId",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExpenseNames",
                columns: table => new
                {
                    ExpenseNameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseNames", x => x.ExpenseNameId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseNameId",
                table: "Expenses",
                column: "ExpenseNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseNames_ExpenseNameId",
                table: "Expenses",
                column: "ExpenseNameId",
                principalTable: "ExpenseNames",
                principalColumn: "ExpenseNameId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseNames_ExpenseNameId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseNames");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpenseNameId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpenseNameId",
                table: "Expenses");
        }
    }
}
