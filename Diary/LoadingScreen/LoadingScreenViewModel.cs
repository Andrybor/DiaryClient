using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Diary.Framework.Infrastructure.Base;
using Diary.Framework.Infrastructure.Interfaces;

namespace Diary.LoadingScreen
{
    [Export(typeof(ILoadingScreen))]
    public class LoadingScreenViewModel : DScreen,ILoadingScreen
    {
        public Action CloseLoadingScreen { get; set; }

        protected async override void OnActivate()
        {
            base.OnActivate();

            await Task.Run(() => Task.Delay(TimeSpan.FromSeconds(3)));

            var view = this.GetView();
            if (view != null && view is LoadingScreenView v)
            {
                v.Visibility = System.Windows.Visibility.Collapsed;

                CloseLoadingScreen.Invoke();
            }
        }
    }
}
