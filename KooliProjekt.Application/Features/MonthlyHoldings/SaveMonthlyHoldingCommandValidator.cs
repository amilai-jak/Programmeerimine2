using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.MonthlyHoldings
{
    // 15.11.2025
    // Valideerimise klass SaveMonthlyHoldingCommand kasu jaoks
    // Voetakse programmi poolt kulge automaatselt
    public class SaveMonthlyHoldingCommandValidator : AbstractValidator<SaveMonthlyHoldingCommand>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveMonthlyHoldingCommandValidator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(command => command.StateID)
                .GreaterThan(0).WithMessage("Oleku ID on kohustuslik")
                // Oma loogikaga valideerimise reegel - olek peab andmebaasis olemas olema
                .Custom((stateId, context) =>
                {
                    if (stateId <= 0)
                    {
                        return;
                    }

                    if (!_dbContext.MonthlyStates.Any(state => state.Id == stateId))
                    {
                        context.AddFailure(nameof(SaveMonthlyHoldingCommand.StateID), "Olekut ei leitud");
                    }
                });

            RuleFor(command => command.AssetID)
                .GreaterThan(0).WithMessage("Vara ID on kohustuslik")
                // Oma loogikaga valideerimise reegel - vara peab andmebaasis olemas olema
                .Custom((assetId, context) =>
                {
                    if (assetId <= 0)
                    {
                        return;
                    }

                    if (!_dbContext.Assets.Any(asset => asset.Id == assetId))
                    {
                        context.AddFailure(nameof(SaveMonthlyHoldingCommand.AssetID), "Vara ei leitud");
                    }
                });

            RuleFor(command => command.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Kogus ei saa olla negatiivne");

            RuleFor(command => command.Value)
                .GreaterThanOrEqualTo(0).WithMessage("Vaartus ei saa olla negatiivne");
        }
    }
}
