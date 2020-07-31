using MediatR;

using Microsoft.Extensions.Logging;

using Pluto.BlogCore.Domain.Events.UserEvents;
using Pluto.BlogCore.Infrastructure.Providers;

using System.Threading;
using System.Threading.Tasks;

namespace Pluto.BlogCore.Application.EventBus.Users
{
    public class EnableUserEventHandler : INotificationHandler<EnableUserEvent>
    {
        private readonly ILogger<EnableUserEventHandler> _logger;
        private readonly EventIdProvider _eventIdProvider;
        public EnableUserEventHandler(ILogger<EnableUserEventHandler> logger, EventIdProvider eventIdProvider)
        {
            this._logger = logger;
            _eventIdProvider = eventIdProvider;
        }

        public Task Handle(EnableUserEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(_eventIdProvider.EventId, "event:{notificationType} 。{@notification}", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
}