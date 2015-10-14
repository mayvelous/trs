using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trs.backend.Common;

namespace trs.backend.Models
{
    public class InputInfo
    {
        public Commands CmdKey { get; set; }
        public int? XVal { get; set; }
        public int? YVal { get; set; }
        public Directions FVal { get; set; }
    }
}
