using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student.Migrations
{
    public partial class Migration0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InoviceTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InoviceAmount = table.Column<float>(type: "real", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<float>(type: "real", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    phoneNo = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    DepartmentID_FK = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DepartmentID_Std",
                        column: x => x.DepartmentID_FK,
                        principalTable: "students",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    gender = table.Column<int>(type: "int", nullable: false),
                    phoneNo = table.Column<int>(type: "int", nullable: false),
                    balance = table.Column<double>(type: "float", nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    JoinedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Experience = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teachers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DepartmentID_FK = table.Column<int>(type: "int", nullable: true),
                    TeacherID_FK = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TeacherID_subjects",
                        column: x => x.TeacherID_FK,
                        principalTable: "teachers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    SubjectID_FK = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SubjectID_Dep",
                        column: x => x.SubjectID_FK,
                        principalTable: "subjects",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Enrollement",
                columns: table => new
                {
                    StudentID_FK = table.Column<int>(type: "int", nullable: true),
                    SubjectID_FK = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_StudentID_Sub",
                        column: x => x.StudentID_FK,
                        principalTable: "students",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_SubjectID_Std",
                        column: x => x.SubjectID_FK,
                        principalTable: "subjects",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "libraries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentID_FK = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libraries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClassID_Lib",
                        column: x => x.DepartmentID_FK,
                        principalTable: "departments",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_departments_SubjectID_FK",
                table: "departments",
                column: "SubjectID_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollement_StudentID_FK",
                table: "Enrollement",
                column: "StudentID_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollement_SubjectID_FK",
                table: "Enrollement",
                column: "SubjectID_FK");

            migrationBuilder.CreateIndex(
                name: "IX_libraries_DepartmentID_FK",
                table: "libraries",
                column: "DepartmentID_FK");

            migrationBuilder.CreateIndex(
                name: "IX_students_DepartmentID_FK",
                table: "students",
                column: "DepartmentID_FK");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_TeacherID_FK",
                table: "subjects",
                column: "TeacherID_FK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollement");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "libraries");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "teachers");
        }
    }
}
