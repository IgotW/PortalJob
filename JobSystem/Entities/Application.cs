using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobSystem.Entities
{
	public class Application
	{
		[Key]
		public int ApplicationId { get; set; }
		[ForeignKey("CandidateAccounts")]
		public int CandidateId { get; set; }
		public string Resume { get; set; }
		public string ApplicationDescription { get; set; }
		public DateOnly DateApplied { get; set; }

		public CandidateAccount CandidateAccounts { get; set; }
		public ICollection<JobApplicants> JobApplicants { get; set; }
	}
}
