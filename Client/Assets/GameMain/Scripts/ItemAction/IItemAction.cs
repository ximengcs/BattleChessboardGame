using System;

namespace Try2
{
    public interface IItemAction
    {
        void Initialize(IItemActionData userData);
        void Start();
        void Update();
        void OnCancel(Action callback);
        void OnComplete(Action callback);
    }
}
