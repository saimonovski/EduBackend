using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LearningPlanInitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionMetadata_MetadataId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Questions_QuestionId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Question");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_QuestionId",
                table: "Question",
                newName: "IX_Question_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_MetadataId",
                table: "Question",
                newName: "IX_Question_MetadataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LearningSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PresentationId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimplifiedTask",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PresentationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearningSessionId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimplifiedTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimplifiedTask_LearningSessions_LearningSessionId",
                        column: x => x.LearningSessionId,
                        principalTable: "LearningSessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OptionItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SimplifiedTaskId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OptionItem_SimplifiedTask_SimplifiedTaskId",
                        column: x => x.SimplifiedTaskId,
                        principalTable: "SimplifiedTask",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OptionItem_SimplifiedTaskId",
                table: "OptionItem",
                column: "SimplifiedTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_SimplifiedTask_LearningSessionId",
                table: "SimplifiedTask",
                column: "LearningSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_QuestionMetadata_MetadataId",
                table: "Question",
                column: "MetadataId",
                principalTable: "QuestionMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Question_QuestionId",
                table: "Question",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuestionMetadata_MetadataId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Question_QuestionId",
                table: "Question");

            migrationBuilder.DropTable(
                name: "OptionItem");

            migrationBuilder.DropTable(
                name: "SimplifiedTask");

            migrationBuilder.DropTable(
                name: "LearningSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Questions");

            migrationBuilder.RenameIndex(
                name: "IX_Question_QuestionId",
                table: "Questions",
                newName: "IX_Questions_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_MetadataId",
                table: "Questions",
                newName: "IX_Questions_MetadataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryId",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionMetadata_MetadataId",
                table: "Questions",
                column: "MetadataId",
                principalTable: "QuestionMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Questions_QuestionId",
                table: "Questions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
