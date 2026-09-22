using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    internal static class Program
    {
        // 27.02.2026 - Windows Formsi programmi sisenemispunkt
        // 19.03.2026 - API klient antakse vormile ette
        // 26.03.2026 - MVP: presenter luuakse siin ja antakse vormile
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            IApiClient apiClient = new ApiClient();

            var view = new Form1(apiClient);
            var presenter = new MainViewPresenter(apiClient, view);

            Application.Run(view);
        }
    }
}
