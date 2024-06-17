namespace DataExporter.Model
{
    public sealed class Policy
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public decimal Premium { get; set; }
        public DateTime StartDate { get; set; }
        public ICollection<Note> Notes { get; set; }
    }
}
