using FluentValidation;

namespace Cuzdanim.Application.Features.Budgets.Commands.CreateBudget;

public class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı ID zorunludur");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Kategori seçimi zorunludur");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Bütçe adı zorunludur")
            .MaximumLength(200).WithMessage("Bütçe adı 200 karakterden uzun olamaz");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Bütçe tutarı 0'dan büyük olmalıdır");

        RuleFor(x => x.Currency)
            .IsInEnum().WithMessage("Geçerli bir para birimi seçiniz");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Başlangıç tarihi zorunludur");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Bitiş tarihi zorunludur")
            .GreaterThan(x => x.StartDate).WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır");

        RuleFor(x => x.AlertThresholdPercentage)
            .InclusiveBetween(0, 100).WithMessage("Uyarı eşiği %0 ile %100 arasında olmalıdır");
    }
}