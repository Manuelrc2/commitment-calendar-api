using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace commitment_calendar_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdFromIntToLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");
            migrationBuilder.DropIndex(
                name: "IX_Appointments_UserId_StartsAt",
                table: "Appointments");

            migrationBuilder.AlterColumn<long>(
                name: "AppointmentId",
                table: "Appointments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "AppointmentId");
            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId_StartsAt",
                table: "Appointments",
                columns: new[] { "UserId", "StartsAt" })
                .Annotation("SqlServer:Include", new[] { "AppointmentId", "Name", "Description", "EndsAt", "Stake" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_UserId_StartsAt",
                table: "Appointments");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "Appointments",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId_StartsAt",
                table: "Appointments",
                columns: new[] { "UserId", "StartsAt" })
                .Annotation("SqlServer:Include", new[] { "Name", "Description", "EndsAt", "Stake" });
        }
    }
}
