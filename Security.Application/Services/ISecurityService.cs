using Security.Application.Dto;
using Security.Application.Utils;

namespace Security.Application.Services
{
    public interface ISecurityService
    {
        Task<Result<string>> Login(string username, string password);

        Task<Result> Register(RegisterAplicationUserModel model, bool isAdmin, bool emailConfirmationRequired);

        Task<Result<string>> DecodeJwt(string token);
    }
}
