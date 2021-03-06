﻿using StrumentiMusicali.Library.Core.Events.Base;
using StrumentiMusicali.Library.Core.interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StrumentiMusicali.Library.Core
{
    //Does used by EventAggregator to reserve subscription
    public class Subscription<Tmessage> : IDisposable
    {
        public readonly MethodInfo MethodInfo;
        private readonly EventAggregator EventAggregator;
        public readonly WeakReference TargetObjet;
        public readonly bool IsStatic;

        private bool isDisposed;

        public Subscription(Action<Tmessage> action, EventAggregator eventAggregator)
        {
            MethodInfo = action.Method;
            if (action.Target == null)
                IsStatic = true;
            TargetObjet = new WeakReference(action.Target);
            EventAggregator = eventAggregator;
        }

        ~Subscription()
        {
            if (!isDisposed)
                Dispose();
        }

        public void Dispose()
        {
            EventAggregator.UnSbscribe(this);
            isDisposed = true;
        }

        public Action<Tmessage> CreatAction()
        {
            if (TargetObjet.Target != null && TargetObjet.IsAlive)
                return (Action<Tmessage>)Delegate.CreateDelegate(typeof(Action<Tmessage>), TargetObjet.Target, MethodInfo);
            if (this.IsStatic)
                return (Action<Tmessage>)Delegate.CreateDelegate(typeof(Action<Tmessage>), MethodInfo);

            return null;
        }
    }

    public class EventAggregator
    {
        private readonly object lockObj = new object();
        private Dictionary<Type, IList> subscriber = new Dictionary<Type, IList>();
        private static EventAggregator _Instance;

        public EventAggregator()
        {
        }

        public static EventAggregator Instance()
        {
            if (_Instance == null)
            {
                _Instance = new EventAggregator();
            }
            return _Instance;
        }

        public void Publish<TMessageType>(TMessageType message)
        {
            Type t = typeof(TMessageType);
            IList sublst;
            if (subscriber.ContainsKey(t))
            {
                lock (lockObj)
                {
                    sublst = new List<Subscription<TMessageType>>(subscriber[t].Cast<Subscription<TMessageType>>());
                }

                foreach (Subscription<TMessageType> sub in sublst)
                {


                    var action = sub.CreatAction();
                    if (action != null)
                    {
                        if (message is FilterControllerEvent)
                        {
                            IKeyController obj = (action.Target as IKeyController);
                            if (obj != null && (message as FilterControllerEvent).GuidKey != obj.INSTANCE_KEY)
                            {
                                continue;
                            }

                        }
                        action(message);
                    }
                }
            }
        }

        public Subscription<TMessageType> Subscribe<TMessageType>(Action<TMessageType> action)
        {
            Type t = typeof(TMessageType);
            IList actionlst;
            var actiondetail = new Subscription<TMessageType>(action, this);

            lock (lockObj)
            {
                if (!subscriber.TryGetValue(t, out actionlst))
                {
                    actionlst = new List<Subscription<TMessageType>>();
                    actionlst.Add(actiondetail);
                    subscriber.Add(t, actionlst);
                }
                else
                {
                    actionlst.Add(actiondetail);
                }
            }

            return actiondetail;
        }

        public void UnSbscribe<TMessageType>(Subscription<TMessageType> subscription)
        {
            Type t = typeof(TMessageType);
            if (subscriber.ContainsKey(t))
            {
                lock (lockObj)
                {
                    subscriber[t].Remove(subscription);
                }
                subscription = null;
            }
        }


    }
}