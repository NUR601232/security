using SharedKernel.Core;

namespace Shared.IntegrationEvents
{
    public record ProyectoCreado : IntegrationEvent
    {
        public Guid ProyectoId { get; set; }
        public string Titulo { get; set; }


    }
}
