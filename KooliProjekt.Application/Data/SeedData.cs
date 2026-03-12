using System;
using System.Linq;

namespace KooliProjekt.Application.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;

        public SeedData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Generate()
        {
            if (_dbContext.AssetClasses.Any())
            {
                return;
            }

            var assetClasses = new[]
            {
                new AssetClass { Name = "Aktsiafondid" },
                new AssetClass { Name = "Võlakirjafondid" },
                new AssetClass { Name = "Kinnisvarainvesteeringud" },
                new AssetClass { Name = "Rahaväärtusega väärtpaberid" },
                new AssetClass { Name = "Krüptovaluutad" },
                new AssetClass { Name = "Toorained" },
                new AssetClass { Name = "Indeksfondid" },
                new AssetClass { Name = "Kasvuaktsiad" },
                new AssetClass { Name = "Dividendiaktsiad" },
                new AssetClass { Name = "Alternatiivfondid" }
            };
            _dbContext.AssetClasses.AddRange(assetClasses);
            _dbContext.SaveChanges();

            var assets = new[]
            {
                new Asset { AssetClassID = assetClasses[0].Id, Name = "Vanguard S&P 500", Ticker = "VOO", IsRealEstate = false },
                new Asset { AssetClassID = assetClasses[0].Id, Name = "iShares MSCI ACWI", Ticker = "ACWX", IsRealEstate = false },
                new Asset { AssetClassID = assetClasses[1].Id, Name = "iShares Core US Aggregate Bond", Ticker = "AGG", IsRealEstate = false },
                new Asset { AssetClassID = assetClasses[1].Id, Name = "Vanguard Total Bond Market", Ticker = "BND", IsRealEstate = false },
                new Asset { AssetClassID = assetClasses[2].Id, Name = "Real Estate Investment Trust", Ticker = "REIT", IsRealEstate = true },
                new Asset { AssetClassID = assetClasses[2].Id, Name = "Nordic Properties Fund", Ticker = "NPF", IsRealEstate = true },
                new Asset { AssetClassID = assetClasses[3].Id, Name = "Money Market Fund", Ticker = "MMF", IsRealEstate = false },
                new Asset { AssetClassID = assetClasses[4].Id, Name = "Bitcoin", Ticker = "BTC", IsRealEstate = false },
                new Asset { AssetClassID = assetClasses[4].Id, Name = "Ethereum", Ticker = "ETH", IsRealEstate = false },
                new Asset { AssetClassID = assetClasses[5].Id, Name = "Kullaindeks", Ticker = "GLD", IsRealEstate = false }
            };
            _dbContext.Assets.AddRange(assets);
            _dbContext.SaveChanges();

            var monthlyStates = new[]
            {
                new MonthlyState { StateDate = new DateTime(2024, 1, 31), UninvestedCash = 1000m, Deposits = 5000m, Withdrawals = 500m, TotalPortfolioValue = 45000m },
                new MonthlyState { StateDate = new DateTime(2024, 2, 29), UninvestedCash = 1500m, Deposits = 3000m, Withdrawals = 1000m, TotalPortfolioValue = 47500m },
                new MonthlyState { StateDate = new DateTime(2024, 3, 31), UninvestedCash = 2000m, Deposits = 4000m, Withdrawals = 0m, TotalPortfolioValue = 50000m },
                new MonthlyState { StateDate = new DateTime(2024, 4, 30), UninvestedCash = 1000m, Deposits = 2000m, Withdrawals = 500m, TotalPortfolioValue = 51500m },
                new MonthlyState { StateDate = new DateTime(2024, 5, 31), UninvestedCash = 2500m, Deposits = 5000m, Withdrawals = 1500m, TotalPortfolioValue = 54000m },
                new MonthlyState { StateDate = new DateTime(2024, 6, 30), UninvestedCash = 1800m, Deposits = 3500m, Withdrawals = 200m, TotalPortfolioValue = 56000m },
                new MonthlyState { StateDate = new DateTime(2024, 7, 31), UninvestedCash = 900m, Deposits = 4500m, Withdrawals = 1000m, TotalPortfolioValue = 58000m },
                new MonthlyState { StateDate = new DateTime(2024, 8, 31), UninvestedCash = 1200m, Deposits = 2500m, Withdrawals = 300m, TotalPortfolioValue = 60000m },
                new MonthlyState { StateDate = new DateTime(2024, 9, 30), UninvestedCash = 3000m, Deposits = 6000m, Withdrawals = 2000m, TotalPortfolioValue = 62000m },
                new MonthlyState { StateDate = new DateTime(2024, 10, 31), UninvestedCash = 1500m, Deposits = 4000m, Withdrawals = 500m, TotalPortfolioValue = 65000m }
            };
            _dbContext.MonthlyStates.AddRange(monthlyStates);
            _dbContext.SaveChanges();

            var monthlyHoldings = new[]
            {
                new MonthlyHolding { StateID = monthlyStates[0].Id, AssetID = assets[0].Id, Quantity = 10.5m, Value = 15000m },
                new MonthlyHolding { StateID = monthlyStates[0].Id, AssetID = assets[2].Id, Quantity = 25m, Value = 12000m },
                new MonthlyHolding { StateID = monthlyStates[0].Id, AssetID = assets[4].Id, Quantity = 15m, Value = 15000m },
                new MonthlyHolding { StateID = monthlyStates[1].Id, AssetID = assets[0].Id, Quantity = 12m, Value = 17000m },
                new MonthlyHolding { StateID = monthlyStates[1].Id, AssetID = assets[1].Id, Quantity = 8m, Value = 8000m },
                new MonthlyHolding { StateID = monthlyStates[1].Id, AssetID = assets[3].Id, Quantity = 30m, Value = 14500m },
                new MonthlyHolding { StateID = monthlyStates[2].Id, AssetID = assets[0].Id, Quantity = 15m, Value = 21000m },
                new MonthlyHolding { StateID = monthlyStates[2].Id, AssetID = assets[5].Id, Quantity = 20m, Value = 20000m },
                new MonthlyHolding { StateID = monthlyStates[3].Id, AssetID = assets[7].Id, Quantity = 0.5m, Value = 3000m },
                new MonthlyHolding { StateID = monthlyStates[3].Id, AssetID = assets[6].Id, Quantity = 100m, Value = 10000m },
                new MonthlyHolding { StateID = monthlyStates[4].Id, AssetID = assets[8].Id, Quantity = 5m, Value = 8000m },
                new MonthlyHolding { StateID = monthlyStates[4].Id, AssetID = assets[9].Id, Quantity = 10m, Value = 5000m }
            };
            _dbContext.MonthlyHoldings.AddRange(monthlyHoldings);
            _dbContext.SaveChanges();
        }
    }
}
