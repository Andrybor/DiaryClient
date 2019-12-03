namespace Diary.Repositories.Models
{
    public class News
    {
        public int Id { get; set; }
        public int? AccountTypeId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}