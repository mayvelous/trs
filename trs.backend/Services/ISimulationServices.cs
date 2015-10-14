using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trs.backend.Models;

namespace trs.backend.Services
{
    public interface ISimulationServices
    {
        RobotPosition Move(RobotPosition pos);

    }
}
