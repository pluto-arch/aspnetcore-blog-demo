using MediatR;

namespace Pluto.BlogCore.Domain.Events.UserEvents
{
    public class EnableUserEvent : INotification
    {

        public string Message { get; set; }


        public EnableUserEvent(string msg)
        {
            Message = msg;
        }
    }
}