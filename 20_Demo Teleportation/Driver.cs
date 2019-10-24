﻿using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;
using System;

namespace Quantum._20_Demo_Teleportation
{
    class Driver
    {
        static void Main(string[] args)
        {
            using (var quantumSimulator = new QuantumSimulator())
            {
                var result = DemoOperation.Run(quantumSimulator).Result;
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}