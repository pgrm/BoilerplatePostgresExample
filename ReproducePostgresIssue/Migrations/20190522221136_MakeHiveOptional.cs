using Microsoft.EntityFrameworkCore.Migrations;

namespace ReproducePostgresIssue.Migrations
{
    public partial class MakeHiveOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HiveId",
                table: "Queens",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "HiveId",
                table: "Ants",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HiveId",
                table: "Queens",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HiveId",
                table: "Ants",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
