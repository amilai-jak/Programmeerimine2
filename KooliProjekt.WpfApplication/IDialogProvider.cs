namespace KooliProjekt.WpfApplication
{
    // 16.04.2026 - dialoogide kuvamise interface (testitav, sest MessageBox'i testida ei saa)
    public interface IDialogProvider
    {
        bool Confirm(string message);
        void ShowError(string message);
    }
}
