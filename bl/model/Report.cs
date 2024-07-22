namespace bl.model
{
    public class Report
    {
        public Guid Id { get; set; }
        public string UserGenerateReport { get; set; }
        public string NameFileGenerateReport { get; set; }
        public DateTime DateGenerateReport { get; set; }
        public string PDF { get; set; }
        public string XMLS { get; set; }
        public string CSV { get; set; }
    }
}
