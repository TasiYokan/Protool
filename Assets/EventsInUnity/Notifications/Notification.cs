using System;

namespace TasiYokan.EventsInUnity
{
    public delegate void NotificationHandler<TEvent>(TEvent e)
        where TEvent : Notification;

    public class Notification : BaseEvent
    {
        public readonly object RealSender;

        public Notification(object _sender):base()
        {
            RealSender = _sender;
        }
    }
}
