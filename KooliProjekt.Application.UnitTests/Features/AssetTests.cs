using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Assets;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    // 16.01.2026 - uks testide klass uhe feature'i kohta
    public class AssetTests : TestBase
    {
        private async Task SeedAssetClassAsync(string name = "Aktsiafondid")
        {
            await DbContext.AssetClasses.AddAsync(new AssetClass { Name = name });
            await DbContext.SaveChangesAsync();
        }

        private async Task SeedAssetAsync(string name = "Vanguard S&P 500", string ticker = "VOO")
        {
            await SeedAssetClassAsync();

            await DbContext.Assets.AddAsync(new Asset
            {
                AssetClassID = 1,
                Name = name,
                Ticker = ticker,
                IsRealEstate = false
            });
            await DbContext.SaveChangesAsync();
        }

        private async Task SeedManyAssetsAsync(int count)
        {
            await SeedAssetClassAsync();

            foreach (var i in Enumerable.Range(1, count))
            {
                await DbContext.Assets.AddAsync(new Asset
                {
                    AssetClassID = 1,
                    Name = "Vara " + i.ToString("00"),
                    Ticker = "T" + i,
                    IsRealEstate = i % 2 == 0
                });
            }

            await DbContext.SaveChangesAsync();
        }

        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetAssetQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetAssetQuery)null;
            var handler = new GetAssetQueryHandler(DbContext);

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
            var query = new GetAssetQuery { Id = id };
            var handler = new GetAssetQueryHandler(GetFaultyDbContext());

            await SeedAssetAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_asset()
        {
            // Arrange
            var query = new GetAssetQuery { Id = 1 };
            var handler = new GetAssetQueryHandler(DbContext);

            await SeedAssetAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, result.Value.Id);
            Assert.Equal("Vanguard S&P 500", result.Value.Name);
            Assert.Equal("VOO", result.Value.Ticker);
            Assert.Equal("Aktsiafondid", result.Value.AssetClassName);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_asset_does_not_exist(int id)
        {
            // Arrange
            var query = new GetAssetQuery { Id = id };
            var handler = new GetAssetQueryHandler(DbContext);

            await SeedAssetAsync();

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
                new ListAssetsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListAssetsQuery)null;
            var handler = new ListAssetsQueryHandler(DbContext);

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
            var query = new ListAssetsQuery { Page = page, PageSize = pageSize };
            var handler = new ListAssetsQueryHandler(GetFaultyDbContext());

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
            var query = new ListAssetsQuery { Page = page, PageSize = pageSize };
            var handler = new ListAssetsQueryHandler(GetFaultyDbContext());

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
            var query = new ListAssetsQuery { Page = 1, PageSize = ListAssetsQuery.MaxPageSize + 1 };
            var handler = new ListAssetsQueryHandler(GetFaultyDbContext());

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
            Assert.Equal("PageSize", ex.ParamName);
        }

        [Fact]
        public async Task List_should_return_page_of_assets()
        {
            // Arrange
            var query = new ListAssetsQuery { Page = 1, PageSize = 5 };
            var handler = new ListAssetsQueryHandler(DbContext);

            await SeedManyAssetsAsync(15);

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
        public async Task List_should_return_empty_result_if_assets_doesnt_exist()
        {
            // Arrange
            var query = new ListAssetsQuery { Page = 1, PageSize = 5 };
            var handler = new ListAssetsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_find_assets_by_name()
        {
            // Arrange
            var query = new ListAssetsQuery { Page = 1, PageSize = 10, Name = "Vanguard" };
            var handler = new ListAssetsQueryHandler(DbContext);

            await SeedManyAssetsAsync(5);
            await DbContext.Assets.AddAsync(new Asset { AssetClassID = 1, Name = "Vanguard Total Bond", Ticker = "BND" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Vanguard Total Bond", result.Value.Results.First().Name);
        }

        [Fact]
        public async Task List_should_find_assets_by_ticker()
        {
            // Arrange
            var query = new ListAssetsQuery { Page = 1, PageSize = 10, Ticker = "T3" };
            var handler = new ListAssetsQueryHandler(DbContext);

            await SeedManyAssetsAsync(5);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("T3", result.Value.Results.First().Ticker);
        }

        [Fact]
        public async Task List_should_find_assets_by_asset_class()
        {
            // Arrange
            var query = new ListAssetsQuery { Page = 1, PageSize = 10, AssetClassID = 2 };
            var handler = new ListAssetsQueryHandler(DbContext);

            await SeedAssetClassAsync("Aktsiafondid");
            await DbContext.AssetClasses.AddAsync(new AssetClass { Name = "Volakirjafondid" });
            await DbContext.Assets.AddAsync(new Asset { AssetClassID = 1, Name = "Vanguard S&P 500", Ticker = "VOO" });
            await DbContext.Assets.AddAsync(new Asset { AssetClassID = 2, Name = "iShares Core Bond", Ticker = "AGG" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("iShares Core Bond", result.Value.Results.First().Name);
        }

        [Fact]
        public async Task List_should_find_assets_by_is_real_estate()
        {
            // Arrange
            var query = new ListAssetsQuery { Page = 1, PageSize = 10, IsRealEstate = true };
            var handler = new ListAssetsQueryHandler(DbContext);

            await SeedManyAssetsAsync(5);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Results.Count);
            Assert.All(result.Value.Results, asset => Assert.True(asset.IsRealEstate));
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteAssetCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteAssetCommand)null;
            var handler = new DeleteAssetCommandHandler(DbContext);

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
            var command = new DeleteAssetCommand { Id = id };
            var handler = new DeleteAssetCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_asset()
        {
            // Arrange
            var command = new DeleteAssetCommand { Id = 1 };
            var handler = new DeleteAssetCommandHandler(DbContext);

            await SeedAssetAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.Assets.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_asset()
        {
            // Arrange
            var command = new DeleteAssetCommand { Id = 1034 };
            var handler = new DeleteAssetCommandHandler(DbContext);

            await SeedAssetAsync();

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
                new SaveAssetCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveAssetCommand)null;
            var handler = new SaveAssetCommandHandler(DbContext);

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
            var request = new SaveAssetCommand { Id = -10, Name = "New asset" };
            var handler = new SaveAssetCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_asset()
        {
            // Arrange
            var request = new SaveAssetCommand
            {
                Id = 0,
                AssetClassID = 1,
                Name = "New asset",
                Ticker = "NEW",
                IsRealEstate = false
            };
            var handler = new SaveAssetCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedAsset = await DbContext.Assets.SingleOrDefaultAsync(asset => asset.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedAsset);
            Assert.Equal(1, savedAsset.Id);
            Assert.Equal(request.Name, savedAsset.Name);
            Assert.Equal(request.Ticker, savedAsset.Ticker);
        }

        [Fact]
        public async Task Save_should_update_existing_asset()
        {
            // Arrange
            var request = new SaveAssetCommand
            {
                Id = 1,
                AssetClassID = 1,
                Name = "Updated asset",
                Ticker = "UPD",
                IsRealEstate = true
            };
            var handler = new SaveAssetCommandHandler(DbContext);

            await SeedAssetAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedAsset = await DbContext.Assets.SingleOrDefaultAsync(asset => asset.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedAsset);
            Assert.Equal(request.Name, savedAsset.Name);
            Assert.Equal(request.Ticker, savedAsset.Ticker);
            Assert.True(savedAsset.IsRealEstate);
        }

        [Fact]
        public async Task Save_should_not_update_missing_asset()
        {
            // Arrange
            var request = new SaveAssetCommand { Id = 20, AssetClassID = 1, Name = "Updated asset" };
            var handler = new SaveAssetCommandHandler(DbContext);

            await SeedAssetAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SaveValidator_should_return_false_when_name_is_missing(string name)
        {
            // Arrange
            var command = new SaveAssetCommand { Id = 0, AssetClassID = 1, Name = name };
            var validator = new SaveAssetCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAssetCommand.Name), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_false_when_name_is_too_long()
        {
            // Arrange
            var command = new SaveAssetCommand { Id = 0, AssetClassID = 1, Name = new string('a', 101) };
            var validator = new SaveAssetCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAssetCommand.Name), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_false_when_ticker_is_too_long()
        {
            // Arrange
            var command = new SaveAssetCommand { Id = 0, AssetClassID = 1, Name = "Vara", Ticker = new string('a', 11) };
            var validator = new SaveAssetCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAssetCommand.Ticker), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void SaveValidator_should_return_false_when_asset_class_id_is_not_positive(int assetClassId)
        {
            // Arrange
            var command = new SaveAssetCommand { Id = 0, AssetClassID = assetClassId, Name = "Vara" };
            var validator = new SaveAssetCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAssetCommand.AssetClassID), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_false_when_asset_class_does_not_exist()
        {
            // Arrange
            var command = new SaveAssetCommand { Id = 0, AssetClassID = 42, Name = "Vara" };
            var validator = new SaveAssetCommandValidator(DbContext);

            await SeedAssetClassAsync();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAssetCommand.AssetClassID), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_true_when_command_is_valid()
        {
            // Arrange
            var command = new SaveAssetCommand
            {
                Id = 0,
                AssetClassID = 1,
                Name = "Vanguard S&P 500",
                Ticker = "VOO",
                IsRealEstate = false
            };
            var validator = new SaveAssetCommandValidator(DbContext);

            await SeedAssetClassAsync();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
