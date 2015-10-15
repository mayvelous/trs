using System;
using trs.backend.Models;
using trs.backend.Services;

namespace trs.console
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool isFirstCmd = true;
            ISimulationServices _svc = new SimulationServices(new RobotPosition());

            Console.WriteLine("Robot Activated. Please enter your first command.");

            while (true)
            {
                // Accept Command String Input
                string inputStr = Console.ReadLine(); 
                if (inputStr == "exit" || inputStr == "EXIT")
                {
                    Environment.Exit(0);
                }

                try
                {
                    // Validate Command Keyword
                    var inputInfo = _svc.GetValidatedCommand(inputStr, ref isFirstCmd);
                    // Perform Command Action
                    Console.WriteLine(_svc.PerformAction(inputInfo));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
