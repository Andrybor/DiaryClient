namespace Diary.Common.SettingsHelper
{
    public class Settings : AppSettings<Settings>
    {
        public string Style { get; set; } = "Blue";
        public string Language { get; set; } = "EN";
        public string Theme { get; set; } = "BaseLight";
    }
}