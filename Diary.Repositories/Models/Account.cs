namespace Diary.Repositories.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? AccountTypeId { get; set; }

        public AccountType AccountType { get; set; }
        public User User { get; set; }
    }
}