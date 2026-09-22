using KooliProjekt.WindowsForms;
using KooliProjekt.WindowsForms.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WindowsForms.UnitTests
{
    // 02.04.2026 - presenteri uhiktestid
    public class MainViewPresenterTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IMainView> _mainViewMock;
        private readonly MainViewPresenter _presenter;

        public MainViewPresenterTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _mainViewMock = new Mock<IMainView>();
            _presenter = new MainViewPresenter(_apiClientMock.Object, _mainViewMock.Object);
        }

        [Fact]
        public async Task LoadData_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult<PagedResult<Asset>>();
            faultyResponse.Errors.Add("An error occurred while fetching data.");

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = null)
                .Verifiable();

            // Act
            await _presenter.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task LoadData_should_set_DataSource_with_valid_response()
        {
            // Arrange
            var validResponse = new OperationResult<PagedResult<Asset>>
            {
                Value = new PagedResult<Asset>
                {
                    Results = new List<Asset>
                    {
                        new Asset { Id = 1, Name = "Test Asset 1", Ticker = "TST1" },
                        new Asset { Id = 2, Name = "Test Asset 2", Ticker = "TST2" }
                    }
                }
            };

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(validResponse)
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = validResponse.Value.Results)
                .Verifiable();

            // Act
            await _presenter.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public void SetSelection_should_clear_fields_with_null_selection()
        {
            // Arrange
            var selectedItem = (Asset)null;

            _mainViewMock.SetupSet(view => view.CurrentId = 0).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentName = "").Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentTicker = "").Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentAssetClassID = 0).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentIsRealEstate = false).Verifiable();

            // Act
            _presenter.SetSelection(selectedItem);

            // Assert
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public void SetSelection_should_set_fields_with_valid_selection()
        {
            // Arrange
            var selectedItem = new Asset
            {
                Id = 5,
                AssetClassID = 3,
                Name = "Vanguard S&P 500",
                Ticker = "VOO",
                IsRealEstate = true
            };

            _mainViewMock.SetupSet(view => view.CurrentId = selectedItem.Id).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentName = selectedItem.Name).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentTicker = selectedItem.Ticker).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentAssetClassID = selectedItem.AssetClassID).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentIsRealEstate = selectedItem.IsRealEstate).Verifiable();

            // Act
            _presenter.SetSelection(selectedItem);

            // Assert
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult();
            faultyResponse.Errors.Add("An error occurred while saving data.");

            _apiClientMock
                .Setup(client => client.Save(It.IsAny<Asset>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            // Act
            await _presenter.Save();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_call_LoadData_with_valid_response()
        {
            // Arrange
            var validResponse = new OperationResult();

            _apiClientMock
                .Setup(client => client.Save(It.IsAny<Asset>()))
                .ReturnsAsync(validResponse)
                .Verifiable();
            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new OperationResult<PagedResult<Asset>>
                {
                    Value = new PagedResult<Asset>()
                })
                .Verifiable();

            // Act
            await _presenter.Save();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.Verify(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_return_when_user_didnot_confirmed()
        {
            // Arrange
            _mainViewMock
                .Setup(view => view.ConfirmDelete())
                .Returns(false)
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _mainViewMock.VerifyAll();
            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult();
            faultyResponse.Errors.Add("An error occurred while deleting data.");

            _mainViewMock
                .Setup(view => view.ConfirmDelete())
                .Returns(true)
                .Verifiable();
            _apiClientMock
                .Setup(client => client.Delete(It.IsAny<int>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _mainViewMock.VerifyAll();
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_call_LoadData_with_valid_response()
        {
            // Arrange
            var validResponse = new OperationResult();

            _mainViewMock
                .Setup(view => view.ConfirmDelete())
                .Returns(true)
                .Verifiable();
            _apiClientMock
                .Setup(client => client.Delete(It.IsAny<int>()))
                .ReturnsAsync(validResponse)
                .Verifiable();
            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new OperationResult<PagedResult<Asset>>
                {
                    Value = new PagedResult<Asset>()
                })
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.Verify(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()), Times.Never);
        }
    }
}
