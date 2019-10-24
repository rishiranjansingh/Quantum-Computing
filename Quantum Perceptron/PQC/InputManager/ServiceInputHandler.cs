// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using System;

namespace PQC.Input
{
    /// <summary>
    /// Input Handler Class to get perceptron input vector from service endpoint
    /// </summary>
    internal class ServiceInputHandler
    {
        /// <summary>
        /// Service Input handler initialization point
        /// </summary>
        public ServiceInputHandler()
        {
        }

        /// <summary>
        /// Method to get input vector
        /// </summary>
        /// <returns></returns>
        internal long[] GetInput()
        {
            throw new NotImplementedException();
        }

        internal long[] GetWeight()
        {
            throw new NotImplementedException();
        }

        internal int GetQubitCount()
        {
            throw new NotImplementedException();
        }
    }
}