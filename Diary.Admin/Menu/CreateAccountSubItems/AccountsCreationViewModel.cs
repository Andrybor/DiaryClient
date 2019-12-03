using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.RightsManagement;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Diary.Common;
using Diary.Framework;
using Diary.Framework.Help;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Enums;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;
using ModernMessageBoxLib;

namespace Diary.Admin.Menu.CreateAccountSubItems
{
    [Plugin("AccountsCreation", DPlugin.AccountsCreation, DPlugin.Create, 2, PackIconModernKind.UserAdd)]
    public class AccountsCreationViewModel : DScreen
    {
        private Account _account;
        private ImageSource _photo;
        private string _selectedAccountType;
        private string _sex;
        private bool _isStudentGroup;
        private Group _group;
        private List<Group> _groups;
        private Student _student;
        private User _user;
        private bool _isCreated;

        public bool IsCreated
        {
            get => _isCreated;
            set
            {
                _isCreated = value;
                NotifyOfPropertyChange(()=>IsCreated);
            }
        }
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                NotifyOfPropertyChange(()=> User);
            }
        }
        public Student Student
        {
            get => _student;
            set
            {
                _student = value;
                NotifyOfPropertyChange(() => Student);
            }
        }

        public List<Group> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                NotifyOfPropertyChange(() => Groups);
            }
        }

        public Group Group
        {
            get => _group;
            set
            {
                _group = value;
                NotifyOfPropertyChange(() => Group);
            }
        }

        public bool IsStudentGroup
        {
            get => _isStudentGroup;
            set
            {
                _isStudentGroup = value;
                NotifyOfPropertyChange(() => IsStudentGroup);
            }
        }

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
                NotifyOfPropertyChange(nameof(Account));
            }
        }

        public string SelectedAccountType
        {
            get => _selectedAccountType;
            set
            {
                _selectedAccountType = value;
                NotifyOfPropertyChange(nameof(SelectedAccountType));
            }
        }

        public IEnumerable<string> AccountTypes => EnumHelper.EnumAsString(typeof(TypeAccount));

        public ImageSource Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                NotifyOfPropertyChange(nameof(Photo));
            }
        }

        protected override void OnActivate()
        {
            Init();

            SelectedAccountType = TypeAccount.Student.ToString().Translate();

            // Set default 
            base.OnActivate();
        }


        private void Init()
        {
            Account = new Account();

            User = new User();

            User.Birthday = DateTime.Now;

            Sex = Repositories.Enums.Sex.Male.ToString();

            Photo = new BitmapImage();

            NotifyOfPropertyChange(()=>User);

            if (SelectedAccountType == TypeAccount.Student.ToString().Translate())
            {
                InitGroups();

                IsStudentGroup = true;
            }
        }

        private async void InitGroups()
        {
            var groups = await SimpleService.Get<List<Group>>(Controller.Group, Method.GetAll);
            if (groups != null)
            {
                Groups = groups;

                Group = Groups.FirstOrDefault();

                IsStudentGroup = true;
            }
        }

        public void SelectionChanged()
        {
            if (SelectedAccountType == TypeAccount.Student.ToString().Translate())
                IsStudentGroup = true;
            else
            {
                IsStudentGroup = false;
            }
        }

        public void ChoosePhoto()
        {
            Photo = ImageHelper.OpenImageDialogRetImage();
        }


        public async void Create()
        {
            NotifyOfPropertyChange(()=> Account);

            Account.User = User;

            var photo = ImageHelper.ImageSourceToBytes(Photo);

            Account.User.Image = photo;

            Enum.TryParse(SelectedAccountType.GetKey(),out TypeAccount typeAccount);

            Account.User.CreatingDay = DateTime.Now;

            Account.AccountTypeId = (int)typeAccount;

            Account.User.Sex = HelperConvert.StringSexToBoolConvert(Sex);

            var userId = await SimpleService.PostObject<int>(Controller.Accounts, Method.Create, Account);

            // create student and assign group

            if (SelectedAccountType == TypeAccount.Student.ToString().Translate()
                && userId!=null)
            {
                Student = new Student
                {
                    GroupId = Group.Id,
                    UserId = (int)userId
                };

                await SimpleService.Post(Controller.Student, Method.Create, Student);
            }

            Init();
        }
    }
}