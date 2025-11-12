namespace AttendanceMonitoring.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; }

        //Relationships
        public int AttendanceId { get; set; }
        public Attendance AttendanceLink { get; set; }

        public List<Delivered> DeliveredList { get; set; }
    }


}
