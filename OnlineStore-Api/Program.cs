using System.Reflection;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineStore_Api.Helpers.Config;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
// Repositories
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IImageRepo, ImageRepo>();

// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICateogryService, CateogryService>();
builder.Services.AddScoped<IImageService, ImageService>();

// Config
builder.Services.Configure<LocalFileSettings>(builder.Configuration.GetSection("FileSettings"));

// Special injections
if (builder.Environment.IsDevelopment())
    builder.Services.AddScoped<IImageProcessor, ImageProcessLocal>();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString, builder =>
    {
        builder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10), null);
    });
});

// Serilog
var logger = new LoggerConfiguration()
// .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/storeLogs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.AddSerilog(logger);

// Mapster
// Tell Mapster to scan this assembly searching for the Mapster.IRegister classes and execute them
TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
