using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Helpers;
using Diary.Repositories.Models;
using MaterialDesignThemes.Wpf;

namespace Diary.Teacher.Menu.MainTeacherSubItems
{
    [Plugin("Statistics", DPlugin.Statistics, DPlugin.MainTeacher, 4, PackIconKind.ChartTimelineVariant)]
    public class StatisticsViewModel : DScreen
    {
        private Group _selectedGroup;
        private ObservableCollection<Group> _groups;
        private ObservableCollection<LessonInfo> _lessonInfos;
        private ObservableCollection<StatisticsData> _data;
        private ObservableCollection<Statistics> _studentsStatistics;

        public ObservableCollection<LessonInfo> AllTeacherLessonInfos { get; set; }

        public ObservableCollection<Statistics> StudentStatistics
        {
            get => _studentsStatistics;
            set
            {
                _studentsStatistics = value;
                NotifyOfPropertyChange(() => StudentStatistics);
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
        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                CalculateDataForGroup(_selectedGroup);
                NotifyOfPropertyChange(() => SelectedGroup);
            }
        }
        public ObservableCollection<LessonInfo> LessonInfos
        {
            get => _lessonInfos;
            set
            {
                _lessonInfos = value;
                NotifyOfPropertyChange(() => LessonInfos);
            }
        }

        private async void InitInfos(Group group = null)
        {
            var infos = await SimpleService.Get<ObservableCollection<LessonInfo>>(Controller.LessonInfo, Method.GetForTeacher,
                SimpleService.LoggedUser.Id);
            if (infos != null)
            {
                AllTeacherLessonInfos = infos;
                var groups = AllTeacherLessonInfos.Select(i => i.Schedule.Group).DistinctBy(i => i.Id).ToList();
                if (groups != null)
                {
                    Groups = new ObservableCollection<Group>(groups);

                    SelectedGroup = Groups.FirstOrDefault();                 
                }
            }
        }

        private void CalculateDataForGroup(Group group)
        {
            if (group != null)
            {
                LessonInfos =
                    new ObservableCollection<LessonInfo>(
                        AllTeacherLessonInfos.Where(i => i.Schedule.Group.Id == group.Id));

                var groupStat = CalculateStatistics.InitGroupStatistics(LessonInfos);
                StudentStatistics = new ObservableCollection<Statistics>(groupStat);
                var generalStatistics = CalculateStatistics.CalculateGroupStat(groupStat);

                Data = new ObservableCollection<StatisticsData>
                {
                    new StatisticsData {Name = "Group Present % ", Count = generalStatistics.PercentagePresents},
                    new StatisticsData {Name = "Average Marks % ", Count = generalStatistics.AverageMark},
                };
            }
        }


        protected  override void OnActivate()
        {
            InitInfos();
            base.OnActivate();
        }

        public ObservableCollection<StatisticsData> Data
        {
            get => _data;
            set
            {
                _data = value;
                NotifyOfPropertyChange(() => Data);
            }
        }
    }

    public static class Extensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
