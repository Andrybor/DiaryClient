using System.Collections.Generic;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;

namespace Diary.Student.Menu.MainStudentSubItems
{
    [Plugin("ContactInfo", DPlugin.ContactInfo, DPlugin.MainStudent, 6, PackIconKind.InformationCircle)]
    public class ContactInfoViewModel : DScreen
    {
        private List<ContactInfo> _contacts;

        public List<ContactInfo> Contacts
        {
            get => _contacts;
            set
            {
                _contacts = value;
                NotifyOfPropertyChange(()=> Contacts);
            }
        }

        private async void InitContacts()
        {
            var contacts = await SimpleService.Get<List<ContactInfo>>(Controller.ContactInfo, Method.GetAll);
            if (contacts != null)
            {
                Contacts = contacts;
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            InitContacts();
        }
    }
}