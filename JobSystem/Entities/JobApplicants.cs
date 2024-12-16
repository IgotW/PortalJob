using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Entities
{
	public class JobApplicants
	{
		[Key]
		public int JobApplicantId { get; set; }
		[ForeignKey("JobPostings")]
		public int JobId { get; set; }
		[ForeignKey("Application")]
		public int ApplicationId { get; set; }
		public string ApplicationStatus { get; set; }

		public JobPosting JobPostings { get; set; }
		public Application Application { get; set; }
	}
}
