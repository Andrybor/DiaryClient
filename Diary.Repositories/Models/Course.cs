namespace Diary.Repositories.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? SubjectId { get; set; }
        public int? SpecializationId { get; set; }
        public int? AmountOfHours { get; set; }
        public string Specialization { get; set; }
    }
}