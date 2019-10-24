// <copyright file="DenseRegionComputation.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace PQC.Functional.ImageClassifier
{
    using Microsoft.Quantum.Simulation.Core;
    using Microsoft.Quantum.Simulation.Simulators;
    using PQC.Functional.ComputeManager;
    using PQC.Functional.UtilityManager;
    using Quantum.PQC;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Dense Region Computation Class
    /// </summary>
    public class DenseRegionComputation: BaseDriver
    {
        private readonly IQuantumPerceptronComputeHandler quantumPerceptronComputeHandler;
        private readonly IUtility utility;

        /// <summary>
        /// Dense Region Computation Constructor for initialization
        /// </summary>
        public DenseRegionComputation()
        {
            Console.WriteLine("Dense Region Computation Driver...");
            this.quantumPerceptronComputeHandler = new QuantumPerceptronComputeHandler();
            this.utility = new Utility();
        }

        public override void Initialize()
        {
            ImageBitsConversion ibc = new ImageBitsConversion();
            ibc.ProcessInput();
            int n = ibc.imageSize;
            int m = ibc.filterSize;

            List<long> weightVectorList = new List<long>();

            long[] weightVector = ibc.convert2DArrayTo1DArray(ibc.filterBinaryMatrix, 0, 0);
            int weightVectorLength = weightVector.Length;
            AddPadddingBits(weightVectorList, weightVectorLength, weightVector);
            PerceptronDriver perceptronDriver = new PerceptronDriver();
            long[] wtArray = weightVectorList.ToArray();
            QArray<QArray<long>> weightHyperGraph = perceptronDriver.GetHyperGraph(wtArray, m + 1);
            InvertActualWeightVector(weightVectorList, weightVectorLength);
            QArray<QArray<long>> weightHyperGraphInverted = perceptronDriver.GetHyperGraph(weightVectorList.ToArray(), m + 1);
            int resRowIndex, resColIndex;
            resRowIndex = resColIndex = 0;
            double dotProduct = double.MinValue;

            for (int i = 0; i < n - m + 1; i++)
            {
                for (int j = 0; j < n - m + 1; j++)
                {
                    long[] inputVector = ibc.convert2DArrayTo1DArray(ibc.imageMatrix, i, j);
                    List<long> inputVectorList = new List<long>();
                    AddPadddingBits(inputVectorList, weightVectorLength, inputVector);
                    this.utility.DotProduct(inputVector, weightVector);
                    QArray<QArray<long>> inputHyperGraph = perceptronDriver.GetHyperGraph(inputVectorList.ToArray(), m + 1);
                    double tempDotProduct, tempDotProductInverted;

                    using (var qsim = new QuantumSimulator())
                    {
                        long countOne = Perceptron_PCZ.Run(
                            qsim,
                            inputHyperGraph,
                            weightHyperGraph,
                            m + 1).Result;

                        double cm = Math.Sqrt((double)countOne / (double)8192);
                        tempDotProduct = (1 << (m + 1)) * cm;
                    }

                    using (var qsim = new QuantumSimulator())
                    {
                        long countOne = Perceptron_PCZ.Run(
                            qsim,
                            inputHyperGraph,
                            weightHyperGraphInverted,
                            m + 1).Result;

                        double cm = Math.Sqrt((double)countOne / (double)8192);
                        tempDotProductInverted = (1 << (m + 1)) * cm;
                    }

                    double dotProductOfThisIp = (tempDotProduct - tempDotProductInverted) / 2.0;

                    if (dotProduct < dotProductOfThisIp)
                    {
                        resRowIndex = i;
                        resColIndex = j;
                        dotProduct = dotProductOfThisIp;
                    }

                    Console.WriteLine(i + "," + j);
                    Console.WriteLine("Dot Product: " + dotProductOfThisIp);
                }
            }
            Console.WriteLine("\nDense Region Coordinates: " + resRowIndex + " " + resColIndex);
            Console.WriteLine("Dot Product of Dense Region: " + dotProduct);
            Console.WriteLine("Dense Region");
            for (int i = resRowIndex; i < resRowIndex + m; i++)
            {
                for (int j = resColIndex; j < resColIndex + m; j++)
                {
                    Console.Write(ibc.imageMatrix[i, j] == 1 ? 1 : 0);
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        private void AddPadddingBits(List<long> vector, int n, long[] anyVector)
        {
            for (int i = 0; i < n; i++)
            {
                vector.Add(1);
            }
            for (int i = 0; i < n; i++)
            {
                vector.Add(anyVector[i]);
            }
        }

        private void InvertActualWeightVector(List<long> vector, int n)
        {
            for (int i = n; i < 2 * n; i++)
            {
                vector[i] = -1 * vector[i];
            }
        }
    }
}