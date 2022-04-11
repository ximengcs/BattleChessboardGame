using System;

namespace Try2
{
    public class RestAction : IItemAction
    {
        private RestActionData m_Data;
        private Action m_CompleteCallback;

        public RestAction()
        {

        }

        public void Initialize(IItemActionData userData)
        {
            m_Data = (RestActionData)userData;
        }

        public void OnCancel(Action callback)
        {

        }

        public void OnComplete(Action callback)
        {
            m_CompleteCallback = callback;
        }

        public void Start()
        {
            m_Data.Item.Complete();
            m_CompleteCallback?.Invoke();
        }

        public void Update()
        {

        }
    }
}
