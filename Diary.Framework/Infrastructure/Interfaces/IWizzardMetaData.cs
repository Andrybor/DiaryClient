namespace Diary.Framework.Infrastructure.Interfaces
{
    public interface IWizzardMetaData
    {
        string Id { get; }
        string Name { get; }
        string ParentId { get; }
        int Priority { get; }
    }
}