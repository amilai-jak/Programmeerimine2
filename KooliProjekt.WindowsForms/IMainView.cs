using System.Collections.Generic;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    // 26.03.2026 - MVP mustri View interface
    public interface IMainView
    {
        IList<Asset> DataSource { get; set; }
        Asset SelectedItem { get; set; }
        void SetPresenter(MainViewPresenter presenter);
        void ShowError(string message, OperationResult result);
        int CurrentId { get; set; }
        string CurrentName { get; set; }
        string CurrentTicker { get; set; }
        int CurrentAssetClassID { get; set; }
        bool CurrentIsRealEstate { get; set; }

        // 27.03.2026 - kustutamise kinnitus
        bool ConfirmDelete();
    }
}
