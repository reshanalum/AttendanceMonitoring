namespace AttendanceMonitoring.Models
{
    public class Relationship
    {
        public string RelationshipId { get; set; }

        //Relationships
        public string StudentLRN { get; set; }
        public Student StudentLink { get; set; }
        public string ParentId { get; set; }
        public Parent ParentLink { get; set; }
    }


}
