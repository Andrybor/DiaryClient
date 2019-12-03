using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Diary.Framework;
using Diary.Framework.Help;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Framework.Infrastructure.Interfaces;

namespace Diary.Teacher.Menu
{
    [Plugin("Main", DPlugin.MainTeacher, DPlugin.Teacher)]
    internal class MainTeacherViewModel : Conductor<IDPlugin>.Collection.OneActive, IDPlugin
    {
        private bool _isMenuOpen;
        [ImportMany] protected IEnumerable<Lazy<IDPlugin, IPluginMetaData>> _plugins;
        private object _selectedItem;

        public bool IsMenuOpen
        {
            get => _isMenuOpen;
            set
            {
                _isMenuOpen = value;
                NotifyOfPropertyChange(() => IsMenuOpen);
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

        public object Icon { get; set; }

        public IDependencyData Panel { get; } = new DependencyData();

        protected override void OnActivate()
        {
            var id = PluginHelper.ValueOfIdAttribute<MainTeacherViewModel>();

            PluginHelper.BuildPlugins(_plugins, Items, id);

            SelectedItem = Items[0];

            IsMenuOpen = false;

            base.OnActivate();
        }
    }
}