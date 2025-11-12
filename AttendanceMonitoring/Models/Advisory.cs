namespace AttendanceMonitoring.Models
{
    public class Advisory
    {
        public int AdvisoryId { get; set; }

        public string SectionName { get; set; }
        public string SchoolYear { get; set; }

        //Relationships
        public int ClassAdviserId { get; set; }
        public Class_Adviser ClassAdviserLink { get; set; }
        public int StudentId { get; set; }
        public Student StudentLink { get; set; }


    }


}
