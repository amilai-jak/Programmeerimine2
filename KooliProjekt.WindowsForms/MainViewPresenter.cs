using System.Threading.Tasks;
using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    // 26.03.2026 - MVP mustri Presenter
    // 27.03.2026 - nupuvajutuste loogika kolis siia
    public class MainViewPresenter
    {
        private readonly IApiClient _apiClient;
        private readonly IMainView _mainView;

        private Asset _selectedItem;

        public MainViewPresenter(IApiClient apiClient, IMainView mainView)
        {
            _apiClient = apiClient;
            _mainView = mainView;
            _mainView.SetPresenter(this);
        }

        public async Task LoadData()
        {
            var response = await _apiClient.List(1, 100);
            if (response.HasErrors)
            {
                _mainView.ShowError("Viga andmete laadimisel", response);
                _mainView.DataSource = null;
                return;
            }

            _mainView.DataSource = response.Value.Results;
        }

        public void SetSelection(Asset selectedItem)
        {
            _selectedItem = selectedItem;

            if (_selectedItem == null)
            {
                _mainView.CurrentId = 0;
                _mainView.CurrentName = "";
                _mainView.CurrentTicker = "";
                _mainView.CurrentAssetClassID = 0;
                _mainView.CurrentIsRealEstate = false;
            }
            else
            {
                _mainView.CurrentId = _selectedItem.Id;
                _mainView.CurrentName = _selectedItem.Name;
                _mainView.CurrentTicker = _selectedItem.Ticker;
                _mainView.CurrentAssetClassID = _selectedItem.AssetClassID;
                _mainView.CurrentIsRealEstate = _selectedItem.IsRealEstate;
            }
        }

        public async Task Save()
        {
            var asset = new Asset();
            asset.Id = _mainView.CurrentId;
            asset.Name = _mainView.CurrentName;
            asset.Ticker = _mainView.CurrentTicker;
            asset.AssetClassID = _mainView.CurrentAssetClassID;
            asset.IsRealEstate = _mainView.CurrentIsRealEstate;

            var result = await _apiClient.Save(asset);
            if (result.HasErrors)
            {
                _mainView.ShowError("Viga salvestamisel", result);
                return;
            }

            await LoadData();
        }

        public async Task Delete()
        {
            if (!_mainView.ConfirmDelete())
            {
                return;
            }

            var result = await _apiClient.Delete(_mainView.CurrentId);
            if (result.HasErrors)
            {
                _mainView.ShowError("Viga kustutamisel", result);
                return;
            }

            await LoadData();
        }
    }
}
