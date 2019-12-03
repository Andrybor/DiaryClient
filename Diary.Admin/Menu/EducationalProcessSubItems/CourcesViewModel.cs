using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Framework.Wizard;
using Diary.Repositories;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;

namespace Diary.Admin.Menu.EducationalProcessSubItems
{
    [Plugin("Courses", DPlugin.Course, DPlugin.EducationalProcess, 2, PackIconModernKind.BookHardcoverOpenWriting)]
    public class CourcesViewModel : DScreen
    {
        private ObservableCollection<Course> _cources;
        private Course _course;
        private Specialization _selectedSpecialization;

        private ObservableCollection<Specialization> _specializations;
        [Import(typeof(WizardManager))] private WizardManager wizardManager;

        public ObservableCollection<Course> Cources
        {
            get => _cources;
            set
            {
                _cources = value;
                NotifyOfPropertyChange(() => Cources);
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

        public Specialization SelectedSpecialization
        {
            get => _selectedSpecialization;
            set
            {
                _selectedSpecialization = value;
                NotifyOfPropertyChange(() => SelectedSpecialization);
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

            SelectedSpecialization = Specializations.FirstOrDefault();

            var cources = await SimpleService.Get<ObservableCollection<Course>>(Controller.Course, Method.GetAll);
            if (cources != null)
                Cources = cources;

            Course = new Course();
        }

        public async void Create()
        {
            Course.SpecializationId = SelectedSpecialization.Id;

            if (!string.IsNullOrEmpty(Course.Title))
            {
                await SimpleService.Post(Controller.Course, Method.Create, Course);

                Init();
            }
        }

        public void Edit(object item)
        {
            wizardManager.Show("EditCourcesWizzard", item, "EditCourse");

            Init();
        }

        public async void Delete(object item)
        {
            if (item is Course course)
            {
                await SimpleService.Post(Controller.Course, Method.Delete, course.Id);

                Init();
            }
        }
    }
}