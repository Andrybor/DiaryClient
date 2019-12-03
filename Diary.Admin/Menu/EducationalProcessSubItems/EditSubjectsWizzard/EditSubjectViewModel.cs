using System.Collections.ObjectModel;
using System.Linq;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;

namespace Diary.Admin.Menu.EducationalProcessSubItems.EditSubjectsWizzard
{
    [DWizzard("EditSubject", "EditSubject", "EditSubjectWizzard")]
    public class EditSubjectViewModel : WizzardScreen
    {
        private ObservableCollection<Course> _courses;
        private Course _selectedCourse;
        private Subject _subject;

        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                NotifyOfPropertyChange(() => SelectedCourse);
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

            Subject = Entity as Subject;

            SelectedCourse = Courses.FirstOrDefault(i => i.Id == Subject.CourseId);
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            Init();
        }

        public async void Save()
        {
            var notTracked = new Subject
            {
                Id = Subject.Id,
                CourseId = SelectedCourse.Id,
                Title = Subject.Title
            };

            if (!string.IsNullOrEmpty(Subject.Title))
                await SimpleService.Post(Controller.Subject, Method.Edit, notTracked);

            dynamic parent = this.Parent;

            parent.TryClose(true);
        }
    }
}