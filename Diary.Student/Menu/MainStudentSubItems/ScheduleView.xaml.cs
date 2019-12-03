using System;
using System.Windows;
using System.Windows.Controls;

namespace Diary.Student.Menu.MainStudentSubItems
{
    /// <summary>
    ///     Interaction logic for ScheduleView.xaml
    /// </summary>
    public partial class ScheduleView
    {
        public ScheduleView()
        {
            InitializeComponent();
        }

        private void ScheduleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var datad = DataContext as ScheduleViewModel;
            datad?.AssignDates(ScheduleCalendar.SelectedDates);
        }
    }
}