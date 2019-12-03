using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Caliburn.Micro;
using Diary.About;
using Diary.Framework;
using Diary.Framework.Help;
using Diary.Framework.Infrastructure.Base;
using Diary.Framework.Infrastructure.Interfaces;
using Diary.Repositories;
using Diary.Repositories.Enums;
using Diary.Repositories.Models;
using Diary.Settings;

namespace Diary
{
    [Export(typeof(IMap))]
    public class MapViewModel : Conductor<IDPlugin>.Collection.OneActive, IMap
    {
        [ImportMany] private IEnumerable<Lazy<IDPlugin, IPluginMetaData>> _plugins;

        private string _accountType;
        private User _logedUser;
        private ImageSource _photo;   
        private object _selectedItem;
        private Repositories.Models.Student _student;
        private bool _isStudent;


        private SettingsViewModel SettingsView { get; set; }
        private AboutViewModel AboutView { get; set; }

        public bool IsStudent
        {
            get => _isStudent;
            set
            {
                _isStudent = value;
                NotifyOfPropertyChange(()=> IsStudent);
            }
        }
        public Repositories.Models.Student Student
        {
            get => _student;
            set
            {
                _student = value;
                NotifyOfPropertyChange(()=>Student);
            }
        }

        public User LogedUser
        {
            get => _logedUser;
            set
            {
                if (value != null)
                {
                    _logedUser = value;
                    NotifyOfPropertyChange(nameof(LogedUser));
                }
            }
        }

        public ImageSource Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                NotifyOfPropertyChange(nameof(Photo));
            }
        }

        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value != null)
                {
                    _selectedItem = value;
                    NotifyOfPropertyChange(nameof(SelectedItem));
                    ActiveItem = (IDPlugin) SelectedItem;
                }
            }
        }

        public string AccountType
        {
            get => _accountType;
            set
            {
                _accountType = value;
                ChoiceTypeAccount(value);
                NotifyOfPropertyChange(nameof(AccountType));
            }
        }

        public IDependencyData Panel { get; } = new DependencyData();

        public void LogOut()
        {
            TryClose(true);
        }

        public void About()
        {
            if (SelectedItem == AboutView)
            {
                ChoiceTypeAccount(AccountType);
                return;
            }

            AboutView = new AboutViewModel();
            SelectedItem = AboutView;
        }

        public void ChangeSettings()
        {
            if (SelectedItem == SettingsView)
            {
                ChoiceTypeAccount(AccountType);
                return;
            }

            SettingsView = new SettingsViewModel();
            SelectedItem = SettingsView;
        }

        protected override void OnActivate()
        {
            Initialize();

            base.OnActivate();
        }

        private void Initialize()
        {
            PluginHelper.BuildPlugins(_plugins, Items, DPlugin.Map.ToString());

            ChoiceTypeAccount(AccountType);

            InitStudentInfo(AccountType);
        }


        private async void InitStudentInfo(string accountType)
        {
            if (accountType == TypeAccount.Student.ToString())
            {
                var student = await SimpleService.Get<Repositories.Models.Student>(Controller.Student, Method.Get,
                    SimpleService.LoggedUser.Id);
                if (student!=null && student.Points == null)
                    student.Points = 0;
                Student = student;

                IsStudent = true;
            }
            else
            {
                IsStudent = false;
            }
        }

        private void ChoiceTypeAccount(string typeAccount)
        {
            // fix // check acc by id not acc type
            foreach (var item in Items)
                if (item.Panel.Id == AccountType)
                    SelectedItem = item;
        }
    }
}