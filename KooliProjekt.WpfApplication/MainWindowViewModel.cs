using System.Collections.ObjectModel;
using System.Windows.Input;
using KooliProjekt.WpfApplication.Api;

namespace KooliProjekt.WpfApplication
{
    // 09.04.2026 - MVVM view model
    // 10.04.2026 - view model kasutab API klienti ja NotifyPropertyChanged baasklassi
    // 16.04.2026 - commandid ja IDialogProvider
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly ObservableCollection<Asset> _data;
        private readonly IApiClient _apiClient;
        private readonly IDialogProvider _dialogProvider;

        public ICommand AddNewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        private Asset _selectedItem;

        // Tuleviku huvides on view modelil kaks konstruktorit
        public MainWindowViewModel() : this(new ApiClient(), new DialogProvider())
        {
        }

        public MainWindowViewModel(IApiClient apiClient, IDialogProvider dialogProvider)
        {
            _apiClient = apiClient;
            _dialogProvider = dialogProvider;
            _data = new ObservableCollection<Asset>();

            AddNewCommand = new RelayCommand<Asset>(
                asset =>
                {
                    SelectedItem = new Asset();
                });
            SaveCommand = new RelayCommand<Asset>(
                async asset =>
                {
                    var result = await _apiClient.Save(asset);
                    if (result.HasErrors)
                    {
                        ShowError("Failed to save data", result);
                        return;
                    }

                    await LoadData();
                },
                asset =>
                {
                    return SelectedItem != null;
                });
            DeleteCommand = new RelayCommand<Asset>(
                async asset =>
                {
                    var canDelete = _dialogProvider.Confirm("Are you sure you want to delete this item?");
                    if (!canDelete)
                    {
                        return;
                    }

                    var result = await _apiClient.Delete(asset.Id);
                    if (result.HasErrors)
                    {
                        ShowError("Failed to delete data", result);
                        return;
                    }

                    await LoadData();
                },
                asset =>
                {
                    return SelectedItem != null && SelectedItem.Id != 0;
                });
        }

        public async Task LoadData()
        {
            var data = await _apiClient.List(1, 100);
            _data.Clear();

            if (data.HasErrors)
            {
                ShowError("Failed to load data", data);
                return;
            }

            foreach (var item in data.Value.Results)
            {
                _data.Add(item);
            }
        }

        public ObservableCollection<Asset> Data
        {
            get
            {
                return _data;
            }
        }

        public Asset SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public void ShowError(string message, OperationResult result)
        {
            var error = message + "\r\n";
            var apiErrors = "";
            var propertyErrors = "";

            if (result.Errors != null)
            {
                foreach (var apiError in result.Errors)
                {
                    apiErrors += apiError + "\r\n";
                }
            }

            if (result.PropertyErrors != null)
            {
                foreach (var propertyError in result.PropertyErrors)
                {
                    propertyErrors += propertyError.Key + ": " + propertyError.Value;
                }
            }

            if (!string.IsNullOrEmpty(apiErrors))
            {
                error += "\r\n" + apiErrors + "\r\n";
            }

            if (!string.IsNullOrEmpty(propertyErrors))
            {
                error += "\r\n" + propertyErrors;
            }

            error = error.Trim();

            _dialogProvider.ShowError(error);
        }
    }
}
