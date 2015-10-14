using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trs.backend.Common
{
    public enum Turns : int
    {
        Left = 1,
        Right = -1
    }

    public enum Commands
    {
        PLACE,
        MOVE,
        LEFT,
        RIGHT,
        REPORT
    }
}
