// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using System;
using System.Runtime.Serialization;

namespace Quantum.PQC
{
    [Serializable]
    internal class Exeception : Exception
    {
        public Exeception()
        {
        }

        public Exeception(string message) : base(message)
        {
        }

        public Exeception(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected Exeception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}