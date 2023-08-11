using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationsManagement.Data.Migrations
{
    public partial class RenameDefaultVacationDaysColumnAndAddRejectReasonInVacationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DefaultCountOfVacationDays",
                table: "AspNetUsers",
                newName: "VacationDays");

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "VacationRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "VacationRequests");

            migrationBuilder.RenameColumn(
                name: "VacationDays",
                table: "AspNetUsers",
                newName: "DefaultCountOfVacationDays");
        }
    }
}
