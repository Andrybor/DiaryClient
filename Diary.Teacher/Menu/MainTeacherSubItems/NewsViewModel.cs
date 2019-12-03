using System;
using System.Collections.Generic;
using System.Linq;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Enums;
using Diary.Repositories.Models;
using MaterialDesignThemes.Wpf;

namespace Diary.Teacher.Menu.MainTeacherSubItems
{
    [Plugin("News", DPlugin.News, DPlugin.MainTeacher, 4, PackIconKind.Web)]
    public class NewsViewModel : DScreen
    {
        private List<News> _news;

        public List<News> News
        {
            get => _news;
            set
            {
                _news = value;
                NotifyOfPropertyChange(() => News);
            }
        }

        private async void InitNews()
        {
            var news = await SimpleService.Get<List<News>>(Controller.News, Method.GetAll);
            if (news != null)
            {
                var result = await SimpleService.Post(Controller.AccountType, Method.Get, TypeAccount.Teacher.ToString());
                if (result != null)
                    News = news.Where(i => i.AccountTypeId == Convert.ToInt32(result)).ToList();
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            InitNews();
        }
    }
}