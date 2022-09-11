using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RecipeBook.Application.Entities;
using RecipeBook.Application.Extensions;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories;
using RecipeBook.Infrastructure;

namespace RecipeBook.Application.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public AuthService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
        IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Login(UserCommand userCommand)
    {
        var user = await _userRepository.GetByEmail(userCommand.Email);
        if (user == null) throw new Exception("User not found.");

        if (!PasswordExtension.VerifyPasswordHash(userCommand.Password, user.PasswordHash, user.PasswordSalt))
            throw new Exception("Wrong password.");

        var token = PasswordExtension.CreateToken(userCommand.Email,
            _configuration.GetSection("JWTSettings:SecretKey").Value);
        return token;
    }

    public async Task<string> Register(RegisterUserCommand registerUserCommand)
    {
        var user = await _userRepository.GetByEmail(registerUserCommand.Email);
        if (user == null) throw new Exception("User already exist.");

        PasswordExtension.CreatePasswordHash(registerUserCommand.Password, out var passwordHash, out var passwordSalt);
        await _userRepository.CreateUser(new User
        {
            Name = registerUserCommand.Name,
            Email = registerUserCommand.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        });

        await _unitOfWork.Commit();

        var token = PasswordExtension.CreateToken(registerUserCommand.Email,
            _configuration.GetSection("JWTSettings:SecretKey").Value);
        return token;
    }

    public async Task ValidateUser()
    {
        var isAuthenticatedUser = !_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated;
        if (!isAuthenticatedUser ?? true) throw new Exception("Invalid user.");

        var claimsEmail = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        if (claimsEmail == null) throw new Exception("Invalid user.");

        var user = await _userRepository.GetByEmail(claimsEmail);
        if (user == null) throw new Exception("Invalid user.");
    }
}