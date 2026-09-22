using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Assets
{
    // 15.11.2025
    // Valideerimise klass SaveAssetCommand kasu jaoks
    // Voetakse programmi poolt kulge automaatselt
    public class SaveAssetCommandValidator : AbstractValidator<SaveAssetCommand>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveAssetCommandValidator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Vara nimi on kohustuslik")
                .MaximumLength(100).WithMessage("Nimi peab olema 1-100 marki");

            RuleFor(command => command.Ticker)
                .MaximumLength(10).WithMessage("Ticker voib olla maksimaalselt 10 marki");

            RuleFor(command => command.AssetClassID)
                .GreaterThan(0).WithMessage("Vara klassi ID on kohustuslik")
                // Oma loogikaga valideerimise reegel - vara klass peab andmebaasis olemas olema
                .Custom((assetClassId, context) =>
                {
                    if (assetClassId <= 0)
                    {
                        return;
                    }

                    if (!_dbContext.AssetClasses.Any(assetClass => assetClass.Id == assetClassId))
                    {
                        context.AddFailure(nameof(SaveAssetCommand.AssetClassID), "Vara klassi ei leitud");
                    }
                });
        }
    }
}
