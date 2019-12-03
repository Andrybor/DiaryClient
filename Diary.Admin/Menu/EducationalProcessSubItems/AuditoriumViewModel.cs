using System.Collections.ObjectModel;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;

namespace Diary.Admin.Menu.EducationalProcessSubItems
{
    [Plugin("Auditoriums", DPlugin.Auditorium, DPlugin.EducationalProcess, 7, PackIconModernKind.HomePeople)]
    public class AuditoriumViewModel : DScreen
    {
        private Auditorium _auditorium;
        private ObservableCollection<Auditorium> _auditoriums;

        public ObservableCollection<Auditorium> Auditoriums
        {
            get => _auditoriums;
            set
            {
                _auditoriums = value;
                NotifyOfPropertyChange(() => Auditoriums);
            }
        }

        public Auditorium Auditorium
        {
            get => _auditorium;
            set
            {
                _auditorium = value;
                NotifyOfPropertyChange(() => Auditorium);
            }
        }

        protected override void OnActivate()
        {
            Init();

            base.OnActivate();
        }

        private async void Init()
        {
            var auditoriums =
                await SimpleService.Get<ObservableCollection<Auditorium>>(Controller.Auditorium, Method.GetAll);

            Auditoriums = new ObservableCollection<Auditorium>(auditoriums);

            Auditorium = new Auditorium();
        }

        public async void Create()
        {
            if (!string.IsNullOrEmpty(Auditorium.Title))
                await SimpleService.Post(Controller.Auditorium, Method.Create, Auditorium);

            Init();
        }

        public async void Edit(object item)
        {
            if (item is Auditorium auditorium && !string.IsNullOrEmpty(auditorium.Title))
                await SimpleService.Post(Controller.Auditorium, Method.Edit, auditorium);
        }

        public async void Delete(object item)
        {
            if (item is Auditorium auditorium)
                await SimpleService.Post(Controller.Auditorium, Method.Delete, auditorium.Id);

            Init();
        }
    }
}