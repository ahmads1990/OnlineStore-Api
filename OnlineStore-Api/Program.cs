using System.Reflection;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineStore_Api.Helpers;
using OnlineStore_Api.Helpers.Config;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "bearer",
        Description = "Please insert JWT token into field"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// DI
// Repositories
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IImageRepo, ImageRepo>();

// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICateogryService, CateogryService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<IAuthService, AuthService>();

// Config
builder.Services.Configure<FileStorage>(builder.Configuration.GetSection("FileStorage"));

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

// Security
// configure jwt helper class to use jwt config info
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));

// add Identity with options configuration
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
}).AddEntityFrameworkStores<AppDbContext>();

// Add Authentication with jwt config
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "Random key")),
    };
});

builder.Services.AddAuthorization();

// Serilog
var logger = new LoggerConfiguration()
// .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/storeLogs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.AddSerilog(logger);

// CORS
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? new[] { "" };

    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins(allowedOrigins)
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Mapster
var fileStorage = app.Services.GetRequiredService<IOptions<FileStorage>>().Value;
// Register Mapster mappings
MapsterConfig.RegisterMappings(fileStorage);
// Tell Mapster to scan this assembly searching for the Mapster.IRegister classes and execute them
TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowLocalhost");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
