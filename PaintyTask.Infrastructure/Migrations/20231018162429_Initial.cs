using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PaintyTask.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    UserSenderId = table.Column<int>(type: "int", nullable: false),
                    UserReceiverId = table.Column<int>(type: "int", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => new { x.UserSenderId, x.UserReceiverId });
                    table.ForeignKey(
                        name: "FK_Friendships_Users_UserReceiverId",
                        column: x => x.UserReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_Users_UserSenderId",
                        column: x => x.UserSenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Login", "Name", "Password" },
                values: new object[,]
                {
                    { 1, "user1@example.com", "User1", "User1", "$2a$11$WLh91OsXdVAX.pY4I.CQXeQcYFDTnlunJyKRJ/THzRLI.qVVW6nqi" },
                    { 2, "user2@example.com", "User2", "User2", "$2a$11$rhnoZe9ImM9AsxytdfvOM.IB5Gyvt56oJQa/Vtqj2Mfr778eVdzCS" },
                    { 3, "user3@example.com", "User3", "User3", "$2a$11$3kXhHAh1OCYWtoXMzhbxqOUPXZ.Ne9bgWXXfPyhPVmKYSg8xpJ3ke" },
                    { 4, "user4@example.com", "User4", "User4", "$2a$11$QXEpFM0GUG5igRiKOye5z.ySdEY8ILvbDyhIf0dzy2woPV/HVOIYi" }
                });

            migrationBuilder.InsertData(
                table: "Friendships",
                columns: new[] { "UserReceiverId", "UserSenderId", "IsAccepted" },
                values: new object[,]
                {
                    { 2, 1, true },
                    { 3, 1, false },
                    { 1, 2, false },
                    { 4, 2, true },
                    { 4, 3, false },
                    { 3, 4, true }
                });

            migrationBuilder.InsertData(
                table: "Photos",
                columns: new[] { "Id", "Url", "UserId" },
                values: new object[,]
                {
                    { 1, "/Photos/69850.jpg", 1 },
                    { 2, "/Photos/9acebdc6-126a-4e5b-857e-fcf777e50dd4apple-logo.jpg", 1 },
                    { 3, "/Photos/bd455fb4-0ca9-4c9f-8bcd-7d44647d49f8admin.jpg", 1 },
                    { 4, "/Photos/bdfe5019-5f62-4662-8355-400f5921f2d2bob.jpg", 2 },
                    { 5, "/Photos/brandFon.jpg", 2 },
                    { 6, "/Photos/cmoeeddpcevumpqs.jpg", 3 },
                    { 7, "/Photos/DjNqxwoKLNY.jpg", 3 },
                    { 8, "/Photos/homeFon.jpg", 3 },
                    { 9, "/Photos/images_anubis.jpg", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserReceiverId",
                table: "Friendships",
                column: "UserReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UserId",
                table: "Photos",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
