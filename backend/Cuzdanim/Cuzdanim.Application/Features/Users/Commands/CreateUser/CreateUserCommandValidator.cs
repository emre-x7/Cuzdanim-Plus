using FluentValidation;

namespace Cuzdanim.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email zorunludur")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
            .MaximumLength(256).WithMessage("Email 256 karakterden uzun olamaz");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre zorunludur")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır")
            .Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir")
            .Matches(@"[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir")
            .Matches(@"[0-9]").WithMessage("Şifre en az bir rakam içermelidir")
            .Matches(@"[\W_]").WithMessage("Şifre en az bir özel karakter içermelidir");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad zorunludur")
            .MaximumLength(100).WithMessage("Ad 100 karakterden uzun olamaz");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad zorunludur")
            .MaximumLength(100).WithMessage("Soyad 100 karakterden uzun olamaz");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^(\+90|0)?[0-9]{10}$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Geçerli bir Türkiye telefon numarası giriniz (örn: 5551234567)");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Doğum tarihi bugünden önce olmalıdır")
            .GreaterThan(DateTime.Today.AddYears(-120))
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Geçerli bir doğum tarihi giriniz");
    }
}