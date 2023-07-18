using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Application.UseCase.Command.Security.RegistrarUsuario;
using Application.Utils;
using Security.Domain.Event.Usuarios;
using Infrastructure.Security;
using Shared.Core;

namespace Infrastructure.Command.Security.RegistrarUsuario
{
    public class RegistrarUsuarioHandler : IRequestHandler<RegistrarUsuarioCommand, Result>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        private readonly ILogger<RegistrarUsuarioHandler> _logger;
        public RegistrarUsuarioHandler(UserManager<ApplicationUser> userManager, IMediator mediator, ILogger<RegistrarUsuarioHandler> logger)
        {
            _userManager = userManager;
            _mediator = mediator;
            _logger = logger;
        }
        public async Task<Result> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{request.Email} is trying to register");
            var newUser = new ApplicationUser(request.UserName, request.FirstName, request.LastName, request.Email, request.FechaNacimiento, true, request.isAdmin);

            IdentityResult userCreated = await _userManager.CreateAsync(newUser, request.Password);

            if (userCreated.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                if (request.emailConfirmationRequired)
                {
                    //TODO: Send email confirmation
                }
                else
                {
                    IdentityResult result = await _userManager.ConfirmEmailAsync(newUser, token);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRolesAsync(newUser, request.Roles.AsEnumerable());


                        var domainEvent = new UsuarioCreado(newUser.Id, newUser.FullName, newUser.UserName);
                        domainEvent.MarkAsConsumed();
                        await _mediator.Publish(domainEvent);
                        Type type = typeof(ConfirmedDomainEvent<>).MakeGenericType(domainEvent.GetType());
                        var confirmedEvent = (INotification)Activator.CreateInstance(type, domainEvent);
                        await _mediator.Publish(confirmedEvent);
                        return new Result(true, "User created");
                    }
                    else
                    {
                        return new Result(false, "User created but email confirmation failed");
                    }
                }
            }

            userCreated.Errors.ToList().ForEach(error => _logger.LogError("Error { ErrorCode }: { Description }", error.Code, error.Description));
            return new Result(false, "User not created");
        }

    }
}
