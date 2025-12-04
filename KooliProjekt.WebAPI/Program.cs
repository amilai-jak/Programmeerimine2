using FluentValidation;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace KooliProjekt.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var applicationAssembly = typeof(ErrorHandlingBehavior<,>).Assembly;
            builder.Services.AddValidatorsFromAssembly(applicationAssembly);
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(applicationAssembly);
                config.AddOpenBehavior(typeof(ErrorHandlingBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(TransactionalBehavior<,>));
            });

            var app = builder.Build();

            SeedDatabase(app);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void SeedDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                if (context.AssetClasses.Any())
                {
                    return;
                }

                var assetClasses = new[]
                {
                    new AssetClass { Name = "Aktsiafondid" },
                    new AssetClass { Name = "Võlakirjafondid" },
                    new AssetClass { Name = "Kinnisvarainvesteeringud" },
                    new AssetClass { Name = "Rahaväärtusega väärtpaberid" },
                    new AssetClass { Name = "Krüptovaluutad" }
                };
                context.AssetClasses.AddRange(assetClasses);
                context.SaveChanges();

                var assets = new[]
                {
                    new Asset { AssetClassID = 1, Name = "Vanguard S&P 500", Ticker = "VOO", IsRealEstate = false },
                    new Asset { AssetClassID = 1, Name = "iShares MSCI ACWI", Ticker = "ACWX", IsRealEstate = false },
                    new Asset { AssetClassID = 1, Name = "Vanguard Total World", Ticker = "VT", IsRealEstate = false },
                    new Asset { AssetClassID = 2, Name = "iShares Core US Aggregate Bond", Ticker = "AGG", IsRealEstate = false },
                    new Asset { AssetClassID = 2, Name = "Vanguard Total Bond Market", Ticker = "BND", IsRealEstate = false },
                    new Asset { AssetClassID = 3, Name = "Real Estate Investment Trust", Ticker = "REIT", IsRealEstate = true },
                    new Asset { AssetClassID = 3, Name = "Nordic Properties Fund", Ticker = "NPF", IsRealEstate = true },
                    new Asset { AssetClassID = 4, Name = "Money Market Fund", Ticker = "MMF", IsRealEstate = false },
                    new Asset { AssetClassID = 5, Name = "Bitcoin", Ticker = "BTC", IsRealEstate = false },
                    new Asset { AssetClassID = 5, Name = "Ethereum", Ticker = "ETH", IsRealEstate = false }
                };
                context.Assets.AddRange(assets);
                context.SaveChanges();

                var monthlyStates = new[]
                {
                    new MonthlyState { StateDate = new DateTime(2024, 1, 31), UninvestedCash = 1000m, Deposits = 5000m, Withdrawals = 500m, TotalPortfolioValue = 45000m },
                    new MonthlyState { StateDate = new DateTime(2024, 2, 29), UninvestedCash = 1500m, Deposits = 3000m, Withdrawals = 1000m, TotalPortfolioValue = 47500m },
                    new MonthlyState { StateDate = new DateTime(2024, 3, 31), UninvestedCash = 2000m, Deposits = 4000m, Withdrawals = 0m, TotalPortfolioValue = 50000m },
                    new MonthlyState { StateDate = new DateTime(2024, 4, 30), UninvestedCash = 1000m, Deposits = 2000m, Withdrawals = 500m, TotalPortfolioValue = 51500m },
                    new MonthlyState { StateDate = new DateTime(2024, 5, 31), UninvestedCash = 2500m, Deposits = 5000m, Withdrawals = 1500m, TotalPortfolioValue = 54000m }
                };
                context.MonthlyStates.AddRange(monthlyStates);
                context.SaveChanges();

                var monthlyHoldings = new[]
                {
                    new MonthlyHolding { StateID = 1, AssetID = 1, Quantity = 10.5m, Value = 15000m },
                    new MonthlyHolding { StateID = 1, AssetID = 4, Quantity = 25m, Value = 12000m },
                    new MonthlyHolding { StateID = 1, AssetID = 6, Quantity = 15m, Value = 15000m },
                    new MonthlyHolding { StateID = 1, AssetID = 9, Quantity = 0.5m, Value = 3000m },
                    new MonthlyHolding { StateID = 2, AssetID = 1, Quantity = 12m, Value = 17000m },
                    new MonthlyHolding { StateID = 2, AssetID = 2, Quantity = 8m, Value = 8000m },
                    new MonthlyHolding { StateID = 2, AssetID = 4, Quantity = 30m, Value = 14500m },
                    new MonthlyHolding { StateID = 2, AssetID = 6, Quantity = 18m, Value = 18000m },
                    new MonthlyHolding { StateID = 3, AssetID = 1, Quantity = 15m, Value = 21000m },
                    new MonthlyHolding { StateID = 3, AssetID = 3, Quantity = 5m, Value = 12000m },
                    new MonthlyHolding { StateID = 3, AssetID = 4, Quantity = 35m, Value = 17000m },
                    new MonthlyHolding { StateID = 3, AssetID = 7, Quantity = 20m, Value = 20000m }
                };
                context.MonthlyHoldings.AddRange(monthlyHoldings);
                context.SaveChanges();
            }
        }
    }
}
