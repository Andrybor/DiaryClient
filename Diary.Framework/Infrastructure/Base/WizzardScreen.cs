using Caliburn.Micro;
using Diary.Framework.Infrastructure.Interfaces;

namespace Diary.Framework.Infrastructure.Base
{
    public class WizzardScreen : Screen, IWizzardDPlugin
    {
        public object Entity { get; set; }
    }
}