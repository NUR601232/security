using MediatR;
using Application.Utils;

namespace Application.UseCase.Command.Security.Login
{
    public record LoginCommand : IRequest<Result<string>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
