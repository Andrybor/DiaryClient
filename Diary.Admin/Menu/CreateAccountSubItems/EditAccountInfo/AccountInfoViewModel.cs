using System;
using System.Windows.Media;
using Diary.Framework.Help;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;

namespace Diary.Admin.Menu.CreateAccountSubItems.EditAccountInfo
{
    [DWizzard("EditAccountInfo", "EditAccountInfo", "EditAccountWizzard")]
    public class AccountInfoViewModel : WizzardScreen
    {
        private Account _account;
        private ImageSource _photo;
        private string _sex;

        public string Sex
        {
            get => _sex;
            set
            {
                _sex = value;
                NotifyOfPropertyChange(() => Sex);
            }
        }

        public Account Account
        {
            get => _account;
            set
            {
                _account = value;
                NotifyOfPropertyChange(() => Account);
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

        public void ChoosePhoto()
        {
            Photo = ImageHelper.OpenImageDialogRetImage();
        }

        protected override void OnActivate()
        {
            Account = Entity as Account;

            if (Account?.User.Birthday == DateTime.MinValue)
                Account.User.Birthday = DateTime.Now;
            if (Account?.User.Image != null && Account?.User.Image.Length != 0)
                Photo = ImageHelper.ByteToImage(Account.User.Image);

            Sex = HelperConvert.BoolToSexStringConvert(Account?.User.Sex);

            base.OnActivate();
        }

        public async void Save()
        {
            var photo = ImageHelper.ImageSourceToBytes(Photo);
            if (photo != null && photo.Length != 0)
                Account.User.Image = photo;

            await SimpleService.Post(Controller.Accounts, Method.Edit, Account);

            dynamic parent = this.Parent;

            parent.TryClose(true);
        }
    }
}