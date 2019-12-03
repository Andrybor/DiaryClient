using System;

namespace Diary.Framework.Infrastructure.Interfaces
{
    public interface ILogin : IDPlugin
    {
        Func<object, bool> LoginFinished { get; set; }
    }
}