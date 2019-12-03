using System;

namespace Diary.Repositories.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int? SubjectId { get; set; }
        public int? GroupId { get; set; }
        public int? TeacherId { get; set; }
        public int? AuditoriumId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Theme { get; set; }
        public Auditorium Auditorium { get; set; }
        public Group Group { get; set; }
        public Subject Subject { get; set; }
        public User Teacher { get; set; }
    }
}