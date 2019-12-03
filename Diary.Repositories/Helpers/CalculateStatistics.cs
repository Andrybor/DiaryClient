using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Diary.Repositories.Models;

namespace Diary.Repositories.Helpers
{
    public static class CalculateStatistics
    {
        public static Statistics CalculateGroupStat(List<Statistics> statistics)
        {
            Statistics statGroup = new Statistics();

            int percentagePresent = 0;
            int avgMarks = 0;

            foreach (var stat in statistics)
            {
                percentagePresent += stat.PercentagePresents;
                avgMarks += stat.AverageMark;
            }

            statGroup.AverageMark = avgMarks / statistics.Count;
            statGroup.PercentagePresents = percentagePresent / statistics.Count;

            return statGroup;
        }

        public static List<Statistics> InitGroupStatistics(ObservableCollection<LessonInfo> infos)
        {
            List<Statistics> statGroup = new List<Statistics>();

            var userDistinctId = infos.Select(i => i.UserId).Distinct();

            foreach (var id in userDistinctId)
            {
                statGroup.Add(InitStudentStatisticsForTeacher(infos, id));
            }

            return statGroup;
        }

        public static List<Statistics> InitGroupStatisticsByMark(ObservableCollection<LessonInfo> infos)
        {
            List<Statistics> statGroup = new List<Statistics>();

            var userDistinctId = infos.Select(i => i.UserId).Distinct();

            foreach (var id in userDistinctId)
            {
                statGroup.Add(InitStudentStatisticsByMarkWithoutPresent(infos, id));
            }

            return statGroup;
        }

        public static Statistics InitStudentStatisticsForTeacher(ObservableCollection<LessonInfo> infos, int? userId)
        {
            Statistics statistics = new Statistics();

            int sumMarks = 0;
            int countMarks = 0;
            int allPresent = 0;
            int countPresent = 0;

            var userIdInfos = infos.Where(i => i.UserId == userId);
            foreach (var item in userIdInfos)
            {
                if (item.Grade != null)
                {
                    sumMarks += item.Grade.Value;
                    countMarks += 1;
                }

                allPresent += 1;
                if (item.IsPresent != null && item.IsPresent == true)
                {
                    countPresent += 1;
                }
            }

            statistics.UserFullName = userIdInfos?.FirstOrDefault()?.User?.FullName;
            statistics.UserId = userIdInfos?.FirstOrDefault()?.User?.Id;

            statistics.AverageMark = ((sumMarks / countMarks) * 100) / 12;
            statistics.PercentagePresents = (countPresent * 100) / allPresent;

            return statistics;
        }

        public static PositionStatistics GetStudentRaiting(List<Statistics> groupStatistics)
        {
            PositionStatistics positions = new PositionStatistics();

            var orderByMark = groupStatistics.OrderByDescending(i => i.AverageMark).ToList();
            positions.GroupPositionByMark = orderByMark.FindIndex(i => i.UserId == SimpleService.LoggedUser.Id) + 1;

            var orderByPresent = groupStatistics.OrderByDescending(i => i.PercentagePresents).ToList();
            positions.GroupPositionByPresent = orderByPresent.FindIndex(i => i.UserId == SimpleService.LoggedUser.Id) + 1;

            positions.CountStudentsInGroup = groupStatistics.Count;

            return positions;
        }


        public static Statistics InitStudentStatistics(ObservableCollection<LessonInfo> infos)
        {
            Statistics statistics = new Statistics();

            int sumMarks = 0;
            int countMarks = 0;
            int allPresent = 0;
            int countPresent = 0;

            foreach (var item in infos)
            {
                if (item.Grade != null)
                {
                    sumMarks += item.Grade.Value;
                    countMarks += 1;
                }

                allPresent += 1;
                if (item.IsPresent != null && item.IsPresent == true)
                {
                    countPresent += 1;
                }
            }

            statistics.UserFullName = infos?.FirstOrDefault()?.User?.FullName;
            statistics.UserId = infos?.FirstOrDefault()?.User?.Id;

            statistics.AverageMark = ((sumMarks / countMarks) * 100) / 12;
            statistics.PercentagePresents = (countPresent * 100) / allPresent;

            return statistics;
        }

        public static Statistics InitStudentStatisticsByMarkWithoutPresent(ObservableCollection<LessonInfo> infos, int? userId)
        {
            Statistics statistics = new Statistics();

            int sumMarks = 0;
            int countMarks = 0;

            var userIdInfos = infos.Where(i => i.UserId == userId);

            foreach (var item in userIdInfos)
            {
                if (item.Grade != null)
                {
                    sumMarks += item.Grade.Value;
                    countMarks += 1;
                }
            }

            statistics.UserFullName = userIdInfos?.FirstOrDefault()?.User?.FullName;
            statistics.UserId = userIdInfos?.FirstOrDefault()?.User?.Id;

            statistics.AverageMark = sumMarks / countMarks;

            return statistics;
        }

    }
}
