using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Enums;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;

namespace Diary.Admin.Menu.EducationalProcessSubItems
{
    [Plugin("TeacherInfo", DPlugin.TeacherInfo, DPlugin.EducationalProcess, 5, PackIconModernKind.Brick)]
    public class TeacherInfoViewModel : DScreen
    {
        private ObservableCollection<Course> _courses;
        private Course _selectedCourse;
        private Specialization _selectedSpecialization;

        private User _selectedTeacher;
        private ObservableCollection<Specialization> _specializations;
        private ObservableCollection<AssignSubjects> _subjects;
        private ObservableCollection<User> _teacher;

        public User SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                NotifyOfPropertyChange(() => SelectedTeacher);
            }
        }

        public ObservableCollection<User> Teachers
        {
            get => _teacher;
            set
            {
                _teacher = value;
                NotifyOfPropertyChange(() => Teachers);
            }
        }

        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
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

        public ObservableCollection<AssignSubjects> Subjects
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

        private async void InitSubjects(Course course)
        {
            if (course != null)
            {
                var subjects =
                    await SimpleService.Get<ObservableCollection<Subject>>(Controller.Subject, Method.GetAll);
                if (subjects != null)
                {
                    Subjects = new ObservableCollection<AssignSubjects>();
                    foreach (var subject in subjects)
                        Subjects.Add(new AssignSubjects
                        {
                            Subject = subject
                        });
                }
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

        private async void InitTeachers()
        {
            var accounts = await SimpleService.Get<ObservableCollection<Account>>(Controller.Accounts, Method.GetAll);
            if (accounts != null)
            {
                Teachers = new ObservableCollection<User>(accounts
                    .Where(i => i.AccountType.Type == TypeAccount.Teacher.ToString()).Select(i => i.User));
                SelectedTeacher = Teachers.FirstOrDefault();
            }
        }

        public async void Assign()
        {
            if (Subjects != null)
            {
                var assignedSubjects = Subjects.Where(x => x.isAssign);

                var teacherSkills = new List<Teacher>();
                foreach (var assignedSubject in assignedSubjects)
                    teacherSkills.Add(new Teacher
                    {
                        SubjectId = assignedSubject.Subject.Id,
                        TeacherId = SelectedTeacher.Id
                    });

                await SimpleService.Post(Controller.Teacher, Method.Create, teacherSkills);

                InitSpecializations();

                InitTeachers();
            }
        }

        protected override void OnActivate()
        {
            InitSpecializations();

            InitTeachers();

            base.OnActivate();
        }
    }
}