using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Diary.Framework.Help;
using Diary.Framework.Infrastructure.Base;
using Diary.Framework.Infrastructure.Interfaces;
using Diary.LoadingScreen;
using Diary.Repositories;
using Diary.Repositories.Enums;
using Diary.Repositories.Models;
using Diary.Settings;
using ModernMessageBoxLib;

namespace Diary
{
    [Export(typeof(ILogin))]
    internal class LoginViewModel : DScreen, ILogin
    {
        private Account _account;
        private string _selectedAccountType;
        private bool _isSignIn;
        private bool _isLoad;


        public string SelectedAccountType
        {
            get => _selectedAccountType;
            set
            {
                _selectedAccountType = value;
                NotifyOfPropertyChange(nameof(SelectedAccountType));
            }
        }
        public SecureString SecurePassword { private get; set; }

        public bool IsLoad
        {
            get => _isLoad;
            set
            {
                _isLoad = value;
                NotifyOfPropertyChange(()=>IsLoad);
            }
        }

        public Account Account
        {
            get => _account;
            set
            {
                _account = value;
                NotifyOfPropertyChange(nameof(Account));
            }
        }

        public IEnumerable<string> AccountTypes => EnumHelper.EnumAsString(typeof(TypeAccount));


        public Func<object, bool> LoginFinished { get; set; }

        protected override void OnActivate()
        {
            var settings = Common.SettingsHelper.Settings.Load();

            SettingsHelper.InitSettings(settings);

            Account = new Account();

            SelectedAccountType = TypeAccount.Student.ToString();

            IsSignIn = true;

            base.OnActivate();
        }

        public void OnClose(object res)
        {
            var loadingScreen = IoC.Get<ILoadingScreen>();
            var screen = (LoadingScreenViewModel)loadingScreen;
            screen.TryClose();
        }

        public bool IsSignIn
        {
            get => _isSignIn;
            set
            {
                _isSignIn = value;
                NotifyOfPropertyChange(()=>IsSignIn);
            }
        }

        public async void SignIn()
        {
            IsSignIn = false;
            IsLoad = true;

            Account.Password = new System.Net.NetworkCredential(string.Empty, SecurePassword).Password;

            var logged = await SimpleService.PostObject<AccountWithToken>(Controller.Accounts, Method.Authenticate, Account);

            if (logged is AccountWithToken accountWithToken && accountWithToken.Account != null)
            {
                foreach (var type in (TypeAccount[])Enum.GetValues(typeof(TypeAccount)))
                {
                    if (accountWithToken.Account.AccountType.Type == type.ToString())
                    {
                        if (!string.IsNullOrEmpty(accountWithToken.Token))
                            SimpleService.Token = accountWithToken.Token;

                        SimpleService._client.DefaultRequestHeaders.Remove("Authorization");
                        SimpleService._client.DefaultRequestHeaders.Add("Authorization","Bearer " + SimpleService.Token);

                        IsSignIn = true;
                        IsLoad = false;
                        LoginFinished.Invoke(accountWithToken.Account);
                        break;
                    }
                }
            }
            else
            {
                IsSignIn = true;
                IsLoad = false;
            }
        }
    }
}