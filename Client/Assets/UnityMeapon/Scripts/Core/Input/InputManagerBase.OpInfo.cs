
using System;
using System.Collections.Generic;

namespace Meapon.Input
{
    public partial class InputManagerBase
    {
        private class OperateInfo
        {
            public int OprateCode;
            public List<int> InputCode;
            public Action<object> Callback;

            public OperateInfo(int opCode)
            {
                OprateCode = opCode;
            }
        }
    }
}
