using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;

namespace Diary.Admin.Menu.EducationalProcessSubItems
{
    [Plugin("Schedule", DPlugin.Schedule, DPlugin.EducationalProcess, 6, PackIconModernKind.Calendar)]
    public class ScheduleViewModel : DScreen
    {
        private ObservableCollection<Auditorium> _auditoriums;
        private ObservableCollection<Course> _courses;
        private ObservableCollection<Group> _groups;

        private Schedule _schedule;
        private ObservableCollection<Schedule> _schedules;
        private Auditorium _selectedAuditorium;
        private Course _selectedCourse;
        private Group _selectedGroup;
        private Specialization _selectedSpecialization;
        private Subject _selectedSubject;
        private User _selectedTeacher;
        private ObservableCollection<Specialization> _specializations;
        private ObservableCollection<Subject> _subjects;
        private ObservableCollection<User> _teachers;

        public Schedule Schedule
        {
            get => _schedule;
            set
            {
                _schedule = value;
                NotifyOfPropertyChange(() => Schedule);
            }
        }

        public Auditorium SelectedAuditorium
        {
            get => _selectedAuditorium;
            set
            {
                _selectedAuditorium = value;
                NotifyOfPropertyChange(() => SelectedAuditorium);
            }
        }

        public User SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                NotifyOfPropertyChange(() => SelectedTeacher);
            }
        }

        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                InitSchedules(_selectedGroup);
                NotifyOfPropertyChange(() => SelectedGroup);
            }
        }

        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                InitTeachers(_selectedSubject);
                NotifyOfPropertyChange(() => SelectedSubject);
            }
        }

        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                InitGroups(_selectedCourse);
                InitSubjects(_selectedCourse);
                NotifyOfPropertyChange(() => SelectedCourse);
            }
        }

        public Specialization SelectedSpecialization
        {
            get => _selectedSpecialization;
            set
            {
                _selectedSpecialization = value;
                InitCourses(_selectedSpecialization);
                NotifyOfPropertyChange(() => SelectedSpecialization);
            }
        }

        public ObservableCollection<Schedule> Schedules
        {
            get => _schedules;
            set
            {
                _schedules = value;
                NotifyOfPropertyChange(() => Schedules);
            }
        }

        public ObservableCollection<Auditorium> Auditoriums
        {
            get => _auditoriums;
            set
            {
                _auditoriums = value;
                NotifyOfPropertyChange(() => Auditoriums);
            }
        }

        public ObservableCollection<User> Teachers
        {
            get => _teachers;
            set
            {
                _teachers = value;
                NotifyOfPropertyChange(() => Teachers);
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

        public ObservableCollection<Specialization> Specializations
        {
            get => _specializations;
            set
            {
                _specializations = value;
                NotifyOfPropertyChange(() => Specializations);
            }
        }

        private async void InitGroups(Course course)
        {
            if (course != null)
            {
                var groups = await SimpleService.Get<ObservableCollection<Group>>(Controller.Group, Method.GetAll);
                if (groups != null)
                {
                    Groups = new ObservableCollection<Group>(groups.Where(x => x.CourseId == course.Id));
                    SelectedGroup = Groups.FirstOrDefault();
                }
            }
            else
            {
                if (Groups != null)
                    Groups.Clear();
                SelectedGroup = null;
            }
        }

        private async void InitSubjects(Course course)
        {
            if (course != null)
            {
                var subjects =
                    await SimpleService.Get<ObservableCollection<Subject>>(Controller.Subject, Method.GetAll);
                if (subjects != null)
                {
                    Subjects = new ObservableCollection<Subject>(subjects.Where(x => x.CourseId == course.Id));
                    SelectedSubject = Subjects.FirstOrDefault();
                }
            }
            else
            {
                if (Subjects != null)
                    Subjects.Clear();
                SelectedSubject = null;
                SelectedTeacher = null;
                Teachers = null;
            }
        }

        private async void InitTeachers(Subject subject)
        {
            if (subject != null)
            {
                var skillsAll = await SimpleService.Get<List<Teacher>>(Controller.Teacher, Method.GetAll);

                if (skillsAll != null)
                {
                    Teachers = new ObservableCollection<User>(skillsAll.Where(x => x.SubjectId == subject.Id)
                        .Select(x => x.User));

                    SelectedTeacher = Teachers.FirstOrDefault();
                }
            }
        }

        private async void InitSpecializations()
        {
            var specializations =
                await SimpleService.Get<ObservableCollection<Specialization>>(Controller.Specialization, Method.GetAll);
            if (specializations != null)
            {
                Specializations = specializations;
                SelectedSpecialization = Specializations.FirstOrDefault();
            }
        }

        private async void InitCourses(Specialization specialization)
        {
            if (specialization != null)
            {
                var courses = await SimpleService.Get<ObservableCollection<Course>>(Controller.Course, Method.GetAll);

                if (courses != null)
                {
                    Courses = new ObservableCollection<Course>(courses.Where(x =>
                        x.SpecializationId == specialization.Id));
                    SelectedCourse = Courses.FirstOrDefault();
                }
            }
        }

        private async void InitAuditoriums()
        {
            var auditoriums =
                await SimpleService.Get<ObservableCollection<Auditorium>>(Controller.Auditorium, Method.GetAll);
            if (auditoriums != null)
            {
                Auditoriums = auditoriums;

                SelectedAuditorium = Auditoriums.FirstOrDefault();
            }
        }

        private async void InitSchedules(Group selectedGroup)
        {
            var schedules = await SimpleService.Get<ObservableCollection<Schedule>>(Controller.Schedule, Method.GetAll);
            if (schedules != null && selectedGroup != null)
                Schedules = new ObservableCollection<Schedule>(schedules.Where(i => i.GroupId == selectedGroup.Id));
        }

        public async void Create()
        {
            Schedule.AuditoriumId = SelectedAuditorium.Id;
            Schedule.SubjectId = SelectedSubject.Id;
            Schedule.GroupId = SelectedGroup.Id;
            Schedule.TeacherId = SelectedTeacher.Id;

            await SimpleService.Post(Controller.Schedule, Method.Create, Schedule);

            Init();
        }

        public async void Delete(object item)
        {
            if (item is Schedule schedule)
            {
                await SimpleService.Post(Controller.Schedule, Method.Delete, schedule.Id);

                Init();
            }
        }

        private void Init()
        {
            Schedule = new Schedule();

            InitAuditoriums();

            InitSpecializations();
        }

        protected override void OnActivate()
        {
            Init();

            base.OnActivate();
        }
    }
}