using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.AssetClasses;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    // 16.01.2026 - uks testide klass uhe feature'i kohta
    public class AssetClassTests : TestBase
    {
        private async Task SeedAssetClassAsync(string name = "Aktsiafondid")
        {
            await DbContext.AssetClasses.AddAsync(new AssetClass { Name = name });
            await DbContext.SaveChangesAsync();
        }

        private async Task SeedManyAssetClassesAsync(int count)
        {
            foreach (var i in Enumerable.Range(1, count))
            {
                await DbContext.AssetClasses.AddAsync(new AssetClass { Name = "Klass " + i.ToString("00") });
            }

            await DbContext.SaveChangesAsync();
        }

        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetAssetClassQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetAssetClassQuery)null;
            var handler = new GetAssetClassQueryHandler(DbContext);

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
            var query = new GetAssetClassQuery { Id = id };
            var handler = new GetAssetClassQueryHandler(GetFaultyDbContext());

            await SeedAssetClassAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_asset_class()
        {
            // Arrange
            var query = new GetAssetClassQuery { Id = 1 };
            var handler = new GetAssetClassQueryHandler(DbContext);

            await SeedAssetClassAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, result.Value.Id);
            Assert.Equal("Aktsiafondid", result.Value.Name);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_asset_class_does_not_exist(int id)
        {
            // Arrange
            var query = new GetAssetClassQuery { Id = id };
            var handler = new GetAssetClassQueryHandler(DbContext);

            await SeedAssetClassAsync();

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
                new ListAssetClassesQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListAssetClassesQuery)null;
            var handler = new ListAssetClassesQueryHandler(DbContext);

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
            var query = new ListAssetClassesQuery { Page = page, PageSize = pageSize };
            var handler = new ListAssetClassesQueryHandler(GetFaultyDbContext());

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
            var query = new ListAssetClassesQuery { Page = page, PageSize = pageSize };
            var handler = new ListAssetClassesQueryHandler(GetFaultyDbContext());

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
            var query = new ListAssetClassesQuery { Page = 1, PageSize = ListAssetClassesQuery.MaxPageSize + 1 };
            var handler = new ListAssetClassesQueryHandler(GetFaultyDbContext());

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await handler.Handle(query, CancellationToken.None);
            });
            Assert.Equal("PageSize", ex.ParamName);
        }

        [Fact]
        public async Task List_should_return_page_of_asset_classes()
        {
            // Arrange
            var query = new ListAssetClassesQuery { Page = 2, PageSize = 5 };
            var handler = new ListAssetClassesQueryHandler(DbContext);

            await SeedManyAssetClassesAsync(15);

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
        public async Task List_should_return_empty_result_if_asset_classes_doesnt_exist()
        {
            // Arrange
            var query = new ListAssetClassesQuery { Page = 1, PageSize = 5 };
            var handler = new ListAssetClassesQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_find_asset_classes_by_name()
        {
            // Arrange
            var query = new ListAssetClassesQuery { Page = 1, PageSize = 10, Name = "fond" };
            var handler = new ListAssetClassesQueryHandler(DbContext);

            await SeedManyAssetClassesAsync(5);
            await DbContext.AssetClasses.AddAsync(new AssetClass { Name = "Aktsiafondid" });
            await DbContext.AssetClasses.AddAsync(new AssetClass { Name = "Volakirjafondid" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Results.Count);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteAssetClassCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteAssetClassCommand)null;
            var handler = new DeleteAssetClassCommandHandler(DbContext);

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
            var command = new DeleteAssetClassCommand { Id = id };
            var handler = new DeleteAssetClassCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_asset_class()
        {
            // Arrange
            var command = new DeleteAssetClassCommand { Id = 1 };
            var handler = new DeleteAssetClassCommandHandler(DbContext);

            await SeedAssetClassAsync();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var count = DbContext.AssetClasses.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_asset_class()
        {
            // Arrange
            var command = new DeleteAssetClassCommand { Id = 1034 };
            var handler = new DeleteAssetClassCommandHandler(DbContext);

            await SeedAssetClassAsync();

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
                new SaveAssetClassCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveAssetClassCommand)null;
            var handler = new SaveAssetClassCommandHandler(DbContext);

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
            var request = new SaveAssetClassCommand { Id = -10, Name = "New class" };
            var handler = new SaveAssetClassCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_asset_class()
        {
            // Arrange
            var request = new SaveAssetClassCommand { Id = 0, Name = "New class" };
            var handler = new SaveAssetClassCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.AssetClasses.SingleOrDefaultAsync(assetClass => assetClass.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.Id);
            Assert.Equal(request.Name, saved.Name);
        }

        [Fact]
        public async Task Save_should_update_existing_asset_class()
        {
            // Arrange
            var request = new SaveAssetClassCommand { Id = 1, Name = "Updated class" };
            var handler = new SaveAssetClassCommandHandler(DbContext);

            await SeedAssetClassAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var saved = await DbContext.AssetClasses.SingleOrDefaultAsync(assetClass => assetClass.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(saved);
            Assert.Equal(request.Name, saved.Name);
        }

        [Fact]
        public async Task Save_should_not_update_missing_asset_class()
        {
            // Arrange
            var request = new SaveAssetClassCommand { Id = 20, Name = "Updated class" };
            var handler = new SaveAssetClassCommandHandler(DbContext);

            await SeedAssetClassAsync();

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
            var command = new SaveAssetClassCommand { Id = 0, Name = name };
            var validator = new SaveAssetClassCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAssetClassCommand.Name), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_false_when_name_is_too_long()
        {
            // Arrange
            var command = new SaveAssetClassCommand { Id = 0, Name = new string('a', 101) };
            var validator = new SaveAssetClassCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAssetClassCommand.Name), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_false_when_name_is_not_unique()
        {
            // Arrange
            var command = new SaveAssetClassCommand { Id = 0, Name = "Aktsiafondid" };
            var validator = new SaveAssetClassCommandValidator(DbContext);

            await SeedAssetClassAsync();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAssetClassCommand.Name), result.Errors.First().PropertyName);
        }

        [Fact]
        public async Task SaveValidator_should_return_true_when_updating_existing_asset_class_with_same_name()
        {
            // Arrange
            var command = new SaveAssetClassCommand { Id = 1, Name = "Aktsiafondid" };
            var validator = new SaveAssetClassCommandValidator(DbContext);

            await SeedAssetClassAsync();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task SaveValidator_should_return_true_when_command_is_valid()
        {
            // Arrange
            var command = new SaveAssetClassCommand { Id = 0, Name = "Uus klass" };
            var validator = new SaveAssetClassCommandValidator(DbContext);

            await SeedAssetClassAsync();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
