// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using Microsoft.Quantum.Simulation.Core;
using PQC.Enumerators;
using PQC.Functional;
using PQC.Functional.ComputeManager;
using PQC.Functional.UtilityManager;
using PQC.Input;
using System;
using System.Collections.Generic;

namespace Quantum.PQC
{
    /// <summary>
    /// Perceptron Driver Class
    /// </summary>
    public class PerceptronDriver: BaseDriver
    {
        private readonly IQuantumPerceptronComputeHandler quantumPerceptronComputeHandler;
        private readonly IUtility utility;

        /// <summary>
        /// Perceptron Driver Constructor
        /// </summary>
        public PerceptronDriver()
        {
            Console.WriteLine("Processing Perception Driver...");
            this.quantumPerceptronComputeHandler = new QuantumPerceptronComputeHandler();
            this.utility = new Utility();
        }

        /// <summary>
        /// Method initializing the perceptron process
        /// </summary>
        public override void Initialize()
        {
            try
            {
                this.UsingHyperGraph();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Method to get HyperGraph built
        /// </summary>
        /// <param name="input">input vector</param>
        /// <param name="n">number of qubits</param>
        /// <returns>QArray of QArray</returns>
        public QArray<QArray<long>> GetHyperGraph(long[] input, int n)
        {
            Dictionary<long, Dictionary<long, List<long>>> preComputedOnesListDictionary = this.quantumPerceptronComputeHandler.CreateOnesList(n);
            List<List<long>> listCPZ = this.quantumPerceptronComputeHandler.CalculateInputZsList(input, preComputedOnesListDictionary);
            QArray<QArray<long>> hyperGraph = QArray<QArray<long>>.Create(listCPZ.Count);
            int i = 0;

            foreach (List<long> l in listCPZ)
            {
                QArray<long> tempL = new QArray<long>(l);
                hyperGraph.Modify(i++, tempL);
            }

            return hyperGraph;
        }

        /// <summary>
        /// Perceptron initialization block
        /// </summary>
        private void UsingHyperGraph()
        {
            long[] inputVector = InputHandler.GetInputArray(InputTypeEnum.File);
            long[] weightVector = InputHandler.GetWeightArray(InputTypeEnum.File);
            
            Console.WriteLine($"Expected dot Product: {this.utility.DotProduct(inputVector, weightVector)}");
            double dotproduct = this.quantumPerceptronComputeHandler.Compute(inputVector, weightVector);
            Console.WriteLine($"Dot product computed using quantum perceptron: {dotproduct}\n\n");
        }
    }
}