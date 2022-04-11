using System;
using GameFramework.Event;
using MeaponUnity.Core.Entry;

namespace MeaponUnity.Core.Libs
{
    internal enum DreamAsyncState
    {
        None,
        Complete,
        Update,
        Failure
    }

    public class AsyncOperate
    {
        private DreamAsyncState m_State;
        private int m_EventID;
        private Action<object, GameEventArgs> m_OnComplete;
        private Action<object, GameEventArgs> m_OnUpdate;
        private Action<object, GameEventArgs> m_OnFailure;
        private Func<object, GameEventArgs, DreamAsyncState> m_Condition;

        public event Action<object, GameEventArgs> OnCompleteEvent
        {
            add { m_OnComplete += value; }
            remove { m_OnComplete -= value; }
        }

        public event Action<object, GameEventArgs> OnUpdateEvent
        {
            add { m_OnUpdate += value; }
            remove { m_OnUpdate -= value; }
        }

        public event Action<object, GameEventArgs> OnFailureEvent
        {
            add { m_OnFailure += value; }
            remove { m_OnFailure -= value; }
        }

        internal DreamAsyncState State
        {
            get { return m_State; }
        }

        AsyncOperate(int eventID, Func<object, GameEventArgs, DreamAsyncState> condition)
        {
            m_EventID = eventID;
            m_Condition = condition;
            MeaponEntry.Event.Subscribe(m_EventID, OnCallback);
        }

        public AsyncOperate OnComplete(Action<object, GameEventArgs> callback)
        {
            m_OnComplete = null;
            m_OnComplete = callback;
            return this;
        }

        public AsyncOperate OnUpdate(Action<object, GameEventArgs> callback)
        {
            m_OnUpdate = null;
            m_OnUpdate = callback;
            return this;
        }

        public AsyncOperate OnFailure(Action<object, GameEventArgs> callback)
        {
            m_OnFailure = null;
            m_OnFailure = callback;
            return this;
        }

        private void OnCallback(object sender, GameEventArgs args)
        {
            switch (m_Condition(sender, args))
            {
                case DreamAsyncState.Complete:
                    m_State = DreamAsyncState.Complete;
                    m_OnComplete?.Invoke(sender, args);
                    MeaponEntry.Event.Unsubscribe(m_EventID, OnCallback);
                    Dispose();
                    break;

                case DreamAsyncState.Update:
                    m_State = DreamAsyncState.Update;
                    m_OnUpdate?.Invoke(sender, args);
                    break;

                case DreamAsyncState.Failure:
                    m_State = DreamAsyncState.Failure;
                    m_OnFailure?.Invoke(sender, args);
                    MeaponEntry.Event.Unsubscribe(m_EventID, OnCallback);
                    Dispose();
                    break;
            }
        }

        private void Dispose()
        {
            m_OnComplete = null;
            m_OnUpdate = null;
            m_OnFailure = null;
            m_Condition = null;
        }
    }
}
