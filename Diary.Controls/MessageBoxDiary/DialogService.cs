using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Diary.Controls.MessageBoxDiary
{
    public static class DialogService
    {
        public static async Task<MessageDialogResult> ShowMessage(
            string message, MessageDialogStyle dialogStyle)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            return await metroWindow.ShowMessageAsync(
                "Diary", message, dialogStyle, metroWindow.MetroDialogOptions);
        }
    }
}