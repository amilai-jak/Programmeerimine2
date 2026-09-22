using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Assets;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    // 13.02.2026 - iga Web API kontrolleri jaoks oma klass
    [Collection("Sequential")]
    public class AssetsControllerTests : TestBase
    {
        private async Task SeedAssetClassAsync()
        {
            await DbContext.AssetClasses.AddAsync(new AssetClass { Name = "Aktsiafondid" });
            await DbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Assets/List/?page=1&pageSize=10";
            await SeedAssetClassAsync();
            await DbContext.Assets.AddAsync(new Asset { AssetClassID = 1, Name = "Vanguard S&P 500", Ticker = "VOO" });
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<AssetListItemDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Single(response.Value.Results);
        }

        [Fact]
        public async Task List_should_return_assets_matching_search()
        {
            // Arrange
            var url = "/api/Assets/List/?page=1&pageSize=10&name=Vanguard";
            await SeedAssetClassAsync();
            await DbContext.Assets.AddAsync(new Asset { AssetClassID = 1, Name = "Vanguard S&P 500", Ticker = "VOO" });
            await DbContext.Assets.AddAsync(new Asset { AssetClassID = 1, Name = "iShares Core Bond", Ticker = "AGG" });
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<AssetListItemDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.Single(response.Value.Results);
            Assert.Equal("Vanguard S&P 500", response.Value.Results.First().Name);
        }

        [Fact]
        public async Task List_should_return_bad_request_for_invalid_page()
        {
            // Arrange
            var url = "/api/Assets/List/?page=0&pageSize=10";

            // Act
            using var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Get_should_return_asset()
        {
            // Arrange
            await SeedAssetClassAsync();
            await DbContext.Assets.AddAsync(new Asset { AssetClassID = 1, Name = "Vanguard S&P 500", Ticker = "VOO" });
            await DbContext.SaveChangesAsync();

            var url = "/api/Assets/Get/?id=1";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<AssetDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(1, response.Value.Id);
            Assert.Equal("Vanguard S&P 500", response.Value.Name);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_asset()
        {
            // Arrange
            var url = "/api/Assets/Get/?id=131";

            // Act
            using var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_add_new_asset()
        {
            // Arrange
            var url = "/api/Assets/Save/";
            await SeedAssetClassAsync();

            var command = new SaveAssetCommand
            {
                Id = 0,
                AssetClassID = 1,
                Name = "New asset",
                Ticker = "NEW",
                IsRealEstate = false
            };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveAssetCommand>(url, command);
            var assetFromDb = await DbContext.Assets
                .Where(asset => asset.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(assetFromDb);
            Assert.Equal("New asset", assetFromDb.Name);
        }

        [Fact]
        public async Task Save_should_update_existing_asset()
        {
            // Arrange
            var url = "/api/Assets/Save/";
            await SeedAssetClassAsync();
            await DbContext.Assets.AddAsync(new Asset { AssetClassID = 1, Name = "Old asset", Ticker = "OLD" });
            await DbContext.SaveChangesAsync();

            var command = new SaveAssetCommand
            {
                Id = 1,
                AssetClassID = 1,
                Name = "Updated asset",
                Ticker = "UPD",
                IsRealEstate = true
            };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveAssetCommand>(url, command);
            // AsNoTracking - loeme vaartused andmebaasist, mitte EF-i puhvrist
            var assetFromDb = await DbContext.Assets
                .AsNoTracking()
                .Where(asset => asset.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(assetFromDb);
            Assert.Equal("Updated asset", assetFromDb.Name);
            Assert.True(assetFromDb.IsRealEstate);
        }

        [Fact]
        public async Task Save_should_work_with_missing_asset()
        {
            // Arrange
            var url = "/api/Assets/Save/";
            var command = new SaveAssetCommand { Id = 10, AssetClassID = 1, Name = "Test asset" };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveAssetCommand>(url, command);
            var assetFromDb = await DbContext.Assets
                .Where(asset => asset.Id == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(assetFromDb);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_asset()
        {
            // Arrange
            var url = "/api/Assets/Save/";
            var command = new SaveAssetCommand { Id = 0, AssetClassID = 0, Name = "" };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveAssetCommand>(url, command);
            var assetFromDb = await DbContext.Assets
                .Where(asset => asset.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(assetFromDb);
        }

        [Fact]
        public async Task Delete_should_remove_existing_asset()
        {
            // Arrange
            var url = "/api/Assets/Delete/";
            await SeedAssetClassAsync();
            var asset = new Asset { AssetClassID = 1, Name = "Vanguard S&P 500", Ticker = "VOO" };
            await DbContext.Assets.AddAsync(asset);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = asset.Id })
            };
            using var response = await Client.SendAsync(request);
            var assetFromDb = await DbContext.Assets
                .Where(item => item.Id == asset.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(assetFromDb);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_asset()
        {
            // Arrange
            var url = "/api/Assets/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = 101 })
            };
            using var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
