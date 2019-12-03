namespace Diary.Repositories.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public int? UserId { get; set; }
        public int? Points { get; set; }
        public User User { get; set; }
        public string Group { get; set; }
    }
}
