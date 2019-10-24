// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace PQC.Functional.PatternRecognition
{
    using PQC.Functional.ComputeManager;
    using PQC.Functional.UtilityManager;
    using PQC.Input;
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Pattern Recognition Handler Class
    /// </summary>
    public class PatternRecognitionHandler : BaseDriver
    {
        private readonly IQuantumPerceptronComputeHandler quantumPerceptronComputeHandler;
        private readonly IUtility utility;

        /// <summary>
        /// Pattern Recognition Handler Constructor
        /// </summary>
        public PatternRecognitionHandler()
        {
            this.quantumPerceptronComputeHandler = new QuantumPerceptronComputeHandler();
            this.utility = new Utility();
        }

        /// <summary>
        /// Method to initialize pattern recognition handler
        /// </summary>
        public override void Initialize()
        {
            try
            {
                this.ProcessPatternRecognition();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Method to perform the pattern recognition processing initiation
        /// Build input and weight vector and calculate averageDeviation from the
        /// DotProduct received
        /// </summary>
        /// <param name="testArray">Test Vector from Sample Image Selected</param>
        private void ProcessPatternRecognition()
        {
            // Get input vector in 2D array
            string inputArrayPath = this.GetArrayFilePath(0);
            long[,] inputArray = InputHandler.GetInput2DArray(inputArrayPath);
            long inputArrayRowCount = InputHandler.GetRowCount(inputArrayPath);
            Console.WriteLine("Original Vector:");
            this.PrintVector(inputArray, (int)inputArrayRowCount);

            string testArrayPath = this.GetArrayFilePath(1);
            long[,] testArray = InputHandler.GetInput2DArray(testArrayPath);
            long testArrayRowCount = InputHandler.GetRowCount(testArrayPath);
            Console.WriteLine("Test Vector:");
            this.PrintVector(testArray, (int)testArrayRowCount);

            // Getting qubit count
            double rowSizeOfArray = Convert.ToDouble(inputArray.GetLength(0));
            double columnSizeOfArray = Convert.ToDouble(inputArray.GetLength(1));

            // Calculating qubit count
            int iterations = this.utility.GetIterations();
            
            Console.Write("Enter Threshold: ");
            string userInput = Console.ReadLine().Trim();
            double threshold = !string.IsNullOrEmpty(userInput) && double.TryParse(userInput, out threshold) ? threshold : 1.0;

            // Initiate matching TestArray 0 to Input Array
            // Convert 2D vector to single vector for both Input and Test vectors
            long[] newInputArray = this.GetSingleVectorFromMultiVector(inputArray, inputArrayRowCount);
            long[] newTestArray = this.GetSingleVectorFromMultiVector(testArray, inputArrayRowCount);
            double expectedDotProduct = this.utility.DotProduct(newInputArray, newTestArray);
            double maxExpectedProduct = rowSizeOfArray * rowSizeOfArray;

            double dotproduct = this.quantumPerceptronComputeHandler.Compute(newInputArray, newTestArray, iterations);
            double computedRatio = dotproduct / maxExpectedProduct;
            string result =
                threshold < computedRatio ?
                $"[Pattern Match]" :
                $"Pattern didn't match";
            Console.WriteLine($"{result} with Probability of {Math.Round(computedRatio*100, 2)}%\n");
        }

        /// <summary>
        /// Method to get run time path to input and test image vector
        /// from user in .txt format. The method validates if the file exists or not
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetArrayFilePath(int type)
        {
            string message = string.Empty;

            switch (type) {
                case 0:
                    message = "Input Base Image Vector path in [path/{somename}.txt] format: ";
                    break;
                case 1:
                    message = "Input Test Image Vector in [path/{somename}.txt] format: ";
                    break;
                default:
                    return string.Empty;
            }

            Console.Write(message);
            string userInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(userInput) &&
                File.Exists(userInput))
            {
                return userInput;
            }
            else
            {
                Console.WriteLine("Incorrect path supplied or File Does not exist..!!");
                return string.Empty;
            }
        }

        /// <summary>
        /// Method to convert 2D array to single dimention conserving
        /// the original values form the master input
        /// </summary>
        /// <param name="inputArray"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        private long[] GetSingleVectorFromMultiVector(long[,] inputArray, double rowCount)
        {
            long[] newArray = new long[Convert.ToInt32(inputArray.GetLength(0) * rowCount)];
            int row = 0;
            int index = 0;

            while (row < rowCount)
            {
                int column = 0;
                while (column < inputArray.GetLength(0))
                {
                    newArray[index] = inputArray[row, column];
                    column++;
                    index++;
                }

                row++;
            }

            return newArray;
        }

        /// <summary>
        /// Method to print vector
        /// </summary>
        /// <param name="tempTestArray"></param>
        private void PrintVector(long[,] tempTestArray, int n)
        {
            int i = 0;
            while (i < n)
            {
                int j = 0;
                while (j < n)
                {
                    Console.Write($"{tempTestArray[i, j]}\t");
                    j++;
                }

                i++;
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Method to get Test Array from User
        /// </summary>
        /// <returns></returns>
        private long[,] GetTestArray()
        {
            try
            {
                Console.WriteLine("Select Sample Pattern [1], [2] or [3] or [Enter] to exit");
                int input = 0;
                while (true)
                {
                    string userInput = Console.ReadLine().Trim();
                    if (string.IsNullOrEmpty(userInput) || int.TryParse(userInput, out input))
                    {
                        switch (input)
                        {
                            case 1:
                                return InputHandler.GetInput2DArray(Constants.TESTSAMPLE_FILEPATH0);
                            case 2:
                                return InputHandler.GetInput2DArray(Constants.TESTSAMPLE_FILEPATH1);
                            case 3:
                                return InputHandler.GetInput2DArray(Constants.TESTSAMPLE_FILEPATH2);
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Utility method to get row vector from 2 dimensional matrix
        /// provided unique index of row being queried
        /// </summary>
        /// <param name="matrix">2D Matrix</param>
        /// <param name="rowNumber">Row Index</param>
        /// <returns>Row Vector</returns>
        private long[] GetRow(long[,] matrix, int rowNumber)
            => Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
    }
}