using FluentValidation;

namespace Cuzdanim.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı ID zorunludur");

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Hesap seçimi zorunludur");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Kategori seçimi zorunludur");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Geçerli bir işlem tipi seçiniz");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Tutar 0'dan büyük olmalıdır");

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Geçerli bir para birimi seçiniz");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("İşlem tarihi zorunludur");
        //.WithMessage("İşlem tarihi gelecekte olamaz");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Açıklama 500 karakterden uzun olamaz");

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notlar 2000 karakterden uzun olamaz");
    }
}