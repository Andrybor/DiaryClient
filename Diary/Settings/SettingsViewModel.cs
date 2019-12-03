using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Diary.Common.Enums;
using Diary.Framework.Help;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories.Enums;
using ModernMessageBoxLib;

namespace Diary.Settings
{
    public class SettingsViewModel : DScreen
    {
        private bool _isTheme;
        private string _language;
        private string _style;


        public IEnumerable<string> Styles => EnumHelper.EnumAsString(typeof(Styles));

        public IEnumerable<string> Languages => EnumHelper.EnumAsString(typeof(Language));
        public Common.SettingsHelper.Settings SettingsInfo { get; set; }

        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    NotifyOfPropertyChange(() => Language);
                    SetLanguage();
                }
            }
        }

        public string Style
        {
            get => _style;
            set
            {
                if (_style != value)
                {
                    _style = value;
                    NotifyOfPropertyChange(() => Style);
                    SetStyle();
                }
            }
        }

        public bool IsTheme
        {
            get => _isTheme;
            set
            {
                _isTheme = value;
                NotifyOfPropertyChange(() => IsTheme);
                SettingsInfo.Theme = SettingsHelper.BoolToStringTheme(IsTheme);
                SetTheme();
            }
        }



        public CompositionContainer container;

        public void LoadExtension()
        {
            var file = DllDynamicHelper.FindDLL();
            DllDynamicHelper.LoadExtension(file);

            QModernMessageBox.Show("Please, restart the application", "Extension was loaded successfull",
                QModernMessageBox.QModernMessageBoxButtons.Ok);
        }
    
       
        protected override void OnActivate()
        {
            SettingsInfo = Common.SettingsHelper.Settings.Load();

            Language = SettingsInfo.Language;
            Style = SettingsInfo.Style;
            IsTheme = SettingsHelper.StringToBoolTheme(SettingsInfo.Theme);

            base.OnActivate();
        }

        private void SetTheme()
        {
            SettingsHelper.SetTheme(SettingsInfo.Theme, SettingsInfo.Style);

            SettingsInfo.Theme = SettingsHelper.BoolToStringTheme(IsTheme);
            SettingsInfo.Save();
        }

        private void SetStyle()
        {
            SettingsHelper.SetStyle(Style, SettingsInfo.Theme);

            SettingsInfo.Style = Style;

            SettingsInfo.Save();
        }

        private void SetLanguage()
        {
            SettingsHelper.SetLanguage(Language);

            SettingsInfo.Language = Language;

            SettingsInfo.Save();
        }
    }
}