using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hrbs_project.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_features",
                table: "features");

            migrationBuilder.DropPrimaryKey(
                name: "PK_facilities",
                table: "facilities");

            migrationBuilder.RenameTable(
                name: "features",
                newName: "Features");

            migrationBuilder.RenameTable(
                name: "facilities",
                newName: "Facilities");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "rooms",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "rooms",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "rooms",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "features",
                table: "rooms",
                newName: "Features");

            migrationBuilder.RenameColumn(
                name: "facilities",
                table: "rooms",
                newName: "Facilities");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "rooms",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "children",
                table: "rooms",
                newName: "Children");

            migrationBuilder.RenameColumn(
                name: "adult",
                table: "rooms",
                newName: "Adult");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "rooms",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "UserCred",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pincode",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_Address",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_Email",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_Name",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_Password",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_Phone",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Facilities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "icon",
                table: "Facilities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "room_id",
                table: "booking_order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "booking_order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Features",
                table: "Features",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facilities",
                table: "Facilities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "booking_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    room_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_details", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "carousel",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carousel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contact_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact_details", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rating_review",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    review = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rating_review", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "room_images",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsShutdown = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleMap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "team_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_details", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_queries",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_queries", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "booking_details");

            migrationBuilder.DropTable(
                name: "carousel");

            migrationBuilder.DropTable(
                name: "contact_details");

            migrationBuilder.DropTable(
                name: "rating_review");

            migrationBuilder.DropTable(
                name: "room_images");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "team_details");

            migrationBuilder.DropTable(
                name: "user_queries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Features",
                table: "Features");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facilities",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "UserCred");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "UserCred");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "UserCred");

            migrationBuilder.DropColumn(
                name: "Pincode",
                table: "UserCred");

            migrationBuilder.DropColumn(
                name: "User_Address",
                table: "UserCred");

            migrationBuilder.DropColumn(
                name: "User_Email",
                table: "UserCred");

            migrationBuilder.DropColumn(
                name: "User_Name",
                table: "UserCred");

            migrationBuilder.DropColumn(
                name: "User_Password",
                table: "UserCred");

            migrationBuilder.DropColumn(
                name: "User_Phone",
                table: "UserCred");

            migrationBuilder.DropColumn(
                name: "room_id",
                table: "booking_order");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "booking_order");

            migrationBuilder.RenameTable(
                name: "Features",
                newName: "features");

            migrationBuilder.RenameTable(
                name: "Facilities",
                newName: "facilities");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "rooms",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "rooms",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "rooms",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "Features",
                table: "rooms",
                newName: "features");

            migrationBuilder.RenameColumn(
                name: "Facilities",
                table: "rooms",
                newName: "facilities");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "rooms",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Children",
                table: "rooms",
                newName: "children");

            migrationBuilder.RenameColumn(
                name: "Adult",
                table: "rooms",
                newName: "adult");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "rooms",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "UserCred",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "facilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "icon",
                table: "facilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_features",
                table: "features",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_facilities",
                table: "facilities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });
        }
    }
}
