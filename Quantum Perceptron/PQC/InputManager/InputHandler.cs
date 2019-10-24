// <copyright file="Driver.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using PQC.Enumerators;
using System;

namespace PQC.Input
{
    public class InputHandler
    {
        /// <summary>
        /// Method to get input vector row by row
        /// </summary>
        /// <returns></returns>
        public static int GetQubitCount(InputTypeEnum inputType)
        {
            try
            {
                switch (inputType)
                {
                    case InputTypeEnum.File:
                        return new FileHandler().GetQubitCount();
                    case InputTypeEnum.Service:
                        return new ServiceInputHandler().GetQubitCount();
                    case InputTypeEnum.Database:
                        return new DatabaseInputHandler().GetQubitCount();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// Method to get input vector row by row
        /// </summary>
        /// <returns></returns>
        public static long[] GetInputArray(InputTypeEnum inputType)
        {
            try
            {
                switch (inputType)
                {
                    case InputTypeEnum.File:
                        return new FileHandler().GetInput();
                    case InputTypeEnum.Service:
                        return new ServiceInputHandler().GetInput();
                    case InputTypeEnum.Database:
                        return new DatabaseInputHandler().GetInput();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Method to get input vector row by row
        /// </summary>
        /// <returns></returns>
        public static long[] GetWeightArray(InputTypeEnum inputType)
        {
            try
            {
                switch (inputType)
                {
                    case InputTypeEnum.File:
                        return new FileHandler().GetWeight();
                    case InputTypeEnum.Service:
                        return new ServiceInputHandler().GetWeight();
                    case InputTypeEnum.Database:
                        return new DatabaseInputHandler().GetWeight();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Method to get input vector in 2D array
        /// </summary>
        /// <returns></returns>
        public static long[,] GetInput2DArray(string path)
        {
            try
            {
                string[] inputVector = new FileHandler().GetRows(path);

                // Get string length for each row
                int size = inputVector[0].Length;

                // Convert to Long and 0 to -1
                long[,] convertedArray = new long[size, size];
                int i = 0;

                foreach (string input in inputVector)
                {
                    // Get each character from string and convert it into -1/+1 format
                    // Only when there's value in the diagnosed flowing
                    for (int j = 0; j < input.Length; j++)
                    {
                        if (input[j] == '0')
                        {
                            convertedArray[i, j] = -1;
                        }
                        else if (input[j] == '1')
                        {
                            convertedArray[i, j] = 1;
                        }

                    }
                    i++;
                }

                return convertedArray;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Method to get Count of Rows in Input Vector
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetRowCount(string path)
            => new FileHandler().GetRows(path).Length;
    }
}
