using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.UseCase.Command.Security.Login;
using Application.UseCase.Command.Security.RegistrarUsuario;
using Application.UseCase.Query.Usuarios;

namespace SecurityWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IMediator _mediator;


        public SecurityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(new
                {
                    jwt = result.Value,
                });
            }
            else
            {
                return Unauthorized();
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("registrar-comun")]
        public async Task<IActionResult> RegistrarComun([FromBody] RegistrarUsuarioCommand command)
        {
            command.Roles.Clear();
            command.Roles.Add("CommonUser");
            command.isAdmin = false;
            command.emailConfirmationRequired = false;

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(new
                {
                    result
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registrar-administrador")]
        public async Task<IActionResult> RegistrarAdministrador([FromBody] RegistrarUsuarioCommand command)
        {
            command.Roles.Clear();
            command.Roles.Add("Administrator");
            command.isAdmin= true;
            command.emailConfirmationRequired = false;

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(new
                {
                    result
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [Route("administradores")]
        [HttpGet]
        public async Task<IActionResult> BuscarUsuarioAdministrador()
        {
            var query = new GetListaUsuarioQuery();
            query.Rol = "Administrator";
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [AllowAnonymous]
        [Route("comunes")]
        [HttpGet]
        public async Task<IActionResult> BuscarUsuarioComun()
        {
            var query = new GetListaUsuarioQuery();
            query.Rol = "CommonUser";
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
