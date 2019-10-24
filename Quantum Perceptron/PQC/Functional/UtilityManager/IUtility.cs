// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace PQC.Functional.UtilityManager
{
    /// <summary>
    /// Utility Interface for general purpose computation
    /// </summary>
    public interface IUtility
    {
        /// <summary>
        /// Method to get Iteration from user input or as default
        /// </summary>
        /// <returns></returns>
        int GetIterations();

        /// <summary>
        /// Method to calculate DotProduct on classical node for comparison
        /// </summary>
        /// <param name="inputVector"></param>
        /// <param name="weightVector"></param>
        /// <returns>Dot Product of the Input Vectors</returns>
        double DotProduct(long[] inputVector, long[] weightVector);

        /// <summary>
        /// Method to get 1 Column by index from a 2D Matrix
        /// </summary>
        /// <typeparam name="T">Column Type</typeparam>
        /// <param name="matrix">2D Matrix</param>
        /// <param name="columnNumber">Column Index to fetch</param>
        /// <returns>Column Vector</returns>
        T[] GetColumn<T>(T[,] matrix, int columnNumber);
    }
}
