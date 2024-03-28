using Base.Domain;
using Contracts;

namespace App.Domain;

public class EmailRecipents : DomainEntityId
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = default!;
    public string? Comment { get; set; }
}