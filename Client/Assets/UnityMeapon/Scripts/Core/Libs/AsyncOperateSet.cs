using System;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace MeaponUnity.Core.Libs
{
    public class AsyncOperateSet
    {
        private int m_Count;
        private Action m_Complete;

        public AsyncOperateSet(params AsyncOperate[] operates)
        {
            m_Count = operates.Length;
            foreach (AsyncOperate op in operates)
            {
                if (op.State == DreamAsyncState.Complete || op.State == DreamAsyncState.Failure)
                {
                    m_Count--;
                    continue;
                }
                else
                {
                    op.OnCompleteEvent += InternalCallback;
                    op.OnFailureEvent += InternalCallback;
                }
            }
        }

        public AsyncOperateSet OnComplete(Action callback)
        {
            m_Complete = null;
            m_Complete = callback;
            return this;
        }

        private void InternalCallback(object sender, GameEventArgs args)
        {
            m_Count--;
            if (m_Count == 0)
            {
                m_Complete?.Invoke();
                m_Complete = null;
            }
            else if (m_Count < 0)
            {
                Log.Error("[DreamAsyncOperateSet]Count Error.");
            }
        }
    }
}
