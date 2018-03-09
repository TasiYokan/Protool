using System.Collections.Generic;
using System;
using UnityEngine;

namespace TasiYokan.EventsInUnity
{
    public class SimpleEventDispatcher : BaseEventDispatcher,
        ISimpleEventDispatcher
    {
        #region Fields


        #endregion Fields

        #region Constructors

        public SimpleEventDispatcher(SimpleEventDispatcherMono _host = null) : base(_host)
        {
        }

        #endregion Constructors

        #region Properties

        // If no mono hosts it, then it host itself.
        // We hide it inside dispatcher so that we can get the mono explicitly.
        private ISimpleEventDispatcher Host
        {
            get
            {
                return base.m_host as ISimpleEventDispatcher;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Automatically select the handler with correct signature.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_evtType"></param>
        /// <param name="_evt"></param>
        public void DispatchEvent<T>(string _evtType, T _evt)
            where T : BaseEvent
        {
            // Everytime we dispatch an event,
            // its type ought to be the same as what we specified in the signature
            _evt.eventType = _evtType;
            EventRegister.DispatchEvent<T>(_evtType, _evt);
        }

        #endregion Methods
    }
}