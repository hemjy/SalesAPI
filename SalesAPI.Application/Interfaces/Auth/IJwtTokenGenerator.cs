namespace SalesAPI.Application.Interfaces.Auth
{
    public interface IJwtTokenGenerator
    {
        (string token, string refreshToken,  DateTime refreshTokenExp) GenerateJwtTokenInfo(string userId, string username);
    }
}
