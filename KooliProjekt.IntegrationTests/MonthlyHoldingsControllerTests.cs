using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.MonthlyHoldings;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    // 13.02.2026 - iga Web API kontrolleri jaoks oma klass
    [Collection("Sequential")]
    public class MonthlyHoldingsControllerTests : TestBase
    {
        private async Task SeedRelatedDataAsync()
        {
            await DbContext.AssetClasses.AddAsync(new AssetClass { Name = "Aktsiafondid" });
            await DbContext.SaveChangesAsync();

            await DbContext.Assets.AddAsync(new Asset { AssetClassID = 1, Name = "Vanguard S&P 500", Ticker = "VOO" });
            await DbContext.MonthlyStates.AddAsync(new MonthlyState
            {
                StateDate = new DateTime(2024, 1, 31),
                UninvestedCash = 1000m,
                Deposits = 5000m,
                Withdrawals = 500m,
                TotalPortfolioValue = 45000m
            });
            await DbContext.SaveChangesAsync();
        }

        private async Task<MonthlyHolding> SeedMonthlyHoldingAsync()
        {
            await SeedRelatedDataAsync();

            var holding = new MonthlyHolding
            {
                StateID = 1,
                AssetID = 1,
                Quantity = 10m,
                Value = 2500m
            };

            await DbContext.MonthlyHoldings.AddAsync(holding);
            await DbContext.SaveChangesAsync();

            return holding;
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/MonthlyHoldings/List/?page=1&pageSize=10";
            await SeedMonthlyHoldingAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<MonthlyHoldingListItemDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Single(response.Value.Results);
            Assert.Equal("Vanguard S&P 500", response.Value.Results.First().AssetName);
        }

        [Fact]
        public async Task List_should_return_holdings_matching_search()
        {
            // Arrange
            var url = "/api/MonthlyHoldings/List/?page=1&pageSize=10&assetID=1";
            await SeedMonthlyHoldingAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<MonthlyHoldingListItemDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.Single(response.Value.Results);
            Assert.Equal(1, response.Value.Results.First().AssetID);
        }

        [Fact]
        public async Task List_should_return_bad_request_for_invalid_page()
        {
            // Arrange
            var url = "/api/MonthlyHoldings/List/?page=0&pageSize=10";

            // Act
            using var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Get_should_return_monthly_holding()
        {
            // Arrange
            var holding = await SeedMonthlyHoldingAsync();
            var url = "/api/MonthlyHoldings/Get/?id=" + holding.Id;

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<MonthlyHoldingDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(holding.Id, response.Value.Id);
            Assert.Equal(10m, response.Value.Quantity);
            Assert.Equal("Vanguard S&P 500", response.Value.AssetName);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_monthly_holding()
        {
            // Arrange
            var url = "/api/MonthlyHoldings/Get/?id=131";

            // Act
            using var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_add_new_monthly_holding()
        {
            // Arrange
            var url = "/api/MonthlyHoldings/Save/";
            await SeedRelatedDataAsync();

            var command = new SaveMonthlyHoldingCommand
            {
                Id = 0,
                StateID = 1,
                AssetID = 1,
                Quantity = 10m,
                Value = 2500m
            };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveMonthlyHoldingCommand>(url, command);
            var holdingFromDb = await DbContext.MonthlyHoldings
                .AsNoTracking()
                .Where(holding => holding.AssetID == 1)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(holdingFromDb);
            Assert.Equal(2500m, holdingFromDb.Value);
        }

        [Fact]
        public async Task Save_should_update_existing_monthly_holding()
        {
            // Arrange
            var url = "/api/MonthlyHoldings/Save/";
            var holding = await SeedMonthlyHoldingAsync();

            var command = new SaveMonthlyHoldingCommand
            {
                Id = holding.Id,
                StateID = 1,
                AssetID = 1,
                Quantity = 25m,
                Value = 6000m
            };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveMonthlyHoldingCommand>(url, command);
            // AsNoTracking - loeme vaartused andmebaasist, mitte EF-i puhvrist
            var holdingFromDb = await DbContext.MonthlyHoldings
                .AsNoTracking()
                .Where(item => item.Id == holding.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(holdingFromDb);
            Assert.Equal(25m, holdingFromDb.Quantity);
            Assert.Equal(6000m, holdingFromDb.Value);
        }

        [Fact]
        public async Task Save_should_work_with_missing_monthly_holding()
        {
            // Arrange
            var url = "/api/MonthlyHoldings/Save/";
            await SeedRelatedDataAsync();

            var command = new SaveMonthlyHoldingCommand
            {
                Id = 10,
                StateID = 1,
                AssetID = 1,
                Quantity = 10m,
                Value = 2500m
            };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveMonthlyHoldingCommand>(url, command);
            var holdingFromDb = await DbContext.MonthlyHoldings
                .AsNoTracking()
                .Where(holding => holding.Id == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(holdingFromDb);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_monthly_holding()
        {
            // Arrange
            var url = "/api/MonthlyHoldings/Save/";
            var command = new SaveMonthlyHoldingCommand
            {
                Id = 0,
                StateID = 0,
                AssetID = 0,
                Quantity = -1m,
                Value = -1m
            };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveMonthlyHoldingCommand>(url, command);
            var holdingFromDb = await DbContext.MonthlyHoldings
                .AsNoTracking()
                .Where(holding => holding.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(holdingFromDb);
        }

        [Fact]
        public async Task Delete_should_remove_existing_monthly_holding()
        {
            // Arrange
            var url = "/api/MonthlyHoldings/Delete/";
            var holding = await SeedMonthlyHoldingAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = holding.Id })
            };
            using var response = await Client.SendAsync(request);
            var holdingFromDb = await DbContext.MonthlyHoldings
                .AsNoTracking()
                .Where(item => item.Id == holding.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(holdingFromDb);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_monthly_holding()
        {
            // Arrange
            var url = "/api/MonthlyHoldings/Delete/";

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
