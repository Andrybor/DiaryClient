using System;
using System.Collections.Generic;
using System.Linq;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Models;
using MaterialDesignThemes.Wpf;

namespace Diary.Student.Menu.MainStudentSubItems
{
    [Plugin("Progress", DPlugin.Progress, DPlugin.MainStudent, 2, PackIconKind.ChartLine)]
    public class ProgressViewModel : DScreen
    {
        private List<LessonInfo> _infos;
        private Subject _subject;
        private List<Subject> _subjects;


        public List<LessonInfo> AllLessonInfos { get; set; } 
        public Subject Subject
        {
            get => _subject;
            set
            {
                _subject = value;
                if(_subject!=null)
                    Infos = AllLessonInfos.Where(i => i.Schedule.Subject.Id == Subject.Id).ToList();
                NotifyOfPropertyChange(() => Subject);             
            }
        }

        public List<Subject> Subjects
        {
            get => _subjects;
            set
            {
                _subjects = value;
                NotifyOfPropertyChange(() => Subjects);
            }
        }

        public List<LessonInfo> Infos
        {
            get => _infos;
            set
            {
                _infos = value;
                NotifyOfPropertyChange(() => Infos);
            }
        }


        public async void Init()
        {
            var infos = await SimpleService.Get<List<LessonInfo>>(Controller.LessonInfo, Method.Get,
                SimpleService.LoggedUser.Id);
            if (infos != null)
            {
                AllLessonInfos = infos;

                Infos = infos;

                Subjects = infos.Select(i => i.Schedule.Subject).DistinctBy(i => i.Id).ToList();

                Subject = Subjects.FirstOrDefault();

                Infos = infos.Where(i => i.Schedule.Subject.Id == Subject.Id).ToList();
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            Init();   
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