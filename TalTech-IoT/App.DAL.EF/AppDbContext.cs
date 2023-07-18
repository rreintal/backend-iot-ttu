using App.Domain;
using App.Domain.Identity;
using App.Domain.Translations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{

    public DbSet<LanguageString> LanguageStrings { get; set; } = default!;
    public DbSet<LanguageStringTranslation> LanguageStringTranslations { get; set; } = default!;

    public DbSet<AppUser> Users { get; set; } = default!;
    public DbSet<AppRole> Roles { get; set; } = default!;
    public DbSet<News> News { get; set; } = default!;
    public DbSet<Content> Contents { get; set; } = default!;
    public DbSet<ContentType> ContentTypes { get; set; } = default!;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Define that Content has LanguageString and LanguageString might not have Content!
        builder.Entity<Content>()
            .HasOne(c => c.LanguageString)
            .WithOne()
            .HasForeignKey<Content>(c => c.LanguageStringId)
            .IsRequired();

        builder.Entity<LanguageString>()
            .HasOne(ls => ls.Content)
            .WithOne(c => c.LanguageString)
            .HasForeignKey<Content>(c => c.LanguageStringId)
            .IsRequired(false);
        //

        builder.Entity<ContentType>()
            .HasIndex(x => x.Name).IsUnique();
        
        
        // TODO - lisa cascade delete, kui news kustutatakse siis kõik sellega seotud translationid ka!
        // disable cascade delete
        foreach (var relationship in builder.Model
                     .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}