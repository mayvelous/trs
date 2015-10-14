﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trs.backend.Common;
using trs.backend.Models;

namespace trs.backend.Services
{
    public interface ISimulationServices
    {
        IList<Placement> ConfigurePlacements();
        InputInfo GetValidatedCommand(string cmdStr, ref bool isFirstCmd);
        RobotPosition PerformAction(InputInfo inputInfo);
        RobotPosition Place(InputInfo inputInfos);
        RobotPosition Move(InputInfo inputInfos);
        
        //RobotPosition Left(RobotPosition pos);
        //RobotPosition Right(RobotPosition pos);
        //RobotPosition Report(RobotPosition pos);
    }
}
