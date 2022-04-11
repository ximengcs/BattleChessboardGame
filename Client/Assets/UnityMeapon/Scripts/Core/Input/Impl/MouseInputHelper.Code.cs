
namespace MeaponUnity.Core.Com
{
    public partial class MouseInputHelper
    {
        public class Code
        {
            public const int OnLeftClick = 0;
            public const int OnEnterArea = OnLeftClick + 1;
            public const int OnLeaveArea = OnEnterArea + 1;
            public const int OnLeftDown = OnLeaveArea + 1;
            public const int OnLeftUp = OnLeftDown + 1;
            public const int OnLeftDragStart = OnLeftUp + 1;
            public const int OnLeftDraging = OnLeftDragStart + 1;
            public const int OnLeftDragEnd = OnLeftDraging + 1;
        }
    }
}
