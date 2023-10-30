using App.BLL;
using App.BLL.Contracts;
using App.DAL.Contracts;
using App.DAL.EF;
using App.Domain;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Public.DTO.ApiExceptions;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(cfg =>
{
    cfg.Filters.Add(new MyAPIExceptionFilter());
});

// Dependency injection
builder.Services.AddScoped<AppDbContext>();

// Add UOW
builder.Services.AddScoped<IAppUOW, AppUOW>();
builder.Services.AddScoped<IAppBLL, AppBLL>();

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

// Add db 
// TODO Identity
//DockerDbConnection 
//DevDbConnection
var connectionString = builder.Configuration.GetConnectionString("DevDbConnection") ??
                       throw new InvalidOperationException("Connection string not found");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString, options =>
    {
        options.CommandTimeout(60);
    });
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
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();




var app = builder.Build();

// Setup start data
SetupAppData(app, app.Environment, app.Configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
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

app.UseCors("develop");
app.UseHttpsRedirection();
app.UseStaticFiles();




app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static void SetupAppData(IApplicationBuilder app, IWebHostEnvironment environment, IConfiguration configuration)
{
    using var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();
    using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
    if (configuration.GetValue<bool>("DataInit:DropDatabase"))
    {
        context!.Database.EnsureDeleted();
    }
    if (configuration.GetValue<bool>("DataInit:Migrate"))
    {
        context!.Database.Migrate();
    }
    if (configuration.GetValue<bool>("DataInit:Seed"))
    {
        var count = context!.ContentTypes.ToList().Count;
        if (count == 0)
        {
            var t1 = new ContentType()
            {
                Id = Guid.NewGuid(),
                Name = "BODY"
            };
            var t2 = new ContentType()
            {
                Id = Guid.NewGuid(),
                Name = "TITLE"
            };
            context.ContentTypes.Add(t1);
            context.ContentTypes.Add(t2);
        }

        var areasCount = context.TopicAreas.ToList().Count;

        if (areasCount == 0)
        {
            var t1 = TopicArea.TopicAreaFactory("Tehnoloogia", "Technology");
            var t2 = TopicArea.TopicAreaFactory("Robootika", "Robotics");
            var t3 = TopicArea.TopicAreaFactory("Arvutivõrgud", "Networking");
            var t3Child = TopicArea.TopicAreaFactory("4G", "4G");
            var t3Child2 = TopicArea.TopicAreaFactory("5G", "5G");
            t3Child.ParentTopicAreaId = t3.Id;
            t3Child2.ParentTopicAreaId = t3.Id;
            context.TopicAreas.AddRangeAsync(new List<TopicArea>() { t1, t2, t3, t3Child2, t3Child });
        }

        context.SaveChanges();
    }
}

public partial class Program { }