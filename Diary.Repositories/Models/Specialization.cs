using System.Collections.Generic;

namespace Diary.Repositories.Models
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<Course> Courses { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}