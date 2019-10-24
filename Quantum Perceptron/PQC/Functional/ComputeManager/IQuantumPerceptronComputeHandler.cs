// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace PQC.Functional.ComputeManager
{
    /// <summary>
    /// Quantum Perceptron Computer Handler Interface
    /// </summary>
    public interface IQuantumPerceptronComputeHandler
    {
        /// <summary>
        /// Method to help compute input vector provided weight vector and qubit count
        /// for the provided iterations or defaulted to 2048 units and get dotproduct back
        /// from Quantum Computer. The result is probabilistic and accuracy depends upon the
        /// number of the times the code has been executed controlled by iteration units
        /// </summary>
        /// <param name="inputVector">Perceptron Input Vector in -1/+1</param>
        /// <param name="weightVector">Perceptron Weigh Vector in -1/+1</param>
        /// <param name="qubitCount">Qubit count required to execute the perceptron</param>
        /// <param name="iterations">Number of times the iteration has to execute to get better probablity value</param>
        /// <returns></returns>
        double Compute(long[] inputVector, long[] weightVector, int? iterations = null);

        /// <summary>
        /// Method to calculate Controlled Z indexes based on the input vector
        /// </summary>
        /// <param name="inputVector"></param>
        /// <param name="preComputedOnesListDictionary"></param>
        /// <returns></returns>
        List<List<long>> CalculateInputZsList(long[] inputVector, Dictionary<long, Dictionary<long, List<long>>> preComputedOnesListDictionary);

        /// <summary>
        /// Method to retrieve Ones list
        /// </summary>
        /// <param name="qubitCount"></param>
        /// <returns></returns>
        Dictionary<long, Dictionary<long, List<long>>> CreateOnesList(int qubitCount);
    }
}