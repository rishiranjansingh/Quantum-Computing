// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace PQC.Functional.UtilityManager
{
    using System;
    using System.Linq;

    /// <summary>
    /// Utility Class
    /// </summary>
    public class Utility: IUtility
    {
        /// <summary>
        /// Method to get Iteration from user input or as default
        /// </summary>
        /// <returns></returns>
        public int GetIterations()
        {
            Console.Write(
                "Enter Number of Iterations you want to run\n" +
                "[Default value of 2048 will be taken if left blank]\n" +
                "Ans: ");
            string input = Console.ReadLine().Trim();

            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int iterations))
            {
                return iterations;
            }
            else
            {
                // Returning default if no valid user input is found
                return Constants.DEFAULT_ITERATIONS;
            }
        }


        /// <summary>
        /// Method to calculate DotProduct on classical node for comparison
        /// </summary>
        /// <param name="inputVector"></param>
        /// <param name="weightVector"></param>
        /// <returns>Dot Product of the Input Vectors</returns>
        public double DotProduct(long[] inputVector, long[] weightVector)
        {
            long prod = 0;

            for (int i = 0; i < inputVector.Length; i++)
            {
                prod += inputVector[i] * weightVector[i];
            }

            return prod;
        }

        /// <summary>
        /// Method to get 1 Column by index from a 2D Matrix
        /// </summary>
        /// <typeparam name="T">Column Type</typeparam>
        /// <param name="matrix">2D Matrix</param>
        /// <param name="columnNumber">Column Index to fetch</param>
        /// <returns>Column Vector</returns>
        public T[] GetColumn<T>(T[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();
        }
    }
}
