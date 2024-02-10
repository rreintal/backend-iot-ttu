using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class PartnerImage : DomainEntityId
{
    public string Image { get; set; } = default!;
}