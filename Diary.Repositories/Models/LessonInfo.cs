namespace Diary.Repositories.Models
{
    public class LessonInfo
    {
        public int Id { get; set; }
        public int? ScheduleId { get; set; }
        public int? UserId { get; set; }
        public int? Grade { get; set; }
        public bool IsPresent { get; set; }
        public Schedule Schedule { get; set; }
        public User User { get; set; }
    }
}
