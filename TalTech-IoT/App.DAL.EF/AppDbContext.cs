using App.Domain;
using App.Domain.Identity;
using App.Domain.Translations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

// TODO: Tee mingi BaseDb jne
public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, 
    IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{

    public DbSet<LanguageString> LanguageStrings { get; set; } = default!;
    public DbSet<LanguageStringTranslation> LanguageStringTranslations { get; set; } = default!;

    public DbSet<FeedPagePost> FeedPagePosts { get; set; } = default!;
    public DbSet<FeedPage> FeedPages { get; set; } = default!;
    public DbSet<FeedPageCategory> FeedPageCategories { get; set; } = default!;

    public DbSet<News> News { get; set; } = default!;
    public DbSet<Content> Contents { get; set; } = default!;
    public DbSet<Project> Projects { get; set; } = default!;
    public DbSet<ContentType> ContentTypes { get; set; } = default!;
    public DbSet<TopicArea> TopicAreas { get; set; } = default!;
    public DbSet<HasTopicArea> HasTopicAreas { get; set; } = default!;
    public DbSet<AppRefreshToken> AppRefreshTokens { get; set; } = default!;
    
    public DbSet<PageContent> PageContents { get; set; } = default!;

    public DbSet<PartnerImage> PartnerImages { get; set; } = default!;

    public DbSet<HomePageBanner> HomePageBanners { get; set; } = default!;

    public DbSet<ContactPerson> ContactPersons { get; set; } = default!;

    public DbSet<OpenSourceSolution> OpenSourceSolutions { get; set; } = default!;

    public DbSet<ImageResource> ImageResources { get; set; } = default!;

    private const string TopicAreaUniqueNameExpression = "\"TopicAreaId\" IS NOT NULL";
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Identity
        // do not allow EF to create multiple FK-s, use existing RoleId and UserId
        builder.Entity<AppUserRole>()
            .HasOne(x => x.AppUser)
            .WithMany(x => x!.UserRoles)
            .HasForeignKey(x => x.UserId);

        builder.Entity<AppUserRole>()
            .HasOne(x => x.AppRole)
            .WithMany(x => x!.UserRoles)
            .HasForeignKey(x => x.RoleId);
        
        // Composite key for translations (languageStringId, "et")
        builder.Entity<LanguageStringTranslation>()
            .HasKey(e => new { e.LanguageStringId, e.LanguageCulture });

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

        // TopicArea <-> LanguageString 
        builder.Entity<TopicArea>()
            .HasOne(a => a.LanguageString)
            .WithOne()
            .HasForeignKey<TopicArea>(c => c.LanguageStringId)
            .IsRequired();

        builder.Entity<LanguageString>()
            .HasOne(ls => ls.TopicArea)
            .WithOne(a => a.LanguageString)
            .HasForeignKey<TopicArea>(a => a.LanguageStringId)
            .IsRequired(false);

        // Unique Indexes
        builder.Entity<ContentType>()
            .HasIndex(x => x.Name).IsUnique();
        
        builder.Entity<FeedPage>()
            .HasIndex(x => x.FeedPageName)
            .IsUnique();

        // disable cascade delete
        foreach (var relationship in builder.Model
                     .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
        
        // adding cascade delete 
        builder.Entity<News>()
            .HasMany(x => x.Content)
            .WithOne(x => x.News)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Content>()
            .HasOne<LanguageString>(x => x.LanguageString)
            .WithOne(x => x.Content)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<LanguageString>()
            .HasMany(x => x.LanguageStringTranslations)
            .WithOne(x => x.LanguageString)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<LanguageStringTranslation>()
            .HasOne(x => x.LanguageString)
            .WithMany(x => x.LanguageStringTranslations)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<HasTopicArea>()
            .HasOne<News>(x => x.News)
            .WithMany(x => x.HasTopicAreas)
            .OnDelete(DeleteBehavior.Cascade);
        
        // adding cascade delete for Project
        builder.Entity<Project>()
            .HasMany(x => x.Content)
            .WithOne(x => x.Project)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<HomePageBanner>()
            .HasMany(x => x.Content)
            .WithOne(x => x.HomePageBanner)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<ContactPerson>()
            .HasMany(x => x.Content)
            .WithOne(x => x.ContactPerson)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<FeedPagePost>()
            .HasMany(x => x.Content)
            .WithOne(x => x.FeedPagePost)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<FeedPageCategory>()
            .HasMany(x => x.Content)
            .WithOne(x => x.FeedPageCategory)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<OpenSourceSolution>()
            .HasMany(x => x.Content)
            .WithOne(x => x.OpenSourceSolution)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<News>()
            .HasMany(x => x.ImageResources)
            .WithOne(x => x.News)
            .OnDelete(DeleteBehavior.Cascade);
    }
}