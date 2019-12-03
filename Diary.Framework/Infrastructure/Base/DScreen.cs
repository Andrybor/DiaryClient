using Caliburn.Micro;
using Diary.Framework.Infrastructure.Interfaces;

namespace Diary.Framework.Infrastructure.Base
{
    public class DScreen : Screen, IDPlugin
    {
        public IDependencyData Panel { get; } = new DependencyData();
    }
}