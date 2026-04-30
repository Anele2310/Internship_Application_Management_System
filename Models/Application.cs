namespace Internship_Application_Management_System.Models
{
    public class Application
    {
        public int Id { get; set; }

        public int InternshipId { get; set; }
        public Internship? Internship { get; set; }

        public string? UserId { get; set; }

        public DateTime AppliedDate { get; set; } = DateTime.Now;

        public string? CVFilePath { get; set; }
    }
}
