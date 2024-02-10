using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Microsoft.EntityFrameworkCore;

namespace App.Domain;

public class ContentType : DomainEntityId
{
    public string Name { get; set; } = default!;
}