using System.Windows;

namespace KooliProjekt.WpfApplication
{
    // 16.04.2026 - paris dialoogide kuvaja
    public class DialogProvider : IDialogProvider
    {
        public bool Confirm(string message)
        {
            var result = MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo);

            return result == MessageBoxResult.Yes;
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
