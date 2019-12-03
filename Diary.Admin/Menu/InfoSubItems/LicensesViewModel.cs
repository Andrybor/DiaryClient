using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;

namespace Diary.Admin.Menu.InfoSubItems
{
    [Plugin("Licenses", DPlugin.Licenses, DPlugin.Info, 4, PackIconModernKind.Lock)]
    class LicensesViewModel : DScreen
    {
        private ObservableCollection<License> _licenses;

        public ObservableCollection<License> Licenses
        {
            get => _licenses;
            set
            {
                _licenses = value;
                NotifyOfPropertyChange(()=> Licenses);
            }
        }

        private async void InitLicenses()
        {
            var licenses = await SimpleService.Get<List<License>>(Controller.Licenses, Method.GetAll);
            if (licenses != null)
            {
                Licenses = new ObservableCollection<License>(licenses.Where(i => i.IsAssigned == false));
            }
        }

        protected override void OnActivate()
        {
            InitLicenses();

            base.OnActivate();
        }

        public async void GenerateLicenses()
        {
            // create for default 10 licenses
            await SimpleService.Post(Controller.Licenses, Method.Create, 10);

            InitLicenses();
        }

        public async void AssignLicense(object item)
        {
            if (item is License license)
            {
                await SimpleService.Post(Controller.Licenses, Method.AssignLicense, license.Id);

                InitLicenses();
            }
        }

    }

}
