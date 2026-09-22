using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.MonthlyHoldings;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    // 16.01.2026 - uks testide klass uhe feature'i kohta
    public class MonthlyHoldingTests : TestBase
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

        private async Task SeedHoldingAsync()
        {
            await SeedRelatedDataAsync();

            await DbContext.MonthlyHoldings.AddAsync(new MonthlyHolding
            {
                StateID = 1,
                AssetID = 1,
                Quantity = 10m,
                Value = 2500m
            });
            await DbContext.SaveChangesAsync();
        }

        private async Task SeedManyHoldingsAsync(int count)
        {
            await DbContext.AssetClasses.AddAsync(new AssetClass { Name = "Aktsiafondid" });
            await DbContext.SaveChangesAsync();

            foreach (var i in Enumerable.Range(1, count))
            {
                await DbContext.Assets.AddAsync(new Asset
                {
                    AssetClassID = 1,
                    Name = "Vara " + i.ToString("00"),
                    Ticker = "T" + i
                });
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

            foreach (var i in Enumerable.Range(1, count))
            {
                await DbContext.MonthlyHoldings.AddAsync(new MonthlyHolding
                {
                    StateID = i,
                    AssetID = i,
                    Quantity = i * 10m,
                    Value = i * 100m
                });
            }

            await DbContext.SaveChangesAsync();
        }

        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetMonthlyHoldingQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetMonthlyHoldingQuery)null;
            var handler = new GetMonthlyHoldingQueryHandler(DbContext);

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
            var query = new GetMonthlyHoldingQuery { Id = id };
            var handler = new GetMonthlyHoldingQueryHandler(GetFaultyDbContext());

            await SeedHoldingAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_holding()
        {
            // Arrange
            var query = new GetMonthlyHoldingQuery { Id = 1 };
            var handler = new GetMonthlyHoldingQueryHandler(DbContext);

            await SeedHoldingAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, result.Value.Id);
            Assert.Equal(10m, result.Value.Quantity);
            Assert.Equal(2500m, result.Value.Value);
            Assert.Equal("Vanguard S&P 500", result.Value.AssetName);
            Assert.Equal(new DateTime(2024, 1, 31), result.Value.StateDate);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_holding_does_not_exist(int id)
        {
            // Arrange
            var query = new GetMonthlyHoldingQuery { Id = id };
            var handler = new GetMonthlyHoldingQueryHandler(DbContext);

            await SeedHoldingAsync();

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
                new ListMonthlyHoldingsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListMonthlyHoldingsQuery)null;
            var handler = new ListMonthlyHoldingsQueryHandler(DbContext);

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
            var query = new ListMonthlyHoldingsQuery { Page = page, PageSize = pageSize };
            var handler = new ListMonthlyHoldingsQueryHandler(GetFaultyDbContext());

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
            var query = new ListMonthlyHoldingsQuery { Page = page, PageSize = pageSize };
            var handler = new ListMonthlyHoldingsQueryHandler(GetFaultyDbContext());

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
            var query = new ListMonthlyHoldingsQuery { Page = 1, PageSize = ListMonthlyHoldingsQuery.MaxPageSize + 1 };
            var handler = new ListMonthlyHoldingsQueryHandler(GetFaultyDbContext());

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
            Assert.Equal("PageSize", ex.ParamName);
        }

        [Fact]
        public async Task List_should_return_page_of_holdings()
        {
            // Arrange
            var query = new ListMonthlyHoldingsQuery { Page = 1, PageSize = 5 };
            var handler = new ListMonthlyHoldingsQueryHandler(DbContext);

            await SeedManyHoldingsAsync(15);

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
        public async Task List_should_return_empty_result_if_holdings_doesnt_exist()
        {
            // Arrange
            var query = new ListMonthlyHoldingsQuery { Page = 1, PageSize = 5 };
            var handler = new ListMonthlyHoldingsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_find_holdings_by_asset_name()
        {
            // Arrange
            var query = new ListMonthlyHoldingsQuery { Page = 1, PageSize = 10, AssetName = "Vara 03" };
            var handler = new ListMonthlyHoldingsQueryHandler(DbContext);

            await SeedManyHoldingsAsync(5);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Vara 03", result.Value.Results.First().AssetName);
        }

        [Fact]
        public async Task List_should_find_holdings_by_asset_id()
        {
            // Arrange
            var query = new ListMonthlyHoldingsQuery { Page = 1, PageSize = 10, AssetID = 2 };
            var handler = new ListMonthlyHoldingsQueryHandler(DbContext);

            await SeedManyHoldingsAsync(5);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal(2, result.Value.Results.First().AssetID);
        }

        [Fact]
        public async Task List_should_find_holdings_by_state_id()
        {
            // Arrange
            var query = new ListMonthlyHoldingsQuery { Page = 1, PageSize = 10, StateID = 4 };
            var handler = new ListMonthlyHoldingsQueryHandler(DbContext);

            await SeedManyHoldingsAsync(5);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal(4, result.Value.Results.First().StateID);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteMonthlyHoldingCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteMonthlyHoldingCommand)null;
            var handler = new DeleteMonthlyHoldingCommandHandler(DbContext);

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
            var command = new DeleteMonthlyHoldingCommand { Id = id };
            var handler = new DeleteMonthlyHoldingCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_holding()
        {
            // Arrange
            var command = new DeleteMonthlyHoldingCommand { Id = 1 };
            var handler = new DeleteMonthlyHoldingCommandHandler(DbContext);

            await SeedHoldingAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.MonthlyHoldings.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_holding()
        {
            // Arrange
            var command = new DeleteMonthlyHoldingCommand { Id = 1034 };
            var handler = new DeleteMonthlyHoldingCommandHandler(DbContext);

            await SeedHoldingAsync();

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
                new SaveMonthlyHoldingCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveMonthlyHoldingCommand)null;
            var handler = new SaveMonthlyHoldingCommandHandler(DbContext);

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
            var request = new SaveMonthlyHoldingCommand { Id = -10 };
            var handler = new SaveMonthlyHoldingCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_holding()
        {
            // Arrange
            var request = new SaveMonthlyHoldingCommand
            {
                Id = 0,
                StateID = 1,
                AssetID = 1,
                Quantity = 10m,
                Value = 2500m
            };
            var handler = new SaveMonthlyHoldingCommandHandler(DbContext);

            await SeedRelatedDataAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.MonthlyHoldings.SingleOrDefaultAsync(holding => holding.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
            Assert.Equal(request.Quantity, saved.Quantity);
        }

        [Fact]
        public async Task Save_should_update_existing_holding()
        {
            // Arrange
            var request = new SaveMonthlyHoldingCommand
            {
                Id = 1,
                StateID = 1,
                AssetID = 1,
                Quantity = 25m,
                Value = 6000m
            };
            var handler = new SaveMonthlyHoldingCommandHandler(DbContext);

            await SeedHoldingAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.MonthlyHoldings.SingleOrDefaultAsync(holding => holding.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.Quantity, saved.Quantity);
            Assert.Equal(request.Value, saved.Value);
        }

        [Fact]
        public async Task Save_should_not_update_missing_holding()
        {
            // Arrange
            var request = new SaveMonthlyHoldingCommand { Id = 20, StateID = 1, AssetID = 1 };
            var handler = new SaveMonthlyHoldingCommandHandler(DbContext);

            await SeedHoldingAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_state_id_is_not_positive(int stateId)
        {
            // Arrange
            var command = new SaveMonthlyHoldingCommand { Id = 0, StateID = stateId, AssetID = 1 };
            var validator = new SaveMonthlyHoldingCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == nameof(SaveMonthlyHoldingCommand.StateID));
        }

        [Fact]
        public async Task SaveValidator_should_return_false_when_state_does_not_exist()
        {
            // Arrange
            var command = new SaveMonthlyHoldingCommand { Id = 0, StateID = 42, AssetID = 1 };
            var validator = new SaveMonthlyHoldingCommandValidator(DbContext);

            await SeedHoldingAsync();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == nameof(SaveMonthlyHoldingCommand.StateID));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_asset_id_is_not_positive(int assetId)
        {
            // Arrange
            var command = new SaveMonthlyHoldingCommand { Id = 0, StateID = 1, AssetID = assetId };
            var validator = new SaveMonthlyHoldingCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == nameof(SaveMonthlyHoldingCommand.AssetID));
        }

        [Fact]
        public async Task SaveValidator_should_return_false_when_asset_does_not_exist()
        {
            // Arrange
            var command = new SaveMonthlyHoldingCommand { Id = 0, StateID = 1, AssetID = 42 };
            var validator = new SaveMonthlyHoldingCommandValidator(DbContext);

            await SeedHoldingAsync();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == nameof(SaveMonthlyHoldingCommand.AssetID));
        }

        [Theory]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_quantity_is_negative(decimal quantity)
        {
            // Arrange
            var command = new SaveMonthlyHoldingCommand { Id = 0, StateID = 1, AssetID = 1, Quantity = quantity };
            var validator = new SaveMonthlyHoldingCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == nameof(SaveMonthlyHoldingCommand.Quantity));
        }

        [Theory]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_value_is_negative(decimal value)
        {
            // Arrange
            var command = new SaveMonthlyHoldingCommand { Id = 0, StateID = 1, AssetID = 1, Value = value };
            var validator = new SaveMonthlyHoldingCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == nameof(SaveMonthlyHoldingCommand.Value));
        }

        [Fact]
        public async Task SaveValidator_should_return_true_when_command_is_valid()
        {
            // Arrange
            var command = new SaveMonthlyHoldingCommand
            {
                Id = 0,
                StateID = 1,
                AssetID = 1,
                Quantity = 10m,
                Value = 2500m
            };
            var validator = new SaveMonthlyHoldingCommandValidator(DbContext);

            await SeedHoldingAsync();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
