using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace commitment_calendar_api.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_UserId_StartsAt",
                table: "Appointments");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId_IsDeleted_StartsAt",
                table: "Appointments",
                columns: new[] { "UserId", "IsDeleted", "StartsAt" })
                .Annotation("SqlServer:Include", new[] { "AppointmentId", "Name", "Description", "EndsAt", "Stake" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_UserId_IsDeleted_StartsAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId_StartsAt",
                table: "Appointments",
                columns: new[] { "UserId", "StartsAt" })
                .Annotation("SqlServer:Include", new[] { "AppointmentId", "Name", "Description", "EndsAt", "Stake" });
        }
    }
}
