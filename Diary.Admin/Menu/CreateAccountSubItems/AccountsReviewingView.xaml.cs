using System.Windows.Controls;

namespace Diary.Admin.Menu.CreateAccountSubItems
{
    public partial class AccountsReviewingView
    {
        public AccountsReviewingView()
        {
            InitializeComponent();
        }

        private void Users_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1 + ".";
        }
    }
}