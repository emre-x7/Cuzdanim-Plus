namespace Cuzdanim.Application.Features.Accounts.DTOs;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string? BankName { get; set; }
    public string? IBAN { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}