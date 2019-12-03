using System.Collections.ObjectModel;
using System.Linq;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;

namespace Diary.Admin.Menu.EducationalProcessSubItems.EditCourcesWizzard
{
    [DWizzard("EditCources", "EditCources", "EditCourcesWizzard")]
    public class EditCourcesViewModel : WizzardScreen
    {
        private Course _course;
        private Specialization _selectedSpecialization;
        private ObservableCollection<Specialization> _specializations;

        public Specialization SelectedSpecialization
        {
            get => _selectedSpecialization;
            set
            {
                _selectedSpecialization = value;
                NotifyOfPropertyChange(() => SelectedSpecialization);
            }
        }

        public Course Course
        {
            get => _course;
            set
            {
                _course = value;
                NotifyOfPropertyChange(() => Course);
            }
        }

        public ObservableCollection<Specialization> Specializations
        {
            get => _specializations;
            set
            {
                _specializations = value;
                NotifyOfPropertyChange(() => Specializations);
            }
        }

        protected override void OnActivate()
        {
            Init();

            base.OnActivate();
        }

        private async void Init()
        {
            var specializations =
                await SimpleService.Get<ObservableCollection<Specialization>>(Controller.Specialization, Method.GetAll);
            if (specializations != null)
                Specializations = specializations;

            Course = Entity as Course;

            SelectedSpecialization = Specializations.FirstOrDefault(i => i.Id == Course.SpecializationId);
        }

        public async void Save()
        {
            Course.SpecializationId = SelectedSpecialization.Id;

            var notTracked = new Course
            {
                Id = Course.Id,
                SpecializationId = Course.SpecializationId,
                Title = Course.Title,
                AmountOfHours = Course.AmountOfHours
            };
            if (!string.IsNullOrEmpty(notTracked.Title))
                await SimpleService.Post(Controller.Course, Method.Edit, notTracked);

            dynamic parent = this.Parent;

            parent.TryClose(true);
        }
    }
}