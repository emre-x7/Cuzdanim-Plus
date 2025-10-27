using FluentValidation;

namespace Cuzdanim.Application.Features.Goals.Commands.AddContribution;

public class AddContributionCommandValidator : AbstractValidator<AddContributionCommand>
{
    public AddContributionCommandValidator()
    {
        RuleFor(x => x.GoalId)
            .NotEmpty().WithMessage("Hedef ID zorunludur");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Katkı tutarı 0'dan büyük olmalıdır");

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Geçerli bir para birimi seçiniz");
    }
}