using MassTransit;
using MediatR;
using Security.Domain.Event.Usuarios;
using Shared.Core;

namespace Application.UseCase.DomainEventHandler.Proyectos
{
    public class PublishingIntegrationEventWhenUsuarioCreadoHandler : INotificationHandler<ConfirmedDomainEvent<UsuarioCreado>>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PublishingIntegrationEventWhenUsuarioCreadoHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(ConfirmedDomainEvent<UsuarioCreado> notification, CancellationToken cancellationToken)
        {
            Shared.IntegrationEvents.UsuarioCreado evento = new Shared.IntegrationEvents.UsuarioCreado()
            {
                UsuarioId = notification.DomainEvent.UsuarioId,
                NombreCompleto = notification.DomainEvent.NombreCompleto,
                UserName = notification.DomainEvent.UserName,
            };
            await _publishEndpoint.Publish<Shared.IntegrationEvents.UsuarioCreado>(evento);


        }
    }
}
