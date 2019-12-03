using System.Collections.ObjectModel;
using System.Linq;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;

namespace Diary.Admin.Menu.EducationalProcessSubItems.EditGroupWizzard
{
    [DWizzard("EditGroup", "EditGroup", "EditGroupWizzard")]
    public class EditGroupViewModel : WizzardScreen
    {
        private ObservableCollection<Course> _courses;
        private Group _group;
        private Course _selectedCourse;

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

        public Group Group
        {
            get => _group;
            set
            {
                _group = value;
                NotifyOfPropertyChange(() => Group);
            }
        }

        private async void Init()
        {
            var courses = await SimpleService.Get<ObservableCollection<Course>>(Controller.Course, Method.GetAll);
            if (courses != null)
                Courses = courses;

            Group = Entity as Group;

            SelectedCourse = Courses.FirstOrDefault(i => i.Id == Group.CourseId);
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            Init();
        }

        public async void Save()
        {
            var notTracked = new Group
            {
                Id = Group.Id,
                CourseId = SelectedCourse.Id,
                Title = Group.Title
            };

            if (!string.IsNullOrEmpty(Group.Title))
                await SimpleService.Post(Controller.Group, Method.Edit, notTracked);

            dynamic parent = this.Parent;

            parent.TryClose(true);
        }
    }
}