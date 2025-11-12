namespace AttendanceMonitoring.Models
{
    public class Contact
    {
        public string ContactId { get; set; }
        public string PhoneNumber { get; set; }
        public string Network { get; set; }

        //Relationships
        public string ParentId { get; set; }
        public Parent ParentLink { get; set; }
        public List<Delivered> DeliveredList { get; set; }
    }


}
