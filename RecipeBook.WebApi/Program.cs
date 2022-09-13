using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RecipeBook.Application.Configs;
using RecipeBook.Infrastructure;
using RecipeBook.WebApi;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Configuration.AddEnvironmentVariables("FILE_STORAGE");
var fileStorageEnv = Environment.GetEnvironmentVariable("FILE_STORAGE");
if (fileStorageEnv == null)
    builder.Services.AddSingleton(builder.Configuration.GetSection("FileStorageSettings").Get<FileStorageSettings>());
else
    builder.Services.AddSingleton(builder.Configuration.Get<FileStorageSettings>().BasePath = fileStorageEnv);


builder.Services.AddDependencies();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<RecipeBookDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("ConnectionString"), b =>
    {
        b.MigrationsAssembly("RecipeBook.Migrations");
        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    }));

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("JWTSettings:SecretKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options =>
    options.AddPolicy("FlutterOrigins",
        policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("FlutterOrigins");
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();