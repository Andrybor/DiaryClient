using System.Windows.Controls;

namespace Diary.Admin.Menu.InfoSubItems
{
    public partial class LibraryView
    {
        public LibraryView()
        {
            InitializeComponent();
        }

        private void DataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1 + ".";
        }
    }
}