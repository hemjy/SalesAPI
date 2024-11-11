using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SalesAPI.Application.Commons;
using SalesAPI.Application.DTOs.Auth;
using SalesAPI.Application.Interfaces.Auth;
using SalesAPI.Application.Interfaces.Repositories;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<Result<TokenDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    internal class LoginCommandHandler(UserManager<User> userManager, IConfiguration configuration,
        IJwtTokenGenerator jwtTokenGenerator, IGenericRepositoryAsync<RefreshToken> refreshTokenRepo) : IRequestHandler<LoginCommand, Result<TokenDto>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IGenericRepositoryAsync<RefreshToken> _refreshTokenRepo = refreshTokenRepo;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

        public async Task<Result<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email)) return Result<TokenDto>.Failure("Email is Required");
            if (string.IsNullOrEmpty(request.Password)) return Result<TokenDto>.Failure("Password is Required");

            var user = await _userManager.FindByNameAsync(request.Email.Trim().ToLower());
         

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var (accessToken, refreshToken, refreshTokenExp) = _jwtTokenGenerator.GenerateJwtTokenInfo(user.Id, user.UserName);

            var refreshTokenEntity = RefreshToken.Create(user.Id, refreshToken, refreshTokenExp);

            await _refreshTokenRepo.AddAsync(refreshTokenEntity);


            return Result<TokenDto>.Success(new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken});
        }

      
    }
}
