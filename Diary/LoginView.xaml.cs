using System;
using System.Windows;
using System.Windows.Controls;
using Diary.Settings;
using MahApps.Metro.Controls;

namespace Diary
{
    public partial class LoginView : MetroWindow
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((LoginViewModel)this.DataContext).SecurePassword= ((PasswordBox)sender).SecurePassword; }
        }
    }
}