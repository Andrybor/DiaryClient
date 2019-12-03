using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Diary.Common;
using Diary.Framework.Help;
using Diary.Framework.Infrastructure.Base;
using Diary.Framework.Infrastructure.Interfaces;

namespace Diary.Framework.Wizard
{
    [Export(typeof(IWizard))]
    public sealed class WizardViewModel : Conductor<IWizzardDPlugin>.Collection.OneActive, IWizard
    {
        [ImportMany] private IEnumerable<Lazy<IWizzardDPlugin, IWizzardMetaData>> _allWizardPages;

        private bool _isBack;
        private bool _isNext;

        private IEnumerable<Lazy<IWizzardDPlugin, IWizzardMetaData>> _usageWizardPages;
        [Import(typeof(IWindowManager))] private IWindowManager _windowManager;


        public bool IsNext
        {
            get => _isNext;
            set
            {
                _isNext = value;
                NotifyOfPropertyChange(nameof(IsNext));
            }
        }

        public bool IsBack
        {
            get => _isBack;
            set
            {
                _isBack = value;
                NotifyOfPropertyChange(nameof(IsBack));
            }
        }

        public int Index { get; private set; }
        public string ParentId { get; private set; }
        public object Entity { get; private set; }

        public void Back()
        {
            Index--;
            GoTo(Index);
        }

        public void GoTo(int index)
        {
            if (index >= 0 && index <= _usageWizardPages.Count() - 1)
            {
                var wizzardScreen = (WizzardScreen) _usageWizardPages.ElementAt(index).Value;
                wizzardScreen.Entity = Entity;
                ActiveItem = wizzardScreen;
                ActiveItem.DisplayName = _usageWizardPages.ElementAt(index).Metadata.Name.Translate();
            }

            if (index <= 0) Index = 0;

            if (index >= _usageWizardPages.Count() - 1) Index = _usageWizardPages.Count() - 1;
        }

        public void Next()
        {
            Index++;
            GoTo(Index);
        }

        public bool? CreateAndShowWizardDialog(string parentId, string title, object dataContext)
        {
            dynamic settings = new ExpandoObject();
            settings.Title = title.Translate();
            settings.ResizeMode = ResizeMode.NoResize;
            settings.SizeToContent = SizeToContent.WidthAndHeight;
            settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ParentId = parentId;
            Entity = dataContext;
            //Settings();
            return _windowManager.ShowDialog(this, null, settings);
        }

        public IScreen GetWizardScreen(string parentId, string title)
        {
            ParentId = parentId;

            Settings();

            return this;
        }

        protected override void OnActivate()
        {
            Settings();

            base.OnActivate();
        }

        public void Exit(bool value)
        {
            this.TryClose(value);
        }

        private void Settings()
        {
            Index = 0;

            _usageWizardPages = _allWizardPages.GetPluginsByParentId(ParentId);

            GoTo(Index);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            foreach (var v in _usageWizardPages)
                v.Value.Deactivate(true);
            Items.Clear();
        }
    }
}