using FluentValidation;

namespace Cuzdanim.Application.Features.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("İşlem ID zorunludur");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Kategori seçimi zorunludur");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Tutar 0'dan büyük olmalıdır");

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Geçerli bir para birimi seçiniz");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("İşlem tarihi zorunludur");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Açıklama 500 karakterden uzun olamaz");

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notlar 2000 karakterden uzun olamaz");
    }
}