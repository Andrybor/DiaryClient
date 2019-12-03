using System;

namespace Diary.Repositories.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int AccountId { get; set; }
        public bool? Sex { get; set; }
        public DateTime CreatingDay { get; set; }
        public byte[] Image { get; set; }
        public string FullName => Name + " " + Surname;
        public DateTime Birthday { get; set; }
    }
}