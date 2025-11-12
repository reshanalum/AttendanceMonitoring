namespace AttendanceMonitoring.Models
{
    public class Delivered
    {
        public int DeliveredId { get; set; }

        public DateTime DateTimeSent { get; set; }


        //Relationships
        public int NotificationId { get; set; }

        public Notification NotificationLink { get; set; }

        public int ContactId { get; set; }

        public Contact ContactLink { get; set; }
    }


}
