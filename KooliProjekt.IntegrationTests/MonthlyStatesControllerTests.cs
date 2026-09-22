using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.MonthlyStates;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    // 13.02.2026 - iga Web API kontrolleri jaoks oma klass
    [Collection("Sequential")]
    public class MonthlyStatesControllerTests : TestBase
    {
        private async Task<MonthlyState> SeedMonthlyStateAsync()
        {
            var state = new MonthlyState
            {
                StateDate = new DateTime(2024, 1, 31),
                UninvestedCash = 1000m,
                Deposits = 5000m,
                Withdrawals = 500m,
                TotalPortfolioValue = 45000m
            };

            await DbContext.MonthlyStates.AddAsync(state);
            await DbContext.SaveChangesAsync();

            return state;
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/MonthlyStates/List/?page=1&pageSize=10";
            await SeedMonthlyStateAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<MonthlyStateListItemDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Single(response.Value.Results);
        }

        [Fact]
        public async Task List_should_return_monthly_states_matching_search()
        {
            // Arrange
            var url = "/api/MonthlyStates/List/?page=1&pageSize=10&stateDateFrom=2024-06-01";
            await SeedMonthlyStateAsync();
            await DbContext.MonthlyStates.AddAsync(new MonthlyState
            {
                StateDate = new DateTime(2024, 7, 31),
                UninvestedCash = 2000m,
                Deposits = 4000m,
                Withdrawals = 0m,
                TotalPortfolioValue = 50000m
            });
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<MonthlyStateListItemDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.Single(response.Value.Results);
            Assert.Equal(new DateTime(2024, 7, 31), response.Value.Results.First().StateDate);
        }

        [Fact]
        public async Task List_should_return_bad_request_for_invalid_page()
        {
            // Arrange
            var url = "/api/MonthlyStates/List/?page=0&pageSize=10";

            // Act
            using var response = await Client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Get_should_return_monthly_state()
        {
            // Arrange
            var state = await SeedMonthlyStateAsync();
            var url = "/api/MonthlyStates/Get/?id=" + state.Id;

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<MonthlyStateDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(state.Id, response.Value.Id);
            Assert.Equal(45000m, response.Value.TotalPortfolioValue);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_monthly_state()
        {
            // Arrange
            var url = "/api/MonthlyStates/Get/?id=131";

            // Act
            using var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_add_new_monthly_state()
        {
            // Arrange
            var url = "/api/MonthlyStates/Save/";
            var command = new SaveMonthlyStateCommand
            {
                Id = 0,
                StateDate = new DateTime(2024, 3, 31),
                UninvestedCash = 2000m,
                Deposits = 4000m,
                Withdrawals = 0m,
                TotalPortfolioValue = 50000m
            };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveMonthlyStateCommand>(url, command);
            var stateFromDb = await DbContext.MonthlyStates
                .AsNoTracking()
                .Where(state => state.StateDate == command.StateDate)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(stateFromDb);
            Assert.Equal(50000m, stateFromDb.TotalPortfolioValue);
        }

        [Fact]
        public async Task Save_should_update_existing_monthly_state()
        {
            // Arrange
            var url = "/api/MonthlyStates/Save/";
            var state = await SeedMonthlyStateAsync();

            var command = new SaveMonthlyStateCommand
            {
                Id = state.Id,
                StateDate = state.StateDate,
                UninvestedCash = 5000m,
                Deposits = 5000m,
                Withdrawals = 500m,
                TotalPortfolioValue = 60000m
            };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveMonthlyStateCommand>(url, command);
            // AsNoTracking - loeme vaartused andmebaasist, mitte EF-i puhvrist
            var stateFromDb = await DbContext.MonthlyStates
                .AsNoTracking()
                .Where(item => item.Id == state.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(stateFromDb);
            Assert.Equal(60000m, stateFromDb.TotalPortfolioValue);
        }

        [Fact]
        public async Task Save_should_work_with_missing_monthly_state()
        {
            // Arrange
            var url = "/api/MonthlyStates/Save/";
            var command = new SaveMonthlyStateCommand
            {
                Id = 10,
                StateDate = new DateTime(2024, 1, 31),
                TotalPortfolioValue = 45000m
            };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveMonthlyStateCommand>(url, command);
            var stateFromDb = await DbContext.MonthlyStates
                .AsNoTracking()
                .Where(state => state.Id == 10)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(stateFromDb);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_monthly_state()
        {
            // Arrange
            var url = "/api/MonthlyStates/Save/";
            var command = new SaveMonthlyStateCommand
            {
                Id = 0,
                StateDate = default(DateTime),
                TotalPortfolioValue = -1m
            };

            // Act
            using var response = await Client.PostAsJsonAsync<SaveMonthlyStateCommand>(url, command);
            var stateFromDb = await DbContext.MonthlyStates
                .AsNoTracking()
                .Where(state => state.Id == 1)
                .FirstOrDefaultAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Null(stateFromDb);
        }

        [Fact]
        public async Task Delete_should_remove_existing_monthly_state()
        {
            // Arrange
            var url = "/api/MonthlyStates/Delete/";
            var state = await SeedMonthlyStateAsync();

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = state.Id })
            };
            using var response = await Client.SendAsync(request);
            var stateFromDb = await DbContext.MonthlyStates
                .AsNoTracking()
                .Where(item => item.Id == state.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(stateFromDb);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_monthly_state()
        {
            // Arrange
            var url = "/api/MonthlyStates/Delete/";

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
