using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace Domain.Base;

public class BaseRefreshToken : BaseRefreshToken<Guid>
{ 
}

public class BaseRefreshToken<TKey> : DomainEntityId
    where TKey : struct, IEquatable<TKey>
{
    [MaxLength(64)]
    public string RefreshToken { get; set; } = Guid.NewGuid().ToString();

    public DateTime ExpirtationDT { get; set; } = DateTime.UtcNow.AddDays(7);
}