using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Diary.Common.Helpers;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Diary.Teacher.Menu.MainTeacherSubItems
{
    [Plugin("Schedule", DPlugin.Schedule, DPlugin.MainTeacher, 2, PackIconKind.Schedule)]
    public class ScheduleViewModel : DScreen
    {
        private DateTime _selectedDate;
        private List<Schedule> _schedule;
        private ObservableCollection<DateTime> _selectedDates;
        private ObservableCollection<LessonInfo> _lessonInfos;

        public ScheduleViewModel()
        {
            InitSchedule();
        }

        public List<Schedule> AllSchedules { get; set; }
        public Schedule CurrentSchedule { get; set; }

        public ObservableCollection<LessonInfo> LessonInfos
        {
            get => _lessonInfos;
            set
            {
                _lessonInfos = value;
                NotifyOfPropertyChange(()=> LessonInfos);
            }
        }

        public ObservableCollection<DateTime> SelectedDates
        {
            get => _selectedDates;
            set
            {
                _selectedDates = value;
                NotifyOfPropertyChange(() => SelectedDates);
            }
        }
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                InitScheduleByCalendar(_selectedDate);
                NotifyOfPropertyChange(() => SelectedDate);
            }
        }
        public List<Schedule> Schedule
        {
            get => _schedule;
            set
            {
                _schedule = value;
                NotifyOfPropertyChange(() => Schedule);
            }
        }

        private async void InitSchedule()
        {
            if (SimpleService.LoggedUser != null)
            {
                var schedule =
                    await SimpleService.Get<List<Schedule>>(Controller.Schedule, Method.Get,
                        SimpleService.LoggedUser.Id);

                if (schedule != null)
                {
                    AllSchedules = schedule;
                }
            }
        }

        private void InitScheduleByCalendar(DateTime dateTime)
        {
            if (AllSchedules != null)
            {
                Schedule = AllSchedules.Where(i => i.StartTime.Value.Date == dateTime.Date).ToList();
            }
        }

        protected override void OnActivate()
        {
            InitSchedule();

            InitScheduleByCalendar(SelectedDate);

            base.OnActivate();
        }

        public void AssignDates(SelectedDatesCollection dates)
        {
            if (dates.Count > 0 && AllSchedules != null)
            {
                foreach (var schedule in AllSchedules)
                {
                    dates.Add(schedule.StartTime.Value.Date);
                }
            }
        }

        public async void Mark()
        {
            if (LessonInfos != null)
            {
                List<LessonInfo> infos = new List<LessonInfo>();
                foreach (var itemLessonInfo in LessonInfos)
                {
                    infos.Add(new LessonInfo
                    {
                        IsPresent = itemLessonInfo.IsPresent,
                        ScheduleId = CurrentSchedule.Id,
                        UserId = itemLessonInfo.UserId,
                        Grade = itemLessonInfo.Grade
                    });
                }

                if (infos.Count != 0)
                {
                    await SimpleService.Post(Controller.LessonInfo, Method.Create, infos);

                    LessonInfos = new ObservableCollection<LessonInfo>();
                }
            }
        }

        public async void HomeWork()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Text files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                string filename = openFileDialog1.FileName;

                byte[] file = FileHelper.FileToByteArray(filename);

                if (file != null
                    && file.Length != 0
                    && CurrentSchedule!=null)
                {
                    Homework homework = new Homework
                    {
                        ScheduleId = CurrentSchedule.Id,
                        Task = file
                    };

                    await SimpleService.Post(Controller.Homework, Method.Create, homework);
                }
            }
        }

        public async void OpenLessonInfo(object item)
        {
            if (item is Schedule schedule)
            {
                CurrentSchedule = schedule;

                var group = await SimpleService.Get<Group>(Controller.Group, Method.Get, schedule.GroupId);

                if (group != null)
                {
                    LessonInfos = new ObservableCollection<LessonInfo>();

                    foreach (var student in group.Students)
                    {
                        LessonInfos.Add(new LessonInfo
                        {
                            User = student.User,
                            UserId = student.UserId,
                            ScheduleId = schedule.Id,
                            Schedule = schedule
                        });
                    }

                }
            }
        }
    }
}