// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;
using PQC.Functional.UtilityManager;
using Quantum.PQC;
using System;
using System.Collections.Generic;
using System.Text;

namespace PQC.Functional.ComputeManager
{
    /// <summary>
    /// Quantum Perceptron Compute handler class for functional implementation
    /// </summary>
    public class QuantumPerceptronComputeHandler : IQuantumPerceptronComputeHandler
    {
        private readonly IUtility utility;

        /// <summary>
        /// Class Constructor for instantiation
        /// </summary>
        public QuantumPerceptronComputeHandler()
        {
            this.utility = new Utility();
        }

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
        public double Compute(long[] inputVector, long[] weightVector, int? iterations = null)
        {
            try
            {
                int qubitCount = Convert.ToInt32(Math.Log(inputVector.Length, 2.0));

                // Checking input parameters
                if (inputVector == null || weightVector == null || qubitCount < 1 || inputVector.Length < 1 || weightVector.Length < 1)
                {
                    Console.WriteLine($"Incorrect Input Paramter supplied while calling Compute Function");
                    return 0;
                }

                // Check if iterations are received or prompt user for input
                if (!iterations.HasValue)
                {
                    iterations = this.utility.GetIterations();
                }

                Console.WriteLine($"Running with {iterations} iterations...");

                Console.WriteLine($"Input Vector: {string.Join(",", inputVector)}");
                Console.WriteLine($"Weight Vector: {string.Join(",", weightVector)}");
                Console.WriteLine("------------------------------------");

                DateTime start = DateTime.Now;
                Dictionary<long, Dictionary<long, List<long>>> preComputedOnesListDictionary = CreateOnesList(qubitCount);
                List<List<long>> listInputCPZ = CalculateInputZsList(inputVector, preComputedOnesListDictionary);
                List<List<long>> listWeightCPZ = CalculateInputZsList(weightVector, preComputedOnesListDictionary);

                bool[] inputHyperGraph = ConstructHyperGraph(listInputCPZ, qubitCount);
                bool[] weightHyperGraph = ConstructHyperGraph(listWeightCPZ, qubitCount);

                using (var qsim = new QuantumSimulator())
                {
                    long countOne = Perceptron.Run(
                        qsim,
                        new QArray<bool>(inputHyperGraph),
                        new QArray<bool>(weightHyperGraph),
                        qubitCount,
                        iterations.Value).Result;

                    double cm = Math.Sqrt((double)countOne / (double)iterations.Value);
                    double dotproduct = (1 << qubitCount) * cm;

                    return dotproduct;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0.0;
        }

        /// <summary>
        /// Method to calculate Controlled Z indexes based on the input vector
        /// </summary>
        /// <param name="inputVector">Input Vector for Perceptron in -1/+1</param>
        /// <param name="preComputedOnesListDictionary">Input Weight for Perceptron in -1/+1</param>
        /// <returns></returns>
        public List<List<long>> CalculateInputZsList(long[] inputVector, Dictionary<long, Dictionary<long, List<long>>> preComputedOnesListDictionary)
        {
            if (inputVector[0] == -1)
            {
                for (int i = 0; i < inputVector.Length; i++)
                {
                    inputVector[i] *= -1;
                }
            }

            return CalculateZs(inputVector, preComputedOnesListDictionary);
        }

        /// <summary>
        /// Method to retrieve Ones list
        /// </summary>
        /// <param name="qubitCount"></param>
        /// <returns></returns>
        public Dictionary<long, Dictionary<long, List<long>>> CreateOnesList(int qubitCount)
        {
            long maxNum = (long)Math.Pow(2.0, qubitCount * 1.0);
            long[] binaryNum = new long[qubitCount];

            for (int i = 0; i < qubitCount; i++)
            {
                binaryNum[i] = (long)Math.Pow(2.0, i * 1.0);
            }

            Dictionary<long, Dictionary<long, List<long>>> res = new Dictionary<long, Dictionary<long, List<long>>>();
            for (int i = 1; i < maxNum; i++)
            {
                int numOnes = 0;
                Dictionary<long, List<long>> onesPos = new Dictionary<long, List<long>>();
                List<long> onesTempList = new List<long>();

                for (int j = 0; j < qubitCount; j++)
                {
                    long temp = i & (long)binaryNum[j];
                    if (temp != 0)
                    {
                        onesTempList.Add(j);
                        numOnes++;
                    }
                }

                res.TryGetValue(numOnes, out onesPos);
                if (onesPos == null)
                {
                    onesPos = new Dictionary<long, List<long>>();
                }

                onesPos.Add(i, onesTempList);
                res[numOnes] = onesPos;
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputVector"></param>
        /// <param name="onesPos"></param>
        /// <returns></returns>
        private List<List<long>> CalculateZs(long[] inputVector, Dictionary<long, Dictionary<long, List<long>>> onesPos)
        {
            long[] inProgressVector = new long[inputVector.Length];
            Array.Fill(inProgressVector, 1);
            List<List<long>> result = new List<List<long>>();

            foreach (KeyValuePair<long, Dictionary<long, List<long>>> entry in onesPos)
            {
                foreach (KeyValuePair<long, List<long>> entry1 in entry.Value)
                {
                    if (inputVector[entry1.Key] != inProgressVector[entry1.Key])
                    {
                        List<long> onesPosList = entry1.Value;
                        result.Add(onesPosList);

                        // Flip all the number greater than equal to Decimal number(entry1.key)
                        for (long i = entry1.Key; i < inputVector.Length; i++)
                        {
                            int onesBitCount = 0;
                            for (int j = 0; j < onesPosList.Count; j++)
                            {
                                long tempPow = 1 << (int)onesPosList[j];

                                if ((i & tempPow) > 0)
                                {
                                    onesBitCount++;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (onesBitCount == onesPosList.Count)
                            {
                                inProgressVector[i] *= -1;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCPZ"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private bool[] ConstructHyperGraph(List<List<long>> listCPZ, int n)
        {
            bool[] output = new bool[(1 << n) - 1];

            for (int i = 0; i < ((1 << n) - 1); i++)
            {
                output[i] = false;
            }

            foreach (List<long> cpz in listCPZ)
            {
                int index = -1;

                foreach (int bitIndex in cpz)
                {
                    index += 1 << bitIndex;
                }

                output[index] = true;
            }

            return output;
        }
    }
}