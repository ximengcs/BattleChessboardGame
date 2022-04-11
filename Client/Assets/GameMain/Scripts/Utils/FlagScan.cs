using System.Collections.Generic;

namespace Try2
{
    public class FlagScan
    {
        Dictionary<int, int> flags;

        public FlagScan(int amount)
        {
            flags = new Dictionary<int, int>(amount);
        }

        public void Add(int id = 0)
        {
            if (!flags.ContainsKey(id))
                flags[id] = 1;
            else
                flags[id] += 1;
        }

        public void Minus(int id = 0)
        {
            flags[id] -= 1;
        }

        public bool Yes(int id = 0)
        {
            return flags[id] == 0;
        }
    }
}
