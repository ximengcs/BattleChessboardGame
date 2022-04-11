using MeaponUnity.Core.Entry;

namespace Try2
{
    public class GameUtils
    {
        private static int Series;

        public static int GetSeries()
        {
            return Series++;
        }

        public static Game GetCurrent()
        {
            GameMainProcedure current = MeaponEntry.Procedure.CurrentProcedure as GameMainProcedure;
            return current.GameInst;
        }
    }
}
