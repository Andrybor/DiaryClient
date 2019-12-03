using System.Collections.ObjectModel;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;

namespace Diary.Admin.Menu.InfoSubItems
{
    [Plugin("ContactInfo",DPlugin.ContactInfo,DPlugin.Info,3,PackIconModernKind.InformationCircle)]
    public class ContactInfoViewModel : DScreen 
    {
        private ContactInfo _contact;
        private ObservableCollection<ContactInfo> _contacts;

        public ObservableCollection<ContactInfo> Contacts
        {
            get => _contacts;
            set
            {
                _contacts = value;
                NotifyOfPropertyChange(()=>Contacts);
            }
        }

        public ContactInfo Contact
        {
            get => _contact;
            set
            {
                _contact = value;
                NotifyOfPropertyChange(()=> Contact);
            }
        }

        private async void Init()
        {
            var contacts =
                await SimpleService.Get<ObservableCollection<ContactInfo>>(Controller.ContactInfo, Method.GetAll);
            if (contacts != null)
            {
                Contacts = contacts;

                Contact = new ContactInfo();
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            Init();   
        }

        public async void Create()
        {
            if (Contact != null)
            {
                await SimpleService.Post(Controller.ContactInfo, Method.Create, Contact);

                Init();
            }
        }

        public async void Delete(object item)
        {
            if (item is ContactInfo contact)
            {
                await SimpleService.Post(Controller.ContactInfo, Method.Delete, contact.Id);
                
                Init();
            }
        }
    }
}
