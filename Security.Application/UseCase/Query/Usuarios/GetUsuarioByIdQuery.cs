using Application.Dto.Usuarios;
using MediatR;

namespace Application.UseCase.Query.Usuarios
{
    public class GetUsuarioByIdQuery : IRequest<UsuarioDto>
    {
        public Guid UsuarioId { get; set; }
    }
}
