using Caliburn.Micro;

namespace Diary.Framework.Infrastructure.Interfaces
{
    public interface IDPlugin : IScreen
    {
        IDependencyData Panel { get; }
    }
}