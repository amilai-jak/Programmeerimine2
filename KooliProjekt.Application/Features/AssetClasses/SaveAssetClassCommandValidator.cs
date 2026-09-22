using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.AssetClasses
{
    // 15.11.2025
    // Valideerimise klass SaveAssetClassCommand kasu jaoks
    // Voetakse programmi poolt kulge automaatselt
    public class SaveAssetClassCommandValidator : AbstractValidator<SaveAssetClassCommand>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveAssetClassCommandValidator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Vara klassi nimi on kohustuslik")
                .MaximumLength(100).WithMessage("Nimi peab olema 1-100 marki")
                // Oma loogikaga valideerimise reegel - sama nimega vara klass ei tohi kaks korda andmebaasis olla
                .Custom((name, context) =>
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        return;
                    }

                    var command = (SaveAssetClassCommand)context.InstanceToValidate;

                    if (_dbContext.AssetClasses.Any(assetClass => assetClass.Name == name && assetClass.Id != command.Id))
                    {
                        context.AddFailure(nameof(SaveAssetClassCommand.Name), "Sellise nimega vara klass on juba olemas");
                    }
                });
        }
    }
}
