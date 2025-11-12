namespace AttendanceMonitoring.Models
{
    public class Parent
    {
        public string ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        //Relationships
        public List<Contact> ContactList { get; set; }
        public List<Relationship> RelationshipList { get; set; }


    }


}
