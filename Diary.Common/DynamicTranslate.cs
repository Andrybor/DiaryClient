using System;
using System.Collections;
using System.Linq;
using System.Windows;
using Diary.Common.SettingsHelper;

namespace Diary.Common
{
    public static class DynamicTranslate
    {
        public static string Translate(this string resourseKey)
        {
            var dict = new ResourceDictionary();

            var settings = Settings.Load();

            dict.Source =
                new Uri($"pack://application:,,,/Diary.Resources;component/Languages/{settings.Language}.xaml");

            var languageResourse =
                Application.Current.Resources.MergedDictionaries.FirstOrDefault(i => i.Source == dict.Source);
            if (languageResourse != null && languageResourse["Txt" + resourseKey] != null)
                return languageResourse["Txt" + resourseKey].ToString();

            return resourseKey;
        }

        public static string GetKey(this string value)
        {
            var dict = new ResourceDictionary();

            var settings = Settings.Load();

            dict.Source =
                new Uri($"pack://application:,,,/Diary.Resources;component/Languages/{settings.Language}.xaml");

            var languageResourse =
                Application.Current.Resources.MergedDictionaries.FirstOrDefault(i => i.Source == dict.Source);
            if (languageResourse != null)
                foreach (DictionaryEntry entry in languageResourse)
                    if (entry.Value.ToString() == value)
                        return entry.Key.ToString().Replace("Txt", "");

            return string.Empty;
        }
    }
}