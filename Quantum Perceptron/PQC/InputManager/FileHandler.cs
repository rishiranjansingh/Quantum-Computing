// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using System;
using System.IO;

namespace PQC.Input
{
    /// <summary>
    /// Input Handler from File [input vector]
    /// </summary>
    internal class FileHandler
    {
        /// <summary>
        /// Initializing Input handler class
        /// </summary>
        public FileHandler()
        {
        }

        /// <summary>
        /// Method to get Input Vector
        /// </summary>
        /// <returns>Input Vector</returns>
        internal int GetQubitCount()
        {
            try
            {
                string[] inputFileContent = this.GetFile(Constants.VECTOR_FILEPATH);
                return int.Parse(inputFileContent[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Method to get Input Vector
        /// </summary>
        /// <returns>Input Vector</returns>
        internal long[] GetInput()
        {
            try
            {
                string[] inputFileContent = this.GetFile(Constants.VECTOR_FILEPATH);
                return this.GetRow(inputFileContent, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Method to get all the rows from a text file
        /// provided path in string to the file in bin
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal string[] GetRows(string path)
        {
            try
            {
                // Read File
                return this.GetFile(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Method to get Weight Vector
        /// </summary>
        /// <returns>Weight Vector</returns>
        internal long[] GetWeight()
        {
            try
            {
                string[] inputFileContent = this.GetFile(Constants.VECTOR_FILEPATH);
                return this.GetRow(inputFileContent, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Getting File Content back
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string[] GetFile(string path) => File.ReadAllLines(path);

        /// <summary>
        /// Method to get row from String array of a file by row index
        /// </summary>
        /// <param name="text">Text array</param>
        /// <param name="line">Line Index</param>
        /// <returns>Line Content</returns>
        private long[] GetRow(string[] text, int line) => Array.ConvertAll(text[line].Split(','), long.Parse);
    }
}