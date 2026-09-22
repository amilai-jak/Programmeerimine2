using Moq;

namespace KooliProjekt.WpfApplication.UnitTests
{
    // 17.04.2026 - WPF view modeli uhiktestid
    public class MainWindowViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IDialogProvider> _dialogProviderMock;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _dialogProviderMock = new Mock<IDialogProvider>();
            _viewModel = new MainWindowViewModel(_apiClientMock.Object, _dialogProviderMock.Object);
        }

        [Fact]
        public void SelectedItem_should_return_correct_item()
        {
            // Arrange
            var item = new Asset { Id = 1, Name = "Test" };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.Equal(item, _viewModel.SelectedItem);
        }

        [Fact]
        public void SelectedItem_should_call_notify_property_changed()
        {
            // Arrange
            var item = new Asset { Id = 1, Name = "Test" };
            var propertyChangedRaised = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainWindowViewModel.SelectedItem))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public async Task LoadData_should_load_data_from_api_client()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Asset>>
            {
                Value = new PagedResult<Asset>
                {
                    Results = new List<Asset>
                    {
                        new Asset { Id = 1, Name = "Test 1" },
                        new Asset { Id = 2, Name = "Test 2" }
                    }
                }
            };

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(apiResult)
                .Verifiable();

            // Act
            await _viewModel.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Equal(2, _viewModel.Data.Count);
            Assert.Equal(1, _viewModel.Data[0].Id);
            Assert.Equal(2, _viewModel.Data[1].Id);
        }

        [Fact]
        public async Task LoadData_should_show_error_when_api_client_fails()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Asset>>
            {
                Errors = new List<string> { "Error" }
            };

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(apiResult)
                .Verifiable();

            // Act
            await _viewModel.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Empty(_viewModel.Data);
        }

        [Fact]
        public void AddNew_Command_Should_Set_Empty_SelectedItem()
        {
            // Arrange
            _viewModel.SelectedItem = new Asset { Id = 7, Name = "Vana vara" };

            // Act
            _viewModel.AddNewCommand.Execute(null);

            // Assert
            Assert.NotNull(_viewModel.SelectedItem);
            Assert.Equal(0, _viewModel.SelectedItem.Id);
            Assert.Null(_viewModel.SelectedItem.Name);
        }

        [Fact]
        public void SaveCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var loadDataApiResult = new OperationResult<PagedResult<Asset>>
            {
                Value = new PagedResult<Asset>
                {
                    Results = new List<Asset>
                    {
                        new Asset { Id = 1, Name = "Test 1" },
                        new Asset { Id = 2, Name = "Test 2" }
                    }
                }
            };
            var saveDataApiResult = new OperationResult();
            var listToSave = new Asset { Id = 1, Name = "Test" };

            _apiClientMock.Setup(client => client.Save(It.IsAny<Asset>()))
                .ReturnsAsync(saveDataApiResult)
                .Verifiable();
            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(loadDataApiResult)
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(listToSave);

            // Arrange
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public async Task SaveCommand_should_return_when_api_gave_error()
        {
            // Arrange
            var saveDataApiResult = new OperationResult();
            saveDataApiResult.Errors.Add("Error");
            var listToSave = new Asset { Id = 1, Name = "Test" };

            _apiClientMock.Setup(client => client.Save(It.IsAny<Asset>()))
                .ReturnsAsync(saveDataApiResult)
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(listToSave);

            // Assert
            _apiClientMock.VerifyAll();
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _dialogProviderMock.Verify(dialog => dialog.ShowError(It.IsAny<string>()), Times.Once);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task SaveCommand_can_execute_when_selected_item_is_not_null()
        {
            // Arrange
            _viewModel.SelectedItem = new Asset { Id = 1, Name = "Test" };

            // Act
            var canExecute = _viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);

            // Arrange
            _viewModel.SelectedItem = null;

            // Act
            canExecute = _viewModel.SaveCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task DeleteCommand_should_return_when_no_confirmation()
        {
            // Arrange
            // DeleteCommand teeb return kui IDialogClient.Confirm() tagastab false
            var listToDelete = new Asset { Id = 1, Name = "Test" };

            _dialogProviderMock.Setup(dialog => dialog.Confirm(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(listToDelete);

            // Assert
            _dialogProviderMock.VerifyAll();
            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task DeleteCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var loadDataApiResult = new OperationResult<PagedResult<Asset>>
            {
                Value = new PagedResult<Asset>
                {
                    Results = new List<Asset>
                    {
                        new Asset { Id = 2, Name = "Test 2" }
                    }
                }
            };
            var deleteDataApiResult = new OperationResult();
            var listToDelete = new Asset { Id = 1, Name = "Test" };

            _dialogProviderMock.Setup(dialog => dialog.Confirm(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            _apiClientMock.Setup(client => client.Delete(It.IsAny<int>()))
                .ReturnsAsync(deleteDataApiResult)
                .Verifiable();
            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(loadDataApiResult)
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(listToDelete);

            // Assert
            _apiClientMock.VerifyAll();
            _dialogProviderMock.VerifyAll();
            Assert.Single(_viewModel.Data);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task DeleteCommand_should_return_when_api_gave_error()
        {
            // Arrange
            var deleteDataApiResult = new OperationResult();
            deleteDataApiResult.Errors.Add("Error");
            var listToDelete = new Asset { Id = 1, Name = "Test" };

            _dialogProviderMock.Setup(dialog => dialog.Confirm(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            _apiClientMock.Setup(client => client.Delete(It.IsAny<int>()))
                .ReturnsAsync(deleteDataApiResult)
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(listToDelete);

            // Assert
            _apiClientMock.VerifyAll();
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _dialogProviderMock.Verify(dialog => dialog.ShowError(It.IsAny<string>()), Times.Once);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task DeleteCommand_can_execute_when_selected_item_is_not_null_and_id_is_not_zero()
        {
            // Arrange
            _viewModel.SelectedItem = null;

            // Act
            var canExecute = _viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);

            // Arrange
            _viewModel.SelectedItem = new Asset { Id = 0, Name = "Uus vara" };

            // Act
            canExecute = _viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);

            // Arrange
            _viewModel.SelectedItem = new Asset { Id = 5, Name = "Olemasolev vara" };

            // Act
            canExecute = _viewModel.DeleteCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);

            await Task.CompletedTask;
        }
    }
}
