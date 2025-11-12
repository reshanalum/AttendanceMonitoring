namespace AttendanceMonitoring.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }

        public DateTime DateTime { get; set; }
        public string Status { get; set; } //either IN or OUT

        //Relationships
        public int StudentId { get; set; }
        public Student StudentLink { get; set; }

        public List<Notification> NotificationList { get; set; }

    }


}
