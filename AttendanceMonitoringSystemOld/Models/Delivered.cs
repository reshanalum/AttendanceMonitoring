namespace AttendanceMonitoring.Models
{
    public class Delivered
    {
        public string DeliveredId { get; set; }

        public DateTime DateTimeSent { get; set; }


        //Relationships
        public string NotificationId { get; set; }

        public Notification NotificationLink { get; set; }

        public string ContactId { get; set; }

        public Contact ContactLink { get; set; }
    }


}
