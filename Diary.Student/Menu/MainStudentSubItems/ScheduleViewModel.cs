using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;

namespace Diary.Student.Menu.MainStudentSubItems
{
    [Plugin("Schedule", DPlugin.Schedule, DPlugin.MainStudent, 3, PackIconKind.Schedule)]
    public class ScheduleViewModel : DScreen
    {
        private DateTime _selectedDate;
        private List<Schedule> _schedule;
        private ObservableCollection<DateTime> _selectedDates;

        public ScheduleViewModel()
        {
            InitSchedule();
        }

        public List<Schedule> AllSchedules { get; set; }

        public ObservableCollection<DateTime> SelectedDates
        {
            get => _selectedDates;
            set
            {
                _selectedDates = value;
                NotifyOfPropertyChange(()=>SelectedDates);
            }
        }
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                InitScheduleByCalendar(_selectedDate);
                NotifyOfPropertyChange(()=> SelectedDate);
            }
        }
        public List<Schedule> Schedule
        {
            get => _schedule;
            set
            {
                _schedule = value;
                NotifyOfPropertyChange(()=> Schedule);
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
            if (dates.Count > 0 && AllSchedules!=null)
            {
                foreach (var schedule in AllSchedules)
                {
                    dates.Add(schedule.StartTime.Value.Date);
                }
            }
        }
    }
}