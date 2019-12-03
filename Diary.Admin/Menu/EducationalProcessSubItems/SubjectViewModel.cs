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
    [Plugin("Subjects", DPlugin.Subject, DPlugin.EducationalProcess, 3, PackIconModernKind.PuzzleRound)]
    public class SubjectViewModel : DScreen
    {
        private ObservableCollection<Course> _courses;
        private Course _selectedCourse;

        private Subject _subject;
        private ObservableCollection<Subject> _subjects;
        [Import(typeof(WizardManager))] private WizardManager wizardManager;

        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                NotifyOfPropertyChange(() => SelectedCourse);
            }
        }

        public ObservableCollection<Subject> Subjects
        {
            get => _subjects;
            set
            {
                _subjects = value;
                NotifyOfPropertyChange(() => Subjects);
            }
        }

        public ObservableCollection<Course> Courses
        {
            get => _courses;
            set
            {
                _courses = value;
                NotifyOfPropertyChange(() => Courses);
            }
        }

        public Subject Subject
        {
            get => _subject;
            set
            {
                _subject = value;
                NotifyOfPropertyChange(() => Subject);
            }
        }

        private async void Init()
        {
            var courses = await SimpleService.Get<ObservableCollection<Course>>(Controller.Course, Method.GetAll);
            if (courses != null)
                Courses = courses;

            var subjects = await SimpleService.Get<ObservableCollection<Subject>>(Controller.Subject, Method.GetAll);
            if (subjects != null)
                Subjects = subjects;

            Subject = new Subject();

            SelectedCourse = courses?.FirstOrDefault();
        }

        protected override void OnActivate()
        {
            Init();

            base.OnActivate();
        }

        public async void Create()
        {
            Subject.CourseId = SelectedCourse.Id;

            if (!string.IsNullOrEmpty(Subject.Title))
            {
                await SimpleService.Post(Controller.Subject, Method.Create, Subject);

                Init();
            }
        }

        public async void Delete(object item)
        {
            if (item is Subject subject)
            {
                await SimpleService.Post(Controller.Subject, Method.Delete, subject.Id);

                Init();
            }
        }

        public void Edit(object item)
        {
            wizardManager.Show("EditSubjectWizzard", item, "EditSubject");

            Init();
        }
    }
}