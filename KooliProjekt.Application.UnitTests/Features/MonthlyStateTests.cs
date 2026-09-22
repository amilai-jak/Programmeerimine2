using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.MonthlyStates;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    // 16.01.2026 - uks testide klass uhe feature'i kohta
    public class MonthlyStateTests : TestBase
    {
        private async Task SeedMonthlyStateAsync(DateTime? stateDate = null)
        {
            await DbContext.MonthlyStates.AddAsync(new MonthlyState
            {
                StateDate = stateDate ?? new DateTime(2024, 1, 31),
                UninvestedCash = 1000m,
                Deposits = 5000m,
                Withdrawals = 500m,
                TotalPortfolioValue = 45000m
            });
            await DbContext.SaveChangesAsync();
        }

        private async Task SeedManyMonthlyStatesAsync(int count)
        {
            foreach (var i in Enumerable.Range(1, count))
            {
                await DbContext.MonthlyStates.AddAsync(new MonthlyState
                {
                    StateDate = new DateTime(2024, 1, 31).AddMonths(i - 1),
                    UninvestedCash = i * 100m,
                    Deposits = i * 1000m,
                    Withdrawals = 0m,
                    TotalPortfolioValue = 40000m + (i * 1000m)
                });
            }

            await DbContext.SaveChangesAsync();
        }

        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetMonthlyStateQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetMonthlyStateQuery)null;
            var handler = new GetMonthlyStateQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_request_id_is_zero_or_negative(int id)
        {
            // Arrange
            var query = new GetMonthlyStateQuery { Id = id };
            var handler = new GetMonthlyStateQueryHandler(GetFaultyDbContext());

            await SeedMonthlyStateAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_monthly_state()
        {
            // Arrange
            var query = new GetMonthlyStateQuery { Id = 1 };
            var handler = new GetMonthlyStateQueryHandler(DbContext);

            await SeedMonthlyStateAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, result.Value.Id);
            Assert.Equal(new DateTime(2024, 1, 31), result.Value.StateDate);
            Assert.Equal(45000m, result.Value.TotalPortfolioValue);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_monthly_state_does_not_exist(int id)
        {
            // Arrange
            var query = new GetMonthlyStateQuery { Id = id };
            var handler = new GetMonthlyStateQueryHandler(DbContext);

            await SeedMonthlyStateAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListMonthlyStatesQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListMonthlyStatesQuery)null;
            var handler = new ListMonthlyStatesQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 10)]
        public async Task List_should_throw_when_page_is_zero_or_negative(int page, int pageSize)
        {
            // Arrange
            // Vigane kontekst toestab, et andmebaasi poole ei poordutud
            var query = new ListMonthlyStatesQuery { Page = page, PageSize = pageSize };
            var handler = new ListMonthlyStatesQueryHandler(GetFaultyDbContext());

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
            Assert.Equal("Page", ex.ParamName);
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(10, -5)]
        public async Task List_should_throw_when_page_size_is_zero_or_negative(int page, int pageSize)
        {
            // Arrange
            var query = new ListMonthlyStatesQuery { Page = page, PageSize = pageSize };
            var handler = new ListMonthlyStatesQueryHandler(GetFaultyDbContext());

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
            Assert.Equal("PageSize", ex.ParamName);
        }

        [Fact]
        public async Task List_should_throw_when_page_size_is_bigger_than_max_page_size()
        {
            // Arrange
            var query = new ListMonthlyStatesQuery { Page = 1, PageSize = ListMonthlyStatesQuery.MaxPageSize + 1 };
            var handler = new ListMonthlyStatesQueryHandler(GetFaultyDbContext());

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
            Assert.Equal("PageSize", ex.ParamName);
        }

        [Fact]
        public async Task List_should_return_page_of_monthly_states()
        {
            // Arrange
            var query = new ListMonthlyStatesQuery { Page = 2, PageSize = 5 };
            var handler = new ListMonthlyStatesQueryHandler(DbContext);

            await SeedManyMonthlyStatesAsync(15);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Page, result.Value.CurrentPage);
            Assert.Equal(query.PageSize, result.Value.Results.Count);
            Assert.Equal(15, result.Value.RowCount);
            Assert.Equal(3, result.Value.PageCount);
        }

        [Fact]
        public async Task List_should_return_empty_result_if_monthly_states_doesnt_exist()
        {
            // Arrange
            var query = new ListMonthlyStatesQuery { Page = 1, PageSize = 5 };
            var handler = new ListMonthlyStatesQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_find_monthly_states_by_state_date_from()
        {
            // Arrange
            var query = new ListMonthlyStatesQuery
            {
                Page = 1,
                PageSize = 20,
                StateDateFrom = new DateTime(2024, 12, 1)
            };
            var handler = new ListMonthlyStatesQueryHandler(DbContext);

            await SeedManyMonthlyStatesAsync(15);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(4, result.Value.Results.Count);
        }

        [Fact]
        public async Task List_should_find_monthly_states_by_state_date_to()
        {
            // Arrange
            var query = new ListMonthlyStatesQuery
            {
                Page = 1,
                PageSize = 20,
                StateDateTo = new DateTime(2024, 2, 1)
            };
            var handler = new ListMonthlyStatesQueryHandler(DbContext);

            await SeedManyMonthlyStatesAsync(15);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
        }

        [Fact]
        public async Task List_should_find_monthly_states_by_min_total_portfolio_value()
        {
            // Arrange
            var query = new ListMonthlyStatesQuery
            {
                Page = 1,
                PageSize = 20,
                MinTotalPortfolioValue = 54000m
            };
            var handler = new ListMonthlyStatesQueryHandler(DbContext);

            await SeedManyMonthlyStatesAsync(15);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Results.Count);
            Assert.All(result.Value.Results, state => Assert.True(state.TotalPortfolioValue >= 54000m));
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteMonthlyStateCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteMonthlyStateCommand)null;
            var handler = new DeleteMonthlyStateCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Delete_should_not_use_dbcontext_if_id_is_zero_or_less(int id)
        {
            // Arrange
            var command = new DeleteMonthlyStateCommand { Id = id };
            var handler = new DeleteMonthlyStateCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_monthly_state()
        {
            // Arrange
            var command = new DeleteMonthlyStateCommand { Id = 1 };
            var handler = new DeleteMonthlyStateCommandHandler(DbContext);

            await SeedMonthlyStateAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.MonthlyStates.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_monthly_state()
        {
            // Arrange
            var command = new DeleteMonthlyStateCommand { Id = 1034 };
            var handler = new DeleteMonthlyStateCommandHandler(DbContext);

            await SeedMonthlyStateAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveMonthlyStateCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveMonthlyStateCommand)null;
            var handler = new SaveMonthlyStateCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_return_if_id_is_negative()
        {
            // Arrange
            var request = new SaveMonthlyStateCommand { Id = -10 };
            var handler = new SaveMonthlyStateCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_monthly_state()
        {
            // Arrange
            var request = new SaveMonthlyStateCommand
            {
                Id = 0,
                StateDate = new DateTime(2024, 1, 31),
                UninvestedCash = 1000m,
                Deposits = 5000m,
                Withdrawals = 500m,
                TotalPortfolioValue = 45000m
            };
            var handler = new SaveMonthlyStateCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.MonthlyStates.SingleOrDefaultAsync(state => state.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
            Assert.Equal(request.TotalPortfolioValue, saved.TotalPortfolioValue);
        }

        [Fact]
        public async Task Save_should_update_existing_monthly_state()
        {
            // Arrange
            var request = new SaveMonthlyStateCommand
            {
                Id = 1,
                StateDate = new DateTime(2024, 2, 29),
                UninvestedCash = 2000m,
                Deposits = 6000m,
                Withdrawals = 0m,
                TotalPortfolioValue = 50000m
            };
            var handler = new SaveMonthlyStateCommandHandler(DbContext);

            await SeedMonthlyStateAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.MonthlyStates.SingleOrDefaultAsync(state => state.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.StateDate, saved.StateDate);
            Assert.Equal(request.TotalPortfolioValue, saved.TotalPortfolioValue);
        }

        [Fact]
        public async Task Save_should_not_update_missing_monthly_state()
        {
            // Arrange
            var request = new SaveMonthlyStateCommand { Id = 20, StateDate = new DateTime(2024, 1, 31) };
            var handler = new SaveMonthlyStateCommandHandler(DbContext);

            await SeedMonthlyStateAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public void SaveValidator_should_return_false_when_state_date_is_missing()
        {
            // Arrange
            var command = new SaveMonthlyStateCommand { Id = 0, StateDate = default(DateTime) };
            var validator = new SaveMonthlyStateCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveMonthlyStateCommand.StateDate), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_false_when_state_date_is_not_unique()
        {
            // Arrange
            var command = new SaveMonthlyStateCommand { Id = 0, StateDate = new DateTime(2024, 1, 31) };
            var validator = new SaveMonthlyStateCommandValidator(DbContext);

            await SeedMonthlyStateAsync();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveMonthlyStateCommand.StateDate), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_true_when_updating_existing_state_with_same_date()
        {
            // Arrange
            var command = new SaveMonthlyStateCommand { Id = 1, StateDate = new DateTime(2024, 1, 31) };
            var validator = new SaveMonthlyStateCommandValidator(DbContext);

            await SeedMonthlyStateAsync();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_uninvested_cash_is_negative(decimal value)
        {
            // Arrange
            var command = new SaveMonthlyStateCommand { Id = 0, StateDate = new DateTime(2024, 1, 31), UninvestedCash = value };
            var validator = new SaveMonthlyStateCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveMonthlyStateCommand.UninvestedCash), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_deposits_is_negative(decimal value)
        {
            // Arrange
            var command = new SaveMonthlyStateCommand { Id = 0, StateDate = new DateTime(2024, 1, 31), Deposits = value };
            var validator = new SaveMonthlyStateCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveMonthlyStateCommand.Deposits), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_withdrawals_is_negative(decimal value)
        {
            // Arrange
            var command = new SaveMonthlyStateCommand { Id = 0, StateDate = new DateTime(2024, 1, 31), Withdrawals = value };
            var validator = new SaveMonthlyStateCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveMonthlyStateCommand.Withdrawals), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_total_portfolio_value_is_negative(decimal value)
        {
            // Arrange
            var command = new SaveMonthlyStateCommand { Id = 0, StateDate = new DateTime(2024, 1, 31), TotalPortfolioValue = value };
            var validator = new SaveMonthlyStateCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveMonthlyStateCommand.TotalPortfolioValue), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_command_is_valid()
        {
            // Arrange
            var command = new SaveMonthlyStateCommand
            {
                Id = 0,
                StateDate = new DateTime(2024, 6, 30),
                UninvestedCash = 1000m,
                Deposits = 5000m,
                Withdrawals = 500m,
                TotalPortfolioValue = 45000m
            };
            var validator = new SaveMonthlyStateCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
