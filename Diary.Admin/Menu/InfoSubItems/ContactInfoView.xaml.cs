using System.Windows.Controls;

namespace Diary.Admin.Menu.InfoSubItems
{
    /// <summary>
    /// Interaction logic for ContactInfoView.xaml
    /// </summary>
    public partial class ContactInfoView 
    {
        public ContactInfoView()
        {
            InitializeComponent();
        }

        private void DataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1 + ".";
        }
    }
}
