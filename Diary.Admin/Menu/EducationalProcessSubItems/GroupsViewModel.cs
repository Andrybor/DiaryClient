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
    [Plugin("Groups", DPlugin.Group, DPlugin.EducationalProcess, 4, PackIconModernKind.Group)]
    public class GroupsViewModel : DScreen
    {
        private ObservableCollection<Course> _courses;

        private Group _group;
        private ObservableCollection<Group> _groups;
        private Course _selectedCourse;
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

        public ObservableCollection<Course> Courses
        {
            get => _courses;
            set
            {
                _courses = value;
                NotifyOfPropertyChange(() => Courses);
            }
        }

        public ObservableCollection<Group> Groups
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

        private async void Init()
        {
            var courses = await SimpleService.Get<ObservableCollection<Course>>(Controller.Course, Method.GetAll);
            if (courses != null)
                Courses = courses;

            var groups = await SimpleService.Get<ObservableCollection<Group>>(Controller.Group, Method.GetAll);
            if (groups != null)
                Groups = groups;

            Group = new Group();

            SelectedCourse = Courses.FirstOrDefault();
        }

        protected override void OnActivate()
        {
            Init();

            base.OnActivate();
        }

        public async void Create()
        {
            Group.CourseId = SelectedCourse.Id;
            Group.SpecializationId = SelectedCourse.SpecializationId;

            if (!string.IsNullOrEmpty(Group.Title))
            {
                await SimpleService.Post(Controller.Group, Method.Create, Group);

                Init();
            }
        }

        public void Edit(object item)
        {
            wizardManager.Show("EditGroupWizzard", item, "EditGroup");

            Init();
        }

        public async void Delete(object item)
        {
            if (item is Group group)
            {
                await SimpleService.Post(Controller.Group, Method.Delete, group.Id);

                Init();
            }
        }
    }
}