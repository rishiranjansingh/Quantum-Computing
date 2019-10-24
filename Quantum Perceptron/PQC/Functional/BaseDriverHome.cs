// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using PQC.Functional.ImageClassifier;
using PQC.Functional.PatternRecognition;
using Quantum.PQC;
using System;

namespace PQC.Functional
{
    /// <summary>
    /// Home Base Driver class for abstract interface to
    /// process initialization based on user input
    /// providing dynamic object initialization.
    /// </summary>
    public class BaseDriverHome
    {
        /// <summary>
        /// Abstract method to get Specific Concrete Object base
        /// on user choice entered. You can further instantiate the
        /// inner object process by calling [object].Initialize()
        /// </summary>
        /// <param name="choice"></param>
        public BaseDriver GetObjectByChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    return new PerceptronDriver();
                case 2:
                    return new DenseRegionComputation();
                case 3:
                    return new PatternRecognitionHandler();
                default:
                    Console.WriteLine("Incorrect choice...");
                    return null;
            }
        }
    }
}
