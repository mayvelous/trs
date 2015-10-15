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
        private RobotPosition _robotAt;        
        private readonly int minVal = 0;
        private readonly int maxVal = 4;
        private IList<Placement> _configs;

        public SimulationServices(RobotPosition robot)
        {
            _robotAt = robot;
            // Initialize and configure default placement settings
            _configs = ConfigurePlacements();
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

        public string PerformAction(InputInfo inputInfo)
        {
            switch (inputInfo.CmdKey)
            {
                case Commands.PLACE:
                    _robotAt = Place(inputInfo);
                    break;
                case Commands.MOVE:
                    _robotAt = Move(inputInfo);
                    break;
                case Commands.LEFT:
                    _robotAt = Turning(inputInfo, Turns.Left);
                    break;
                case Commands.RIGHT:
                    _robotAt = Turning(inputInfo, Turns.Right);
                    break;
                case Commands.REPORT:
                    return GetRobotAt(_robotAt);
                    break;
                default:
                    return("Oops something gone wrong!");
                    break;
            }

            return GetRobotAt(_robotAt);
        }

        public RobotPosition Place(InputInfo inputInfo)
        {
            if (_robotAt == null) throw new Exception("Robot hasnt been initialize yet.");
            _robotAt.XVal = inputInfo.XVal.GetValueOrDefault();
            _robotAt.YVal = inputInfo.YVal.GetValueOrDefault();
            _robotAt.Facing = inputInfo.FVal;

            return _robotAt;
        }

        public RobotPosition Move(InputInfo inputInfo)
        {
            var conf = FindPlacementConfig(_robotAt.Facing);
            // Validate the new coordinates before assigning to RoboNow
            var newX = _robotAt.XVal + conf.XVal;
            var newY = _robotAt.YVal + conf.YVal;
            if (IsValidXY(newX, newY))
            {
                // Assign new values to RoboNow
                _robotAt.XVal += conf.XVal;
                _robotAt.YVal += conf.YVal;
            }

            return _robotAt;
        }

        public RobotPosition Turning(InputInfo inputInfo, Turns turnTo)
        {
            // get currently facing configs
            var conf = FindPlacementConfig(_robotAt.Facing);
            // Get the current facing value and +1 to it
            var newFacingVal = (int)conf.FacingVal + (int)turnTo;
            // Check for valid turn since can keep turning the same direction 360 again and again
            // if keep turing RIGHT +1
            if (newFacingVal > 4)
                newFacingVal = 1;
            // if keep turning LEFT -1
            if (newFacingVal <= 0)
                newFacingVal = 4;
            // Get the Directions of newFacingVal
            string newDirectionName = Enum.GetName(typeof(Directions), newFacingVal);
            Directions newDirection;
            Enum.TryParse(newDirectionName, out newDirection);

            // Update the current Robot facing value
            _robotAt.Facing = newDirection;
            return _robotAt;
        }

        private Placement FindPlacementConfig(Directions fVal)
        {
            string currentlyFacingAt = Enum.GetName(typeof(Directions), fVal);
            // Check RobotNow Facing value against placementconfig
            var conf = _configs.FirstOrDefault(x => x.Name == currentlyFacingAt);
            if (conf == null) throw new Exception("Cannot find placement configs using current facing.");
            return conf;
        }

        private bool IsValidXY(int xVal, int yVal)
        {
            if (xVal < minVal || xVal > maxVal || yVal < minVal || yVal > maxVal) throw new Exception("Oop bad move, you can go there!");
            return true;
        }

        private string GetRobotAt(RobotPosition robo)
        {
            if (robo == null) return string.Empty;
            // eg. Output: 0,0,WEST
            return String.Format(">> Robot At: {0},{1},{2}", robo.XVal, robo.YVal, robo.Facing);
        }
    }
}
