using FluentValidation;

namespace Cuzdanim.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Hesap ID zorunludur");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Hesap adı zorunludur")
            .MaximumLength(200).WithMessage("Hesap adı 200 karakterden uzun olamaz");
    }
}