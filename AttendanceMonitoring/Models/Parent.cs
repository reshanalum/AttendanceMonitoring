namespace AttendanceMonitoring.Models
{
    public class Parent
    {
        public int ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Relationships
        public List<Contact> ContactList { get; set; }
        public List<Relationship> RelationshipList { get; set; }


    }


}
