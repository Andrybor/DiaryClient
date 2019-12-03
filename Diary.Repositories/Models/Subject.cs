using System;

namespace Diary.Repositories.Models
{
    public class Subject 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? CourseId { get; set; }
        public string Course { get; set; }
    }
}