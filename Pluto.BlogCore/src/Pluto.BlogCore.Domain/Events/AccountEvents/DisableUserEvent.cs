using MediatR;

namespace Pluto.BlogCore.Domain.Events.UserEvents
{
    public class DisableUserEvent : INotification
    {

        public string Message { get; set; }


        public DisableUserEvent(string msg)
        {
            Message = msg;
        }
    }
}