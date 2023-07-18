using MediatR;
using Application.Utils;

namespace Application.UseCase.Command.Security.RegistrarUsuario
{
    public record RegistrarUsuarioCommand : IRequest<Result>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public List<string> Roles { get; set; }

        public bool isAdmin { get; set; }
        public bool emailConfirmationRequired { get; set; }
        public RegistrarUsuarioCommand()
        {
            UserName = "";
            FirstName = "";
            LastName = "";
            Password = "";
            Email = "";
            Roles = new List<string>();
            isAdmin = false;
            emailConfirmationRequired = false;  
        }
    }
}
