namespace AttendanceMonitoring.Models
{
    public class Attendance
    {
        public string AttendanceId { get; set; }

        public DateTime DateTime { get; set; }
        public string Status { get; set; } //either IN or OUT

        //Relationships
        public string StudentLRN { get; set; }
        public Student StudentLink { get; set; }

        public List<Notification> NotificationList { get; set; }

    }


}
