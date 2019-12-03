using System.Windows.Controls;

namespace Diary.Admin.Menu.EducationalProcessSubItems
{
    public partial class CourcesView
    {
        public CourcesView()
        {
            InitializeComponent();
        }

        private void DataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1 + ".";
        }
    }
}