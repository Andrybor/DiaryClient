using System;

namespace Diary.Framework.Exceptions
{
    public class InternalServerError : Exception
    {
        public InternalServerError(Exception exception) : base(exception.Message)
        {
        }
    }
}