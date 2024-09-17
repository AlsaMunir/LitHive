using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication6.Data.Migrations
{
    /// <inheritdoc />
    public partial class newcolumnbookmark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
     name: "bmId",
     table: "CartItems",
     type: "int",
     nullable: true,
     oldClrType: typeof(int),
     oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BookmarkId",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_bmId",
                table: "CartItems",
                column: "bmId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Bookmarks_bmId",
                table: "CartItems",
                column: "bmId",
                principalTable: "Bookmarks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Bookmarks_bmId",
                table: "CartItems");

            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_bmId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "BookmarkId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "bmId",
                table: "CartItems");
        }
    }
}
