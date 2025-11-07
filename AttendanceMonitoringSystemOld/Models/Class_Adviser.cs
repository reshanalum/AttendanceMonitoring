namespace AttendanceMonitoring.Models
{
    public class Class_Adviser
    {
        public string ClassAdviserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        //Relationships
        public List<Advisory> AdvisoryList { get; set; }

    }


}
