using System.Collections.ObjectModel;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;

namespace Diary.Admin.Menu.EducationalProcessSubItems
{
    [Plugin("Specializations", DPlugin.Specialization, DPlugin.EducationalProcess, 1,
        PackIconModernKind.AppFavorite)]
    public class SpecializationViewModel : DScreen
    {
        private Specialization _specialization;
        private ObservableCollection<Specialization> _specializations;

        public ObservableCollection<Specialization> Specializations
        {
            get => _specializations;
            set
            {
                _specializations = value;
                NotifyOfPropertyChange(() => Specializations);
            }
        }

        public Specialization Specialization
        {
            get => _specialization;
            set
            {
                _specialization = value;
                NotifyOfPropertyChange(() => Specialization);
            }
        }

        protected override void OnActivate()
        {
            InitSpecializations();

            base.OnActivate();
        }

        private async void InitSpecializations()
        {
            var specializations =
                await SimpleService.Get<ObservableCollection<Specialization>>(Controller.Specialization, Method.GetAll);

            Specializations = new ObservableCollection<Specialization>(specializations);

            Specialization = new Specialization();
        }

        public async void Create()
        {
            if (!string.IsNullOrEmpty(Specialization.Title))
                await SimpleService.Post(Controller.Specialization, Method.Create, Specialization);

            InitSpecializations();
        }

        public async void Edit(object item)
        {
            if (item is Specialization specialization && !string.IsNullOrEmpty(specialization.Title))
                await SimpleService.Post(Controller.Specialization, Method.Edit, specialization);
        }

        public async void Delete(object item)
        {
            if (item is Specialization specialization)
                await SimpleService.Post(Controller.Specialization, Method.Delete, specialization.Id);

            InitSpecializations();
        }
    }
}