using System.ComponentModel.DataAnnotations;

namespace JobSystem.Models
{
    public class JobPostingViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Type { get; set; }

        public string Salary { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime PostedDate { get; set; }
    }
}
