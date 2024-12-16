using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Application_CandidateAccounts_CandidateId",
                table: "Application");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplicants_Application_ApplicationId",
                table: "JobApplicants");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplicants_JobPosting_JobId",
                table: "JobApplicants");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPosting_CompanyAccounts_CompanyId",
                table: "JobPosting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobPosting",
                table: "JobPosting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Application",
                table: "Application");

            migrationBuilder.RenameTable(
                name: "JobPosting",
                newName: "JobPostings");

            migrationBuilder.RenameTable(
                name: "Application",
                newName: "Applications");

            migrationBuilder.RenameIndex(
                name: "IX_JobPosting_CompanyId",
                table: "JobPostings",
                newName: "IX_JobPostings_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Application_CandidateId",
                table: "Applications",
                newName: "IX_Applications_CandidateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobPostings",
                table: "JobPostings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_CandidateAccounts_CandidateId",
                table: "Applications",
                column: "CandidateId",
                principalTable: "CandidateAccounts",
                principalColumn: "CandidateID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicants_Applications_ApplicationId",
                table: "JobApplicants",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicants_JobPostings_JobId",
                table: "JobApplicants",
                column: "JobId",
                principalTable: "JobPostings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_CompanyAccounts_CompanyId",
                table: "JobPostings",
                column: "CompanyId",
                principalTable: "CompanyAccounts",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_CandidateAccounts_CandidateId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplicants_Applications_ApplicationId",
                table: "JobApplicants");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplicants_JobPostings_JobId",
                table: "JobApplicants");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_CompanyAccounts_CompanyId",
                table: "JobPostings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobPostings",
                table: "JobPostings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.RenameTable(
                name: "JobPostings",
                newName: "JobPosting");

            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "Application");

            migrationBuilder.RenameIndex(
                name: "IX_JobPostings_CompanyId",
                table: "JobPosting",
                newName: "IX_JobPosting_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_CandidateId",
                table: "Application",
                newName: "IX_Application_CandidateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobPosting",
                table: "JobPosting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Application",
                table: "Application",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Application_CandidateAccounts_CandidateId",
                table: "Application",
                column: "CandidateId",
                principalTable: "CandidateAccounts",
                principalColumn: "CandidateID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicants_Application_ApplicationId",
                table: "JobApplicants",
                column: "ApplicationId",
                principalTable: "Application",
                principalColumn: "ApplicationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplicants_JobPosting_JobId",
                table: "JobApplicants",
                column: "JobId",
                principalTable: "JobPosting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosting_CompanyAccounts_CompanyId",
                table: "JobPosting",
                column: "CompanyId",
                principalTable: "CompanyAccounts",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
