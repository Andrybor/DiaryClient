namespace Diary.Framework.Infrastructure.Interfaces
{
    public interface IDependencyData
    {
        object Icon { get; set; }
        string Id { get; set; }

        void SetData(string id, object icon);
    }
}