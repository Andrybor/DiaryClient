using System;
using System.Linq;
using System.Windows;
using Diary.Resources;
using MahApps.Metro;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Theme = Diary.Common.Enums.Theme;

namespace Diary.Settings
{
    public static class SettingsHelper
    {
        public static void SetTheme(string theme, string style)
        {
            ModifyTheme(i => i.SetBaseTheme(StringToMaterialTheme(theme)));
            ThemeManager.ChangeAppStyle(Application.Current,
                ThemeManager.GetAccent(style),
                ThemeManager.GetAppTheme(theme));
        }

        public static void SetStyle(string style, string theme)
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                ThemeManager.GetAccent(style),
                ThemeManager.GetAppTheme(theme));

            var paletteHelper = new PaletteHelper();
            var swatchesProvider = new SwatchesProvider();
            var color = swatchesProvider.Swatches.FirstOrDefault(a => a.Name == style.ToLower());
            paletteHelper.ReplacePrimaryColor(color);
        }

        public static void SetLanguage(string language)
        {
            LanguageHelper.SelectCulture(language);
        }

        public static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }

        public static void InitSettings(Common.SettingsHelper.Settings settings)
        {
            SetTheme(settings.Theme, settings.Style);
            SetStyle(settings.Style, settings.Theme);
            SetLanguage(settings.Language);
        }

        private static IBaseTheme StringToMaterialTheme(string theme)
        {
            if (theme == Theme.BaseLight.ToString())
                return MaterialDesignThemes.Wpf.Theme.Light;
            return MaterialDesignThemes.Wpf.Theme.Dark;
        }

        public static string BoolToStringTheme(bool isTheme)
        {
            if (isTheme)
                return Theme.BaseDark.ToString();
            return Theme.BaseLight.ToString();
        }

        public static bool StringToBoolTheme(string theme)
        {
            if (theme == Theme.BaseLight.ToString())
                return false;
            return true;
        }
    }
}