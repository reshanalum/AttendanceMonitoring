namespace AttendanceMonitoring.Models
{
    public class Advisory
    {
        public string AdvisoryId { get; set; }

        public string SectionName { get; set; }
        public string SchoolYear { get; set; }

        //Relationships
        public string ClassAdviserId { get; set; }
        public Class_Adviser ClassAdviserLink { get; set; }
        public string StudentId { get; set; }
        public Student StudentLink { get; set; }


    }


}
