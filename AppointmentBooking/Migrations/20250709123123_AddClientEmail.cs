using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddClientEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReminderDayBeforeSent",
                table: "appointement",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReminderOneHourSent",
                table: "appointement",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderDayBeforeSent",
                table: "appointement");

            migrationBuilder.DropColumn(
                name: "ReminderOneHourSent",
                table: "appointement");
        }
    }
}
