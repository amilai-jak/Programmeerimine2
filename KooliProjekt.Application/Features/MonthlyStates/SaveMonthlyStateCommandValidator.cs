using System.Linq;
using System;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.MonthlyStates
{
    // 15.11.2025
    // Valideerimise klass SaveMonthlyStateCommand kasu jaoks
    // Voetakse programmi poolt kulge automaatselt
    public class SaveMonthlyStateCommandValidator : AbstractValidator<SaveMonthlyStateCommand>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveMonthlyStateCommandValidator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(command => command.StateDate)
                .Must(stateDate => stateDate != default(DateTime)).WithMessage("Kuupaev on kohustuslik")
                // Oma loogikaga valideerimise reegel - sama kuupaevaga olek ei tohi kaks korda andmebaasis olla
                .Custom((stateDate, context) =>
                {
                    if (stateDate == default(DateTime))
                    {
                        return;
                    }

                    var command = (SaveMonthlyStateCommand)context.InstanceToValidate;

                    if (_dbContext.MonthlyStates.Any(state => state.StateDate == stateDate && state.Id != command.Id))
                    {
                        context.AddFailure(nameof(SaveMonthlyStateCommand.StateDate), "Selle kuupaevaga olek on juba olemas");
                    }
                });

            RuleFor(command => command.UninvestedCash)
                .GreaterThanOrEqualTo(0).WithMessage("Vaba raha ei saa olla negatiivne");

            RuleFor(command => command.Deposits)
                .GreaterThanOrEqualTo(0).WithMessage("Sissemakse ei saa olla negatiivne");

            RuleFor(command => command.Withdrawals)
                .GreaterThanOrEqualTo(0).WithMessage("Valjamakse ei saa olla negatiivne");

            RuleFor(command => command.TotalPortfolioValue)
                .GreaterThanOrEqualTo(0).WithMessage("Portfelli vaartus ei saa olla negatiivne");
        }
    }
}
