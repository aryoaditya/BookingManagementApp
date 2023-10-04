using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class FixUniqueConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_m_employees_nik_email_phone_number",
                table: "tb_m_employees");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_email",
                table: "tb_m_employees",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_phone_number",
                table: "tb_m_employees",
                column: "phone_number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tb_m_employees_email",
                table: "tb_m_employees");

            migrationBuilder.DropIndex(
                name: "IX_tb_m_employees_phone_number",
                table: "tb_m_employees");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_nik_email_phone_number",
                table: "tb_m_employees",
                columns: new[] { "nik", "email", "phone_number" },
                unique: true);
        }
    }
}
