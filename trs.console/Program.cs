using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trs.backend.Common;

namespace trs.console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Initialize and configure default placement settings
                var placementList = ConfigurePlacements();

                // Accept Command String Input
                string cmdStr = Console.ReadLine();

                // Validate Command Keyword
                var isValidCmd = ValidateCommand(cmdStr);

                // Perform Command Action
                if(isValidCmd)
                {

                }
            }
            catch (Exception ex)
            {
                // Print out Error message
            }
            
        }

        private static bool ValidateCommand(string cmdStr)
        {   
            // Validate Command Keyword
            if (String.IsNullOrEmpty(commandStr)) throw new Exception("Invalid command. Please use:\n PLACE X,Y,F \n MOVE \n LEFT \n RIGHT \n REPORT");

            // Parse the commandStr
            var cmdVals = cmdStr.Trim().ToUpper().Split(',');

            // Check a valid first command
            if(cmdVals.Length != 4 && cmdVals[0] != Commands.PLACE.ToString()) throw new Exception("Invalid first command, Please start with PLACE X, Y, F");
            if (Enum.IsDefined(typeof(Commands)cmdVals[0]))
            {

            }

            // Perform Command action
        }

        private static IList<Placement> ConfigurePlacements()
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
    }
}
