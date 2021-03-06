﻿using System;
using System.Diagnostics;
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
                if (inputStr.Trim().ToUpper() == "EXIT")
                {
                    Environment.Exit(0);
                }

                try
                {
                    // Validate Command Keyword
                    var inputInfo = _svc.GetValidatedCommand(inputStr, ref isFirstCmd);
                    // Perform Command Action
                    var strResult = _svc.PerformAction(inputInfo);
                    if(!String.IsNullOrEmpty(strResult))
                        Console.WriteLine(strResult);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
