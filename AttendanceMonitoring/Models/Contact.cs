namespace AttendanceMonitoring.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public string PhoneNumber { get; set; }

        //Relationships
        public int ParentId { get; set; }
        public Parent ParentLink { get; set; }
        public List<Delivered> DeliveredList { get; set; }
    }


}
