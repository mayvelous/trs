using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trs.backend.Common;
using trs.backend.Models;

namespace trs.backend.Services
{
    public class SimulationServices : ISimulationServices
    {
        public RobotPosition RobotNow;
        public IList<Placement> PlacementConfigs;
        private readonly int minVal = 0;
        private readonly int maxVal = 4;

        public SimulationServices(RobotPosition robo)
        {
            RobotNow = robo;
            // Initialize and configure default placement settings
            PlacementConfigs = ConfigurePlacements();
        }

        // TODO: Need to refine this so later can load JSON list etc..
        public IList<Placement> ConfigurePlacements()
        {
            /* 
            N = { 0, 1, 1}
            E = { 1, 0, 2}
            S = { 0, -1, 3}
            W = {-1, 0, 4}
            */
            var PlacementList = new List<Placement>();
            // North Config
            PlacementList.Add(new Placement()
            {
                Name = "NORTH",
                XVal = 0,
                YVal = 1,
                FacingVal = 1
            });

            // East Config
            PlacementList.Add(new Placement()
            {
                Name = "EAST",
                XVal = 1,
                YVal = 0,
                FacingVal = 2
            });

            // South Config
            PlacementList.Add(new Placement()
            {
                Name = "SOUTH",
                XVal = 0,
                YVal = -1,
                FacingVal = 3
            });

            // West Config
            PlacementList.Add(new Placement()
            {
                Name = "WEST",
                XVal = -1,
                YVal = 0,
                FacingVal = 4
            });

            return PlacementList;
        }

        public InputInfo GetValidatedCommand(string cmdStr, ref bool isFirstCmd)
        {
            // Convert cmdVal to InputInfo for easier binding
            var result = new InputInfo();

            // Validate Command Keyword
            if (String.IsNullOrEmpty(cmdStr)) throw new Exception("Invalid command. Please use:\n PLACE X,Y,F \n MOVE \n LEFT \n RIGHT \n REPORT");

            // Parse the cmdStr 
            var inVals = cmdStr.Trim().ToUpper().Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            // Validate command key
            if (!Enum.IsDefined(typeof(Commands), inVals[0])) throw new Exception("Invalid command. Please use any of the keywords: PLACE, MOVE, LEFT, RIGHT, REPORT");

            // Return a valid Commands Enum
            result.CmdKey = (Commands)Enum.Parse(typeof(Commands), inVals[0]);

            // Check a valid first command
            if (isFirstCmd && result.CmdKey != Commands.PLACE) throw new Exception("Invalid first command. Please start with PLACE X,Y,F");

            // To check consecutive PLACE command any times after the very first entry is done
            if (result.CmdKey == Commands.PLACE)
            {
                // validate cmd
                if (inVals.Length != 4) throw new Exception("Invalid PLACE command, Please start with PLACE X, Y, F");
                // validate X value
                int xval = 0;
                if (!int.TryParse(inVals[1], out xval)) throw new Exception("X value must be a number");
                if (xval < minVal || xval > maxVal) throw new Exception("X value must be between " + minVal + " and " + maxVal);
                // validate Y value
                int yval = 0;
                if (!int.TryParse(inVals[2], out yval)) throw new Exception("Y value must be a number");
                if (yval < minVal || yval > maxVal) throw new Exception("Y value must be between " + minVal + " and " + maxVal);
                // validate F value
                if (!Enum.IsDefined(typeof(Directions), inVals[3])) throw new Exception("F value must be of " + string.Join(", ", Enum.GetNames(typeof(Directions))));

                result.XVal = xval;
                result.YVal = yval;
                result.FVal = (Directions)Enum.Parse(typeof(Directions), inVals[3]);

                // If all passed then set it to false
                if(isFirstCmd)
                    isFirstCmd = false;
            }
            
            return result;
        }

        public RobotPosition PerformAction(InputInfo inputInfo)
        {
            switch (inputInfo.CmdKey)
            {
                case Commands.PLACE:
                    return Place(inputInfo);
                    break;
                case Commands.MOVE:
                    return Move(inputInfo);
                    break;
                //case Commands.LEFT:
                //    _svc.Left();
                //    break;
                //case Commands.RIGHT:
                //    _svc.Right();
                //    break;
                //case Commands.REPORT:
                //    _svc.Report();
                //    break;
                default:
                    throw new Exception("Something not right, should never see this!");
                    //break;

            }
        }

        public RobotPosition Place(InputInfo inputInfo)
        {
            if (RobotNow == null) throw new Exception("Robot hasnt been initialize yet.");
            RobotNow.XVal = inputInfo.XVal.GetValueOrDefault();
            RobotNow.YVal = inputInfo.YVal.GetValueOrDefault();
            RobotNow.Facing = inputInfo.FVal;

            return RobotNow;
        }

        public RobotPosition Move(InputInfo inputInfo)
        {
            string facingAt = Enum.GetName(typeof(Directions), inputInfo.FVal);
            // Check RobotNow Facing value against placementconfig
            var conf = PlacementConfigs.FirstOrDefault(x => x.Name == facingAt);
            if (conf == null) throw new Exception("Cannot find placement configs using current facing.");
            // Validate the new coordinates before assigning to RoboNow
            var newX = RobotNow.XVal + conf.XVal;
            var newY = RobotNow.YVal + conf.YVal;

            if (IsValidXY(newX, newY))
            {
                // Assign new values to RoboNow
                RobotNow.XVal += conf.XVal;
                RobotNow.YVal += conf.YVal;
            }

            return RobotNow;
        }

        //public RobotPosition Left(RobotPosition pos)
        //{
        //    throw new NotImplementedException();
        //}

        //public RobotPosition Right(RobotPosition pos)
        //{
        //    throw new NotImplementedException();
        //}

        //public RobotPosition Report(RobotPosition pos)
        //{
        //    throw new NotImplementedException();
        //}

        //public RobotPosition Place(RobotPosition pos)
        //{
        //    throw new NotImplementedException();
        //}

        private bool IsValidXY(int xVal, int yVal)
        {
            if (xVal < minVal || xVal > maxVal || yVal < minVal || yVal > maxVal) throw new Exception("Oop bad move, you can go there!");
            return true;
        }
    }
}
