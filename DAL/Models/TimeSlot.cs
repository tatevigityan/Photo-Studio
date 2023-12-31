namespace DAL.Models
{
    public class TimeSlot
    {
        public string text { get; set; }

        public int startTime { get; set; }

        public bool isSelected { get; set; }

        public TimeSlot() { }
    }
}
