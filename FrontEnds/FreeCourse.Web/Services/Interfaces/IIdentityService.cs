using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models;
using IdentityModel.Client;

namespace FreeCourse.Web.Services.Interfaces;

public interface IIdentityService
{
    Task<Response<bool>> SignIn(LoginRequest loginRequest);
    Task<TokenResponse> GetAccessTokenByRefreshToken();
    Task RevokeRefreshToken();
}