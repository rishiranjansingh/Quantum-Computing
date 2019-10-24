// <copyright file="ImageBitsConversion.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

using PQC.Input;
using System;
using System.IO;
using System.Linq;

namespace PQC.Functional.ImageClassifier
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageBitsConversion
    {
        public int imageSize;
        public int filterSize;
        public long[,] filterBinaryMatrix;
        public long[,] imageMatrix;

        /// <summary>
        /// 
        /// </summary>
        public void ProcessInput()
        {
            imageMatrix = InputHandler.GetInput2DArray("image.txt");
            filterBinaryMatrix = InputHandler.GetInput2DArray("filter.txt");
            imageSize = imageMatrix.GetLength(0);
            filterSize = filterBinaryMatrix.GetLength(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxtrix"></param>
        /// <param name="startRow"></param>
        /// <param name="startCol"></param>
        /// <returns></returns>
        public long[] convert2DArrayTo1DArray(long[,] maxtrix, int startRow, int startCol)
        {
            long[] array = new long[filterSize * filterSize];
            int arrayIndex = 0;
            for (int i = startRow; i < startRow + filterSize; i++)
            {
                for (int j = startCol; j < startCol + filterSize; j++)
                {
                    array[arrayIndex++] = maxtrix[i, j];
                }
            }
            return array;
        }
    }
}