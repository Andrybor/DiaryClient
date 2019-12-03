using System.ComponentModel;
using System.Runtime.CompilerServices;
using Diary.Repositories.Annotations;

namespace Diary.ReportBugDioalog
{
    public class BugReportDialogViewModel : INotifyPropertyChanged
    {
        private string _exception;

        public string Exception
        {
            get => _exception;
            set
            {
                _exception = value;
                OnPropertyChanged(nameof(Exception));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
