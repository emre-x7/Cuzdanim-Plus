using Cuzdanim.Domain.Enums;
using FluentValidation;

namespace Cuzdanim.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı ID zorunludur");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Hesap adı zorunludur")
            .MaximumLength(200).WithMessage("Hesap adı 200 karakterden uzun olamaz");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Geçerli bir hesap tipi seçiniz");

        // InitialBalance - Kredi kartı için istisna 
        RuleFor(x => x.InitialBalance)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Type != AccountType.CreditCard) // Kredi kartı değilse negatif olamaz
            .WithMessage("Başlangıç bakiyesi negatif olamaz");

        // Kredi kartı için negatif veya sıfır olabilir 
        RuleFor(x => x.InitialBalance)
            .LessThanOrEqualTo(0)
            .When(x => x.Type == AccountType.CreditCard) //  Kredi kartı ise pozitif olamaz
            .WithMessage("Kredi kartı başlangıç bakiyesi pozitif olamaz (borç tutarını negatif girin)");

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Geçerli bir para birimi seçiniz");

        // IBAN formatı (Türkiye)
        RuleFor(x => x.IBAN)
            .Matches(@"^TR\d{24}$")
            .When(x => !string.IsNullOrEmpty(x.IBAN))
            .WithMessage("Geçerli bir IBAN giriniz (örn: TR330006100519786457841326)");

        // Kart son 4 hanesi
        RuleFor(x => x.CardLastFourDigits)
            .Matches(@"^\d{4}$")
            .When(x => !string.IsNullOrEmpty(x.CardLastFourDigits))
            .WithMessage("Kart son 4 hanesi rakam olmalıdır");

        // Kredi kartı için limit zorunlu
        RuleFor(x => x.CreditLimit)
            .GreaterThan(0)
            .When(x => x.Type == AccountType.CreditCard)
            .WithMessage("Kredi kartı için limit belirtilmelidir");
    }
}