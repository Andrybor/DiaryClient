using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Diary.Framework;
using Diary.Framework.Help;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Framework.Infrastructure.Interfaces;

namespace Diary.Teacher
{
    [Plugin("Teacher", DPlugin.Teacher, DPlugin.Map)]
    public class TeacherViewModel : Conductor<IDPlugin>.Collection.OneActive, IDPlugin
    {
        [ImportMany] protected IEnumerable<Lazy<IDPlugin, IPluginMetaData>> _plugins;
        private object _selectedItem;

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

        public IDependencyData Panel { get; } = new DependencyData();


        protected override void OnActivate()
        {
            var id = PluginHelper.ValueOfIdAttribute<TeacherViewModel>();

            PluginHelper.BuildPlugins(_plugins, Items, id);

            SelectedItem = Items[0];

            base.OnActivate();
        }
    }
}