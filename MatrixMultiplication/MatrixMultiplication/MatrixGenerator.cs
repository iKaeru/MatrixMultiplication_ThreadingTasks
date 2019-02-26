using System;

namespace MatrixMultiplication
{
    public class MatrixGenerator
    {        
        public static int[,] GenerateRectangleMatrixOfInts(int rowsCount, int columnsCount)
        {
            var result = new int[rowsCount, columnsCount];
            var rand = new Random();
            
            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    result[i, j] = rand.Next(100);
                }
            }

            return result;
        }
        
        public static double[,] GenerateRectangleMatrixOfDoubles(int rowsCount, int columnsCount)
        {
            var result = new double[rowsCount, columnsCount];
            var rand = new Random();
            
            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    result[i, j] = rand.NextDouble()*100;
                }
            }

            return result;
        }
    }
}