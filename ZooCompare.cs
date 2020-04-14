using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static ExceptionHandling.Program;

namespace ClassStructure
{
    class ZooCompare : IComparer<ZooController>
    {
        public int Compare([AllowNull] ZooController x, [AllowNull] ZooController y)
        {
            if (x.Count < y.Count)
            {
                return -1;
            }
            else if (x.Count > y.Count)
            {
                return 1;
            }
            else
                return 0;
        }
    }
}
