using System;
using System.Collections.Generic;
using Diary.Common;

namespace Diary.Framework.Help
{
    public static class EnumHelper
    {
        public static IEnumerable<string> EnumAsString(Type enumType)
        {
            var translated = new List<string>();
            foreach (var text in enumType.GetEnumNames())
                if (!string.IsNullOrEmpty(text.Translate()))
                    translated.Add(text.Translate());
                else
                    translated.Add(text);

            return translated;
        }
    }
}