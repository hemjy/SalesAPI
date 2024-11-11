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

    public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<TokenDto>> { }
   
    internal class RefreshTokenCommandHandler(UserManager<User> userManager, IConfiguration configuration,
        IJwtTokenGenerator jwtTokenGenerator, IGenericRepositoryAsync<RefreshToken> refreshTokenRepo) : IRequestHandler<RefreshTokenCommand, Result<TokenDto>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IGenericRepositoryAsync<RefreshToken> _refreshTokenRepo = refreshTokenRepo;
        private readonly IConfiguration _configuration = configuration;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

        public async Task<Result<TokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenEntity = await _refreshTokenRepo.GetByAsync(t => t.Token == request.RefreshToken && !t.IsRevoked);

            if (tokenEntity == null || tokenEntity.ExpirationDate < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }
            // Generate a new access token
            var user = await _userManager.FindByIdAsync(tokenEntity.UserId.ToString());
            var (accessToken, refreshToken, refreshTokenExp) = _jwtTokenGenerator.GenerateJwtTokenInfo(user.Id, user.UserName);
            
            // Revoke old refresh token and store the new one
            tokenEntity.IsRevoked = true;
            var refreshTokenEntity = RefreshToken.Create(user.Id, refreshToken, refreshTokenExp);

            await _refreshTokenRepo.AddAsync(refreshTokenEntity, false);
            await _refreshTokenRepo.UpdateAsync(refreshTokenEntity, false);

            await _refreshTokenRepo.SaveChangesAsync();


            return Result<TokenDto>.Success(new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken });
        }


    }
}
