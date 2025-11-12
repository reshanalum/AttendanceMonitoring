namespace AttendanceMonitoring.Models
{
    public class Class_Adviser
    {
        public int ClassAdviserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        //Relationships
        public List<Advisory> AdvisoryList { get; set; }

    }


}
