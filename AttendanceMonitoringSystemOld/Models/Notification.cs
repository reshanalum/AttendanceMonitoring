namespace AttendanceMonitoring.Models
{
    public class Notification
    {
        public string NotificationId { get; set; }
        public string Message { get; set; }

        //Relationships
        public string AttendanceId { get; set; }
        public Attendance AttendanceLink { get; set; }

        public List<Delivered> DeliveredList { get; set; }
    }


}
