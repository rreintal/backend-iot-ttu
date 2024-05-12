using System.Text;
using App.BLL;
using App.BLL.Contracts;
using App.BLL.Services.ImageStorageService;
using App.DAL.Contracts;
using App.DAL.EF;
using App.DAL.EF.Seeding;
using App.Domain.Identity;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Public.DTO.ApiExceptions;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(cfg =>
{
    cfg.Filters.Add(new MyAPIExceptionFilter());
});

// Dependency injection
builder.Services.AddScoped<AppDbContext>();

// Add UOW
builder.Services.AddScoped<IAppUOW, AppUOW>();
builder.Services.AddScoped<IAppBLL, AppBLL>();

// DI for custom services
builder.Services.AddScoped<IImageStorageService, ImageStorageService>();

// Add CORS

builder.Services.AddCors(options =>
{
    
    options.AddPolicy("develop", policyBuilder =>
    {
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowAnyOrigin();
        policyBuilder.AllowAnyHeader();
    } );
    
});



string? databaseUrl = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.DB_CONNECTION);
if (databaseUrl == null)
{
    throw new InvalidOperationException("Database connection string is null.");
}

builder.Services
    .AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(databaseUrl, opt =>
        {
            opt.CommandTimeout(60);
        }).EnableSensitiveDataLogging();
    });



builder.Services.AddIdentity<AppUser, AppRole>(
        options =>
            options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var JWT_ISSUER = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_ISSUER);
var JWT_AUDIENCE = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_AUDIENCE);
var JWT_KEY = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.JWT_KEY);

if (JWT_AUDIENCE == null || JWT_ISSUER == null || JWT_KEY == null)
{
    throw new InvalidOperationException("JWT Environment variables are missing");
}

// Authentication
// ----------------------------
builder.Services
    .AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = JWT_ISSUER,
            ValidAudience = JWT_AUDIENCE,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JWT_KEY)),
            ClockSkew = TimeSpan.Zero
        };
    });


// ----------------------------
// Automapper
builder.Services.AddAutoMapper(
    typeof(DAL.DTO.AutoMapperConfig),
    typeof(BLL.DTO.AutoMapperConfig)
);
// ----------------------------

// Swagger //
var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    // in case of no explicit version
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

apiVersioningBuilder.AddApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

builder.Services.Configure<IdentityOptions>(options =>
{ // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 1;
    options.User.RequireUniqueEmail = true;
});



var app = builder.Build();

// Setup start data
await AppDataSeeding.SetupAppData(app.Services, app.Configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();
}

// Swagger //
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName);
    }
});

app.UseHsts();
app.UseHttpsRedirection();
app.UseCors("develop");
app.UseRouting();
app.UseAuthorization(); 
app.UseStaticFiles();



app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

/// <inheritdoc>
///     <cref></cref>
/// </inheritdoc>
public partial class Program { }