using FluentValidation;

namespace Cuzdanim.Application.Features.Goals.Commands.UpdateGoal;

public class UpdateGoalCommandValidator : AbstractValidator<UpdateGoalCommand>
{
    public UpdateGoalCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Hedef ID zorunludur");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Hedef adı zorunludur")
            .MaximumLength(200).WithMessage("Hedef adı 200 karakterden uzun olamaz");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Açıklama 1000 karakterden uzun olamaz");

        RuleFor(x => x.TargetAmount)
            .GreaterThan(0).WithMessage("Hedef tutar 0'dan büyük olmalıdır");

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Geçerli bir para birimi seçiniz");

        RuleFor(x => x.TargetDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Hedef tarihi gelecekte olmalıdır");
    }
}