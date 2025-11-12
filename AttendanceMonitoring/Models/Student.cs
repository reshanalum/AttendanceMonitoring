namespace AttendanceMonitoring.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LRN { get; set; }
        public string RFID { get; set; }
        public string EnrollmentStatus { get; set; }

        //Relationships
        public List<Attendance> AttendanceList { get; set; }
        public List<Relationship> RelationshipList { get; set; }

        public List<Advisory> AdvisoryList { get; set; }
    }


}
