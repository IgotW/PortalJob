using Microsoft.EntityFrameworkCore;
using JobSystem.Models;

namespace JobSystem.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 

        }
        public DbSet<CandidateAccount> CandidateAccounts { get; set; }
        public DbSet<CompanyAccount> CompanyAccounts { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<JobApplicants> JobApplicants { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<JobSystem.Models.JobPostingViewModel> JobPostingViewModel { get; set; } = default!;
    }
}
