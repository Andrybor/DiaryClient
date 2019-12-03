using System;

namespace Diary.Framework.Infrastructure.Interfaces
{
    public interface ILoadingScreen
    {
        Action CloseLoadingScreen { get; set; }
    }
}
