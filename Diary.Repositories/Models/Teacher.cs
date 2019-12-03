namespace Diary.Repositories.Models
{
    public class Teacher
    {
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public User User { get; set; }
        public Subject Subject { get; set; }
    }
}