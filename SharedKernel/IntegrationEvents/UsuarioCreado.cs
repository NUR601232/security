using SharedKernel.Core;

namespace Shared.IntegrationEvents
{
    public record UsuarioCreado : IntegrationEvent
    {
        public Guid UsuarioId { get; set; }
        public string NombreCompleto { get; set; }
        public string UserName { get; set; }

    }
}
