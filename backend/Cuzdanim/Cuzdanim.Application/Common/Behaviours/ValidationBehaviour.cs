using FluentValidation;
using MediatR;

namespace Cuzdanim.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // 1. Eğer validator yoksa, direkt devam et
        if (!_validators.Any())
        {
            return await next();
        }

        // 2. Validasyon context oluştur
        var context = new ValidationContext<TRequest>(request);

        // 3. Tüm validator'ları çalıştır
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // 4. Hataları topla
        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        // 5. Eğer hata varsa exception fırlat
        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        // 6. Validasyon başarılı, handler'a devam et
        return await next();
    }
}
