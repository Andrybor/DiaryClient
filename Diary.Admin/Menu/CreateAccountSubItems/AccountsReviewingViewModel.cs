using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Diary.Common;
using Diary.Framework;
using Diary.Framework.Help;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Framework.Wizard;
using Diary.Repositories;
using Diary.Repositories.Enums;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;

namespace Diary.Admin.Menu.CreateAccountSubItems
{
    [Plugin("AllAccounts", DPlugin.AllAccounts, DPlugin.Create, 1, PackIconModernKind.ListGear)]
    internal class AccountsReviewingViewModel : DScreen
    {
        private ObservableCollection<Account> _accounts;
        private string _countAccounts;
        private string _countTypeAccounts;

        private string _selectedAccountType;
        [Import(typeof(WizardManager))] private WizardManager wizardManager;

        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                NotifyOfPropertyChange(() => Accounts);
            }
        }

        public string CountTypeAccounts
        {
            get => _countTypeAccounts;
            set
            {
                _countTypeAccounts = value;
                NotifyOfPropertyChange(() => CountTypeAccounts);
            }
        }

        public string CountAccounts
        {
            get => _countAccounts;
            set
            {
                _countAccounts = value;
                NotifyOfPropertyChange(() => CountAccounts);
            }
        }

        public string SelectedAccountType
        {
            get => _selectedAccountType;
            set
            {
                _selectedAccountType = value;
                NotifyOfPropertyChange(nameof(SelectedAccountType));
                InitAccounts();
            }
        }

        public IEnumerable<string> AccountTypes => EnumHelper.EnumAsString(typeof(TypeAccount));

        protected override void OnActivate()
        {
            InitAccounts();

            SelectedAccountType = AccountTypes.FirstOrDefault();

            base.OnActivate();
        }

        private async void InitAccounts()
        {
            var accounts = await SimpleService.Get<ObservableCollection<Account>>(Controller.Accounts, Method.GetAll);

            if (accounts != null)
            {
                Accounts = new ObservableCollection<Account>(accounts.Where(x =>
                    x.AccountType.Type == SelectedAccountType.GetKey()));

                CountTypeAccounts = "Count".Translate() + " " + SelectedAccountType.Translate() + " " +
                                    "Accounts".Translate() + " : " + Accounts?.Count;

                CountAccounts = "Count".Translate() + " " + "AllAccounts".Translate() + " : " + accounts?.Count;
            }
        }

        public async void Delete(object item)
        {
            if (item != null && item is Account account)
            {
                await SimpleService.Post(Controller.Accounts, Method.Delete, account.Id);

                InitAccounts();
            }
        }

        public void Edit(object item)
        {
            wizardManager.Show("EditAccountWizzard", item, "EditAccount");
        }
    }
}