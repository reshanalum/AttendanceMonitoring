namespace AttendanceMonitoring.Models
{
    public class Relationship
    {
        public int RelationshipId { get; set; }

        public string RelationshipType { get; set; }

        //Relationships
        public int StudentId { get; set; }
        public Student StudentLink { get; set; }
        public int ParentId { get; set; }
        public Parent ParentLink { get; set; }
    }


}
