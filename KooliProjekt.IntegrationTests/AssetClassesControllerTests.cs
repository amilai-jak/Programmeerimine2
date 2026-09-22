using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.AssetClasses;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    // 13.02.2026 - iga Web API kontrolleri jaoks oma klass
    [Collection("Sequential")]
    public class AssetClassesControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/AssetClasses/List/?page=1&pageSize=10";
            await DbContext.AssetClasses.AddAsync(new AssetClass { Name = "Aktsiafondid" });
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<AssetClassListItemDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Single(response.Value.Results);
        }

        [Fact]
        public async Task List_should_return_asset_classes_matching_search()
        {
            // Arrange
            var url = "/api/AssetClasses/List/?page=1&pageSize=10&name=fond";
            await DbContext.AssetClasses.AddAsync(new AssetClass { Name = "Aktsiafondid" });
            await DbContext.AssetClasses.AddAsync(new AssetClass { Name = "Kruutovaluutad" });
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<AssetClassListItemDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.Single(response.Value.Results);
            Assert.Equal("Aktsiafondid", response.Value.Results.First().Name);
        }

        [Fact]
        public async Task List_should_return_bad_request_for_invalid_page()
        {
            // Arrange
            var url = "/api/AssetClasses/List/?page=0&pageSize=10";

            // Act
            using var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Get_should_return_asset_class()
        {
            // Arrange
            var assetClass = new AssetClass { Name = "Aktsiafondid" };
            await DbContext.AssetClasses.AddAsync(assetClass);
            await DbContext.SaveChangesAsync();

            var url = "/api/AssetClasses/Get/?id=" + assetClass.Id;

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<AssetClassDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(assetClass.Id, response.Value.Id);
            Assert.Equal("Aktsiafondid", response.Value.Name);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_asset_class()
        {
            // Arrange
            var url = "/api/AssetClasses/Get/?id=131";

            // Act
            using var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_add_new_asset_class()
        {
            // Arrange
            var url = "/api/AssetClasses/Save/";
            var command = new SaveAssetClassCommand { Id = 0, Name = "Uus klass" };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveAssetClassCommand>(url, command);
            var assetClassFromDb = await DbContext.AssetClasses
                .AsNoTracking()
                .Where(assetClass => assetClass.Name == "Uus klass")
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(assetClassFromDb);
        }

        [Fact]
        public async Task Save_should_update_existing_asset_class()
        {
            // Arrange
            var url = "/api/AssetClasses/Save/";
            var assetClass = new AssetClass { Name = "Aktsiafondid" };
            await DbContext.AssetClasses.AddAsync(assetClass);
            await DbContext.SaveChangesAsync();

            var command = new SaveAssetClassCommand { Id = assetClass.Id, Name = "Updated class" };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveAssetClassCommand>(url, command);
            // AsNoTracking - loeme vaartused andmebaasist, mitte EF-i puhvrist
            var assetClassFromDb = await DbContext.AssetClasses
                .AsNoTracking()
                .Where(item => item.Id == assetClass.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(assetClassFromDb);
            Assert.Equal("Updated class", assetClassFromDb.Name);
        }

        [Fact]
        public async Task Save_should_work_with_missing_asset_class()
        {
            // Arrange
            var url = "/api/AssetClasses/Save/";
            var command = new SaveAssetClassCommand { Id = 10, Name = "Test class" };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveAssetClassCommand>(url, command);
            var assetClassFromDb = await DbContext.AssetClasses
                .AsNoTracking()
                .Where(assetClass => assetClass.Id == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(assetClassFromDb);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_asset_class()
        {
            // Arrange
            var url = "/api/AssetClasses/Save/";
            var command = new SaveAssetClassCommand { Id = 0, Name = "" };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveAssetClassCommand>(url, command);
            var assetClassFromDb = await DbContext.AssetClasses
                .AsNoTracking()
                .Where(assetClass => assetClass.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(assetClassFromDb);
        }

        [Fact]
        public async Task Delete_should_remove_existing_asset_class()
        {
            // Arrange
            var url = "/api/AssetClasses/Delete/";
            var assetClass = new AssetClass { Name = "Aktsiafondid" };
            await DbContext.AssetClasses.AddAsync(assetClass);
            await DbContext.SaveChangesAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = assetClass.Id })
            };
            using var response = await Client.SendAsync(request);
            var assetClassFromDb = await DbContext.AssetClasses
                .AsNoTracking()
                .Where(item => item.Id == assetClass.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(assetClassFromDb);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_asset_class()
        {
            // Arrange
            var url = "/api/AssetClasses/Delete/";

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
