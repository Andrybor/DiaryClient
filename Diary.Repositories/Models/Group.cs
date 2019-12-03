using System.Collections.Generic;

namespace Diary.Repositories.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? SpecializationId { get; set; }
        public int? CourseId { get; set; }
        public int? Semester { get; set; }
        public string Course { get; set; }

        public List<Student> Students { get; set; }
    }
}