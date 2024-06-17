using System.ComponentModel.DataAnnotations;

namespace DataExporter.Dtos
{
    public class CreatePolicyDto
    {
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string PolicyNumber { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Premium must be greater than zero.")]
        public decimal Premium { get; set; }
        public DateTime StartDate { get; set; }
        public List<NoteDto> Notes { get; set; }
    }

    public class NoteDto
    {
        public string Content { get; set; }
    }
}
