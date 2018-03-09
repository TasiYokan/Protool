using System;

namespace TasiYokan.EventsInUnity
{
    public class NotificationCenter
    {
        private static SimpleEventDispatcher m_instance;
        public static SimpleEventDispatcher Instance
        {
            get
            {
                if(m_instance == null)
                    m_instance = new SimpleEventDispatcher();
                return m_instance;
            }
        }
    }
}
