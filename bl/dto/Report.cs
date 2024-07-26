using Microsoft.AspNetCore.Http;

namespace bl.dto
{
    public class Report
    {
        public Guid UserGenerateReport { get; set; }
        public string NameFileGenerateReport { get; set; }
        public DateTime DateGenerateReport { get; set; } = DateTime.Now;
        public bool PDF { get; set; }
        public bool XMLS { get; set; }
        public bool CSV { get; set; }
        public string filter { get; set; }
        public string Status { get; set; } // Pending / Success
        public string ReportNotEmpty { get; set; }

    }

}
