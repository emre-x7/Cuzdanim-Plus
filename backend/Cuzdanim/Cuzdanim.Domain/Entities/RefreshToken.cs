using Cuzdanim.Domain.Common;

namespace Cuzdanim.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public string? RevokedByIp { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? ReplacedByToken { get; private set; }
    public string CreatedByIp { get; private set; }

    // İlişkiler
    public virtual User User { get; private set; }

    private RefreshToken() { }

    public static RefreshToken Create(Guid userId, string token, DateTime expiresAt, string createdByIp)
    {
        return new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiresAt = DateTime.SpecifyKind(expiresAt, DateTimeKind.Utc),
            CreatedByIp = createdByIp
        };
    }

    public void Revoke(string ipAddress, string? replacedByToken = null)
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        RevokedByIp = ipAddress;
        ReplacedByToken = replacedByToken;
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
}