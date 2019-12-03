using System.IO;
using Diary.Common.Helpers;

namespace Diary.Common.SettingsHelper
{
    public class AppSettings<T> where T : new()
    {
        private const string DEFAULT_FILENAME = "settings.json";

        public void Save(string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, JsonHelper.Serialize(this));
        }

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, JsonHelper.Serialize(pSettings));
        }

        public static T Load(string fileName = DEFAULT_FILENAME)
        {
            var t = new T();
            if (File.Exists(fileName))
                t = JsonHelper.Deserialize<T>(File.ReadAllText(fileName));
            return t;
        }
    }
}