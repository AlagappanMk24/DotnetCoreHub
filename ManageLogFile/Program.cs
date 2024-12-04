using ManageLogFile.Data.Context;
using Microsoft.EntityFrameworkCore;
using ManageLogFile.Logger;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ManageLogFile.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ManageLogFile.Model;
using ManageLogFile;

var builder = WebApplication.CreateBuilder(args);

//-------------------------------------------------------------
// Configure Services
//-------------------------------------------------------------

// 1. Configure Database Context
builder.Services.AddDbContext<LogFileContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Register Application Services
builder.Services.RegisterServices();

// 3. Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });

// 4. Register JWT Settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
EnsureJwtSecretKey(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

// 5. Configure Filters, AutoMapper, and Controllers
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));

// 6. Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ManageLogFile API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the token here."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

//-------------------------------------------------------------
// Configure Middleware
//-------------------------------------------------------------
var app = builder.Build();

// 1. Enable Swagger for Development Environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 2. Configure HTTP Middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// 3. Configure Logging
ConfigureCustomLogging(app);

app.Run();

//-------------------------------------------------------------
// Helper Methods
//-------------------------------------------------------------

/// <summary>
/// Ensures the JWT SecretKey is generated and updated in appsettings.json if not already present.
/// </summary>
void EnsureJwtSecretKey(JwtSettings jwtSettings)
{
    if (!string.IsNullOrEmpty(jwtSettings.SecretKey)) return;

    var secretKey = SecretKeyGenerator.GenerateSecretKey();
    jwtSettings.SecretKey = secretKey;

    var appSettingsFile = "appsettings.json";
    var json = File.ReadAllText(appSettingsFile);
    dynamic jsonObj = JsonConvert.DeserializeObject(json);
    jsonObj["JwtSettings"]["SecretKey"] = secretKey;

    string updatedJson = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
    File.WriteAllText(appSettingsFile, updatedJson);
}

/// <summary>
/// Configures custom file logging using a dynamic log file path.
/// </summary>
void ConfigureCustomLogging(WebApplication app)
{
    string formattedDate = DateTime.Now.ToString("MM-dd-yyyy");
    string baseLogPath = builder.Configuration.GetValue<string>("Logging:LogFilePath");
    string logFilePath = Path.Combine(baseLogPath, $"log-{formattedDate}.txt");

    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
    loggerFactory.AddProvider(new CustomFileLoggerProvider(logFilePath, httpContextAccessor));
}