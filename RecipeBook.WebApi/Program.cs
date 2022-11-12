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
builder.Configuration.AddEnvironmentVariables("FILE_STORAGE").AddEnvironmentVariables("CONNECTION_STRING");
var fileStorageEnv = Environment.GetEnvironmentVariable("FILE_STORAGE");
string staticStorageDirectoryPath;
if (fileStorageEnv is null)
{
    staticStorageDirectoryPath = builder.Configuration.GetSection("FileStorageSettings:BasePath").Value;
    builder.Services.AddSingleton(builder.Configuration.GetSection("FileStorageSettings").Get<FileStorageSettings>());
}
else
{
    staticStorageDirectoryPath = fileStorageEnv;
    builder.Services.AddSingleton(builder.Configuration.Get<FileStorageSettings>().BasePath = fileStorageEnv);
}

var staticStorageDirectory = new DirectoryInfo(staticStorageDirectoryPath + "\\images");
if (!staticStorageDirectory.Exists) staticStorageDirectory.Create();

builder.Services.AddDependencies();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

var connectionStringEnv = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Services.AddDbContext<RecipeBookDbContext>(options => options.UseNpgsql(
    connectionStringEnv ?? builder.Configuration.GetConnectionString("ConnectionString"), b =>
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

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<RecipeBookDbContext>();
    dataContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("FlutterOrigins");
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();