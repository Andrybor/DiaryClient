using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Diary.Common;
using Diary.Framework;
using Diary.Framework.Infrastructure.Attributes;
using Diary.Framework.Infrastructure.Base;
using Diary.Repositories;
using Diary.Repositories.Enums;
using Diary.Repositories.Models;
using MahApps.Metro.IconPacks;

namespace Diary.Admin.Menu.InfoSubItems
{
    [Plugin("News", DPlugin.News, DPlugin.Info, 1, PackIconModernKind.Eye)]
    public class NewsViewModel : DScreen
    {
        private ObservableCollection<News> _allNews;
        private News _news;
        private string _selectedAccountType;

        public NewsViewModel()
        {
            AccountTypes = new List<string>
            {
                TypeAccount.Teacher.ToString().Translate(),
                TypeAccount.Student.ToString().Translate()
            };
        }

        public IEnumerable<string> AccountTypes { get; set; }

        public string SelectedAccountType
        {
            get => _selectedAccountType;
            set
            {
                _selectedAccountType = value;
                NotifyOfPropertyChange(() => SelectedAccountType);
                InitAllNews();
            }
        }

        public ObservableCollection<News> AllNews
        {
            get => _allNews;
            set
            {
                _allNews = value;
                NotifyOfPropertyChange(() => AllNews);
            }
        }

        public News News
        {
            get => _news;
            set
            {
                _news = value;
                NotifyOfPropertyChange(() => News);
            }
        }

        public async void Create()
        {
            var result = await SimpleService.Post(Controller.AccountType, Method.Get, SelectedAccountType.GetKey());
            if (result != null)
            {
                News.AccountTypeId = Convert.ToInt32(result);
                await SimpleService.Post(Controller.News, Method.Create, News);
                Init();
            }
        }

        public void Init()
        {
            SelectedAccountType = AccountTypes.FirstOrDefault();

            InitAllNews();

            News = new News();
        }

        private async void InitAllNews()
        {
            var allNews = await SimpleService.Get<ObservableCollection<News>>(Controller.News, Method.GetAll);

            var translatedSelectedAccountType = SelectedAccountType.GetKey();

            if (allNews != null && !string.IsNullOrEmpty(translatedSelectedAccountType))
            {
                var result = await SimpleService.Post(Controller.AccountType, Method.Get, translatedSelectedAccountType);
                if (result != null)
                    AllNews = new ObservableCollection<News>(allNews.Where(i =>
                        i.AccountTypeId == Convert.ToInt32(result)));
            }
        }

        public async void Delete(object item)
        {
            if (item is News news)
            {
                await SimpleService.Post(Controller.News, Method.Delete, news.Id);
                InitAllNews();
            }
        }

        protected override void OnActivate()
        {
            Init();

            base.OnActivate();
        }
    }
}