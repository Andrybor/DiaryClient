using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Helpers;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;

namespace Diary.Student.Menu.MainStudentSubItems
{
    [Plugin("MainInfo", DPlugin.MainInfoStudent, DPlugin.MainStudent, 1, PackIconKind.AccountBadge)]
    public class MainInfoStudentViewModel : DScreen
    {
        private Statistics _statistics;
        private ObservableCollection<StatisticsData> _presentsData;
        private ObservableCollection<StatisticsData> _marksData;
        private ObservableCollection<StatisticsData> _marksGroupData;
        private ObservableCollection<StatisticsData> _presentsGroupData;
        private PositionStatistics _position;
        
        public ObservableCollection<LessonInfo> GroupInfo { get; set; }

        public PositionStatistics Position
        {
            get => _position;
            set
            {
                _position = value;
                NotifyOfPropertyChange(()=> Position);
            }
        }

        public Statistics Statistics
        {
            get => _statistics;
            set
            {
                _statistics = value;
                NotifyOfPropertyChange(()=> Statistics);
            }
        }

        public ObservableCollection<StatisticsData> PresentsData
        {
            get => _presentsData;
            set
            {
                _presentsData = value;
                NotifyOfPropertyChange(() => PresentsData);
            }
        }

        public ObservableCollection<StatisticsData> PresentsGroupData
        {
            get => _presentsGroupData;
            set
            {
                _presentsGroupData = value;
                NotifyOfPropertyChange(() => PresentsGroupData);
            }
        }
        public ObservableCollection<StatisticsData> MarksGroupData
        {
            get => _marksGroupData;
            set
            {
                _marksGroupData = value;
                NotifyOfPropertyChange(() => MarksGroupData);
            }
        }

        public ObservableCollection<StatisticsData> MarksData
        {
            get => _marksData;
            set
            {
                _marksData = value;
                NotifyOfPropertyChange(() => MarksData);
            }
        }

        protected override void OnActivate()
        {
            InitStatistics();

            base.OnActivate();
        }

        private async void InitStatistics()
        {
            var infos = await SimpleService.Get<ObservableCollection<LessonInfo>>(Controller.LessonInfo, Method.Get,
                SimpleService.LoggedUser.Id);

            var groupInfo = await SimpleService.Get<ObservableCollection<LessonInfo>>(Controller.LessonInfo,
                Method.GetLessonInfoByGroup, SimpleService.LoggedUser.Id);
            if (groupInfo != null)
            {
                GroupInfo = groupInfo;

                var groupStat = CalculateStatistics.InitGroupStatistics(GroupInfo);

                var groupStatByAvgMark = CalculateStatistics.InitGroupStatisticsByMark(GroupInfo);

                MarksGroupData = new ObservableCollection<StatisticsData>(groupStatByAvgMark
                    .Select(x => new StatisticsData { Count = x.AverageMark, Name = x.UserFullName }));

                PresentsGroupData = new ObservableCollection<StatisticsData>(groupStat
                    .Select(x => new StatisticsData { Count = x.PercentagePresents, Name = x.UserFullName }));

                Position = CalculateStatistics.GetStudentRaiting(groupStat);
            }

            if (infos != null)
            {
                Statistics = CalculateStatistics.InitStudentStatistics(infos);

                PresentsData = new ObservableCollection<StatisticsData>
                {
                    new StatisticsData {Name = "Present % ", Count = Statistics.PercentagePresents},
                };

                MarksData = new ObservableCollection<StatisticsData>
                {
                    new StatisticsData {Name = "Marks % ", Count = Statistics.AverageMark},
                };
            }
        }

        
    }
}