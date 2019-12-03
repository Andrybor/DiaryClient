using System.Windows;
using System.Windows.Controls;

namespace Diary.Teacher.Menu.MainTeacherSubItems
{
    /// <summary>
    ///     Interaction logic for ScheduleView.xaml
    /// </summary>
    public partial class ScheduleView : UserControl
    {
        public ScheduleView()
        {
            InitializeComponent();
        }

        private void ScheduleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var data = DataContext as ScheduleViewModel;
            data?.AssignDates(ScheduleCalendar.SelectedDates);
        }
        private void DataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1 + ".";
        }
    }
}