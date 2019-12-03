using Caliburn.Micro;

namespace Diary.Framework.Infrastructure.Common
{
    public static class EventAggregator
    {
        public static IEventAggregator Current { get; set; }
    }
}