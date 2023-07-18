using MediatR;
using Application.Dto.Usuarios;

namespace Application.UseCase.Query.Usuarios
{
    public class GetListaUsuarioQuery : IRequest<IEnumerable<UsuarioDto>>
    {
        public string Rol { get; set; }
    }
}
