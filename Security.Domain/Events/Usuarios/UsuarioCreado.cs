using Shared.Core;

namespace Security.Domain.Event.Usuarios
{
    public record UsuarioCreado : DomainEvent
    {
        public Guid UsuarioId { get; private set; }
        public string NombreCompleto { get; private set; }
        public string UserName { get; private set; }

        public UsuarioCreado(Guid usuarioId, string nombreCompleto, string userName) : base(DateTime.Now)
        {
            UsuarioId = usuarioId;
            NombreCompleto = nombreCompleto;
            UserName = userName;
        }
    }
}
