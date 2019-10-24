// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Quantum.PQC
{
    using global::PQC.Functional.PatternRecognition;
    using System;

    /// <summary>
    /// Driver Class for Initialization
    /// </summary>
    class Driver
    {
        /// <summary>
        /// Method to instantiate the program execution with string arguments
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Quantum Perceptron | Pattern Recognition");

                    // Get execution object by user choice from BaseDriver Factory
                    new PatternRecognitionHandler().Initialize();

                    if(Stop())
                    {
                        break;
                    }

                    Console.WriteLine();
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private static bool Stop()
        {
            try
            {
                Console.Write(
                    "Do you want to run another Test\n [Y[y] or N[n] or [Enter]" +
                    "(If left blank or incorrect value is entered,\n" +
                    "it will end the execution by default)\n" +
                    "Ans: ");
                string userInput = Console.ReadLine().Trim();

                if (!string.IsNullOrEmpty(userInput) &&
                    userInput.Length == 1)
                {
                    return userInput.ToLower() == "y" ? false : true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }

        private static int UserChoice()
        {
            try
            {
                return 3;

                Console.WriteLine(
                    "Do you want to execute: \n" +
                    "[1] Simple Perceptron Test,\n" +
                    "[2] Image Processing,\n" +
                    "[3] Pattern Recognition \n" +
                    "[Enter 1, 2 or 3 for selection]\n\n");
                Console.Write("Ans: ");
                int choice = int.Parse(Console.ReadLine());
                Console.WriteLine("\n\n");

                return choice;
            }
            catch (Exeception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}