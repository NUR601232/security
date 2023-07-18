using SharedKernel.Core;

namespace Shared.IntegrationEvents
{
    public record DonacionCompletada : IntegrationEvent
    {
        public Guid ProyectoId { get; set; }
        public Guid DonacionId { get; set; }
    }
}
