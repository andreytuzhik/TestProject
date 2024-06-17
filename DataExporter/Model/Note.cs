﻿namespace DataExporter.Model
{
    public sealed class Note
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int PolicyId { get; set; }
        public Policy Policy { get; set; }
    }
}
