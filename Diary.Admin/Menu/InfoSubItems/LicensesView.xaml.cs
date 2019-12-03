using System.Windows.Controls;

namespace Diary.Admin.Menu.InfoSubItems
{
    /// <summary>
    /// Interaction logic for LicensesView.xaml
    /// </summary>
    public partial class LicensesView : UserControl
    {
        public LicensesView()
        {
            InitializeComponent();
        }
        private void DataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1 + ".";
        }
    }
}
