namespace Diary.Framework.Infrastructure.Interfaces
{
    public interface IPluginMetaData
    {
        object Icon { get; }
        string Id { get; }
        string Name { get; }
        string ParentId { get; }
        int Priority { get; }
    }
}