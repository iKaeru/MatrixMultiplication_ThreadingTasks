using System;
using System.Threading.Tasks;

namespace MatrixMultiplication
{
    public static class MatrixOperations
    {
        private static void MultiplicatePartOfMatrix<T>(T[,] matrix1, T[,] matrix2, T[,] matrixResult,
            (int, int) firstCycle, (int, int) secondCycle, (int, int) thirdCycle)
            where T : struct, IFormattable
        {
            for (var i = firstCycle.Item1; i < firstCycle.Item2; i++)
            {
                for (var j = secondCycle.Item1; j < secondCycle.Item2; j++)
                {
                    for (int k = thirdCycle.Item1; k < thirdCycle.Item2; k++)
                    {
                        matrixResult[i, j] = Addition(matrixResult[i, j],
                            Multiplication(matrix1[i, k], matrix2[k, j]));
                    }
                }
            }
        }

        private static void SmartMultiplicatePartOfMatrix<T>(T[,] matrix1, T[,] matrix2, T[,] matrixResult,
            (int, int) firstCycle, (int, int) secondCycle, (int, int) thirdCycle)
            where T : struct, IFormattable
        {
            for (var i = firstCycle.Item1; i < firstCycle.Item2; i++)
            {
                for (var k = secondCycle.Item1; k < secondCycle.Item2; k++)
                {
                    for (var j = thirdCycle.Item1; j < thirdCycle.Item2; j++)
                    {
                        matrixResult[i, j] = Addition(matrixResult[i, j],
                            Multiplication(matrix1[i, k], matrix2[k, j]));
                    }
                }
            }
        }

        public static T[,] MultiplicateMatrix<T>(T[,] matrix1, T[,] matrix2)
            where T : struct, IFormattable
        {
            CheckInput(matrix1, matrix2);

            var array1RowsAmount = matrix1.GetLength(0);
            var array2ColumnsAmount = matrix2.GetLength(1);
            var result = new T[array1RowsAmount, array2ColumnsAmount];

            MultiplicatePartOfMatrix(matrix1, matrix2, result, (0, array1RowsAmount),
                (0, array2ColumnsAmount), (0, matrix1.GetLength(1)));

            return result;
        }

        public static T[,] SmartMultiplicateMatrix<T>(T[,] matrix1, T[,] matrix2)
            where T : struct, IFormattable
        {
            CheckInput(matrix1, matrix2);

            var array1RowsAmount = matrix1.GetLength(0);
            var array2ColumnsAmount = matrix2.GetLength(1);
            var result = new T[array1RowsAmount, array2ColumnsAmount];

            SmartMultiplicatePartOfMatrix(matrix1, matrix2, result, (0, array1RowsAmount),
                (0, matrix1.GetLength(1)), (0, array2ColumnsAmount));

            return result;
        }

        public static T[,] ParallelMultiplicateMatrix<T>(T[,] matrix1, T[,] matrix2)
            where T : struct, IFormattable
        {
            CheckInput(matrix1, matrix2);

            var array1RowsAmount = matrix1.GetLength(0);
            var array2ColumnsAmount = matrix2.GetLength(1);
            var result = new T[array1RowsAmount, array2ColumnsAmount];
            Task task1;
            Task task2;
            var thirdCycle = (0, matrix1.GetLength(1));

            if (array1RowsAmount > array2ColumnsAmount)
            {
                task1 = Task.Run(() => MultiplicatePartOfMatrix(matrix1, matrix2, result, (0, array1RowsAmount),
                    (0, array2ColumnsAmount / 2), thirdCycle));
                task2 = Task.Run(() => MultiplicatePartOfMatrix(matrix1, matrix2, result, (0, array1RowsAmount),
                    (array2ColumnsAmount / 2, array2ColumnsAmount), thirdCycle));
            }
            else
            {
                task1 = Task.Run(() => MultiplicatePartOfMatrix(matrix1, matrix2, result, (0, array1RowsAmount / 2),
                    (0, array2ColumnsAmount), thirdCycle));
                task2 = Task.Run(() => MultiplicatePartOfMatrix(matrix1, matrix2, result,
                    (array1RowsAmount / 2, array1RowsAmount),
                    (0, array2ColumnsAmount), thirdCycle));
            }

            task1.Wait();
            task2.Wait();

            return result;
        }

        public static T[,] SmartParallelMultiplicateMatrix<T>(T[,] matrix1, T[,] matrix2)
            where T : struct, IFormattable
        {
            CheckInput(matrix1, matrix2);

            var array1RowsAmount = matrix1.GetLength(0);
            var array2ColumnsAmount = matrix2.GetLength(1);
            var result = new T[array1RowsAmount, array2ColumnsAmount];
            Task task1;
            Task task2;
            var thirdCycle = (0, array2ColumnsAmount);
            var matrix1Columns = matrix1.GetLength(1);

            if (array1RowsAmount > array2ColumnsAmount)
            {
                task1 = Task.Run(() => SmartMultiplicatePartOfMatrix(matrix1, matrix2, result, (0, array1RowsAmount),
                    (0, matrix1Columns / 2), thirdCycle));
                task2 = Task.Run(() => SmartMultiplicatePartOfMatrix(matrix1, matrix2, result, (0, array1RowsAmount),
                    (matrix1Columns / 2, matrix1Columns), thirdCycle));
            }
            else
            {
                task1 = Task.Run(() => SmartMultiplicatePartOfMatrix(matrix1, matrix2, result,
                    (0, array1RowsAmount / 2),
                    (0, matrix1Columns), thirdCycle));
                task2 = Task.Run(() => SmartMultiplicatePartOfMatrix(matrix1, matrix2, result,
                    (array1RowsAmount / 2, array1RowsAmount),
                    (0, matrix1Columns), thirdCycle));
            }

            task1.Wait();
            task2.Wait();

            return result;
        }

        private static void CheckInput<T>(T[,] matrix1, T[,] matrix2)
        {
            if (matrix1 == null)
            {
                throw new ArgumentNullException(nameof(matrix1));
            }

            if (matrix2 == null)
            {
                throw new ArgumentNullException(nameof(matrix2));
            }
        }

        public static void PrintMatrix<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }

                Console.WriteLine();
            }
        }

        private static T Multiplication<T>(T firstNumber, T secondNumber) where T : struct
        {
            return (dynamic) firstNumber * (dynamic) secondNumber;
        }

        private static T Addition<T>(T firstNumber, T secondNumber) where T : struct
        {
            return (dynamic) firstNumber + (dynamic) secondNumber;
        }
    }
}