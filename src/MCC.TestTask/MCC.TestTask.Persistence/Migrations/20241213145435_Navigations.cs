using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MCC.TestTask.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Navigations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityUser_Communities_SubscribedToId",
                table: "CommunityUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityUser_Users_SubscribersId",
                table: "CommunityUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Communities_CommunityId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Posts_PostId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CommunityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PostId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tags_PostId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "SubscribersId",
                table: "CommunityUser",
                newName: "CommunityId");

            migrationBuilder.RenameColumn(
                name: "SubscribedToId",
                table: "CommunityUser",
                newName: "AdministratorsId");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityUser_SubscribersId",
                table: "CommunityUser",
                newName: "IX_CommunityUser_CommunityId");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressObjectId",
                table: "Posts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommunityUser1",
                columns: table => new
                {
                    SubscribedToId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscribersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityUser1", x => new { x.SubscribedToId, x.SubscribersId });
                    table.ForeignKey(
                        name: "FK_CommunityUser1_Communities_SubscribedToId",
                        column: x => x.SubscribedToId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityUser1_Users_SubscribersId",
                        column: x => x.SubscribersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PostTag_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostUser",
                columns: table => new
                {
                    LikedById = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUser", x => new { x.LikedById, x.PostId });
                    table.ForeignKey(
                        name: "FK_PostUser_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUser_Users_LikedById",
                        column: x => x.LikedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityUser1_SubscribersId",
                table: "CommunityUser1",
                column: "SubscribersId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagsId",
                table: "PostTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUser_PostId",
                table: "PostUser",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityUser_Communities_CommunityId",
                table: "CommunityUser",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityUser_Users_AdministratorsId",
                table: "CommunityUser",
                column: "AdministratorsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityUser_Communities_CommunityId",
                table: "CommunityUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityUser_Users_AdministratorsId",
                table: "CommunityUser");

            migrationBuilder.DropTable(
                name: "CommunityUser1");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropTable(
                name: "PostUser");

            migrationBuilder.DropColumn(
                name: "AddressObjectId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "CommunityId",
                table: "CommunityUser",
                newName: "SubscribersId");

            migrationBuilder.RenameColumn(
                name: "AdministratorsId",
                table: "CommunityUser",
                newName: "SubscribedToId");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityUser_CommunityId",
                table: "CommunityUser",
                newName: "IX_CommunityUser_SubscribersId");

            migrationBuilder.AddColumn<Guid>(
                name: "CommunityId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Tags",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CommunityId",
                table: "Users",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PostId",
                table: "Users",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_PostId",
                table: "Tags",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityUser_Communities_SubscribedToId",
                table: "CommunityUser",
                column: "SubscribedToId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityUser_Users_SubscribersId",
                table: "CommunityUser",
                column: "SubscribersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Communities_CommunityId",
                table: "Users",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Posts_PostId",
                table: "Users",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
