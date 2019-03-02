using System;
using System.Threading.Tasks;

namespace MatrixMultiplication
{
    public static class MatrixOperations<T>
    {
        private static readonly MathProvider<T> Math;

        static MatrixOperations()
        {
            if (typeof(T) == typeof(double))
                Math = new DoubleMathProvider() as MathProvider<T>;
            else if (typeof(T) == typeof(int))
                Math = new IntMathProvider() as MathProvider<T>;
            
            if (Math == null)
                throw new InvalidOperationException(
                    "Type " + typeof(T) + " is not supported by MatrixOperations.");
        }

        private static void MultiplicateElements(T[,] matrix1, T[,] matrix2, T[,] matrixResult,
            (int, int) firstCycle)
        {
            for (var i = firstCycle.Item1; i < firstCycle.Item2; i++)
            {
                for (var j = 0; j < matrix2.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix1.GetLength(1); k++)
                    {
                        matrixResult[i, j] = Math.Add(matrixResult[i, j],
                            Math.Multiply(matrix1[i, k], matrix2[k, j]));
                    }
                }
            }
        }

        private static void SmartMultiplicateElements(T[,] matrix1, T[,] matrix2, T[,] matrixResult,
            (int, int) firstCycle)
        {
            for (var i = firstCycle.Item1; i < firstCycle.Item2; i++)
            {
                for (var k = 0; k < matrix1.GetLength(1); k++)
                {
                    for (var j = 0; j < matrix2.GetLength(1); j++)
                    {
                        matrixResult[i, j] = Math.Add(matrixResult[i, j],
                            Math.Multiply(matrix1[i, k], matrix2[k, j]));
                    }
                }
            }
        }

        public static T[,] MultiplicateMatrix(T[,] matrix1, T[,] matrix2)
        {
            CheckInput(matrix1, matrix2);

            var array1RowsAmount = matrix1.GetLength(0);
            var result = new T[array1RowsAmount, matrix2.GetLength(1)];

            MultiplicateElements(matrix1, matrix2, result, (0, array1RowsAmount));

            return result;
        }

        public static T[,] SmartMultiplicateMatrix(T[,] matrix1, T[,] matrix2)
        {
            CheckInput(matrix1, matrix2);

            var array1RowsAmount = matrix1.GetLength(0);
            var result = new T[array1RowsAmount, matrix2.GetLength(1)];

            SmartMultiplicateElements(matrix1, matrix2, result, (0, array1RowsAmount));

            return result;
        }

        public static T[,] ParallelMultiplicateMatrix(T[,] matrix1, T[,] matrix2)
        {
            CheckInput(matrix1, matrix2);

            return ParallelOperations(matrix1, matrix2, MultiplicateElements);
        }

        public static T[,] SmartParallelMultiplicateMatrix(T[,] matrix1, T[,] matrix2)
        {
            CheckInput(matrix1, matrix2);

            return ParallelOperations(matrix1, matrix2, SmartMultiplicateElements);
        }

        private static T[,] ParallelOperations(T[,] matrix1, T[,] matrix2,
            Action<T[,], T[,], T[,], (int, int)> multiplicator)
        {
            var array1RowsAmount = matrix1.GetLength(0);
            var result = new T[array1RowsAmount, matrix2.GetLength(1)];
            var tasksCount = Environment.ProcessorCount;
            var tasks = new Task[tasksCount];
            var rowsForTask = array1RowsAmount / tasksCount;

            for (int n = 0; n < tasksCount; n++)
            {
                var lowerBorder = n * rowsForTask;
                var upperBorder = (n + 1) * rowsForTask;
                if (n == tasksCount - 1)
                {
                    upperBorder += array1RowsAmount % tasksCount;
                }

                tasks[n] = Task.Run(() =>
                {
                    multiplicator(matrix1, matrix2, result, (lowerBorder, upperBorder));
                });
            }

            Task.WaitAll(tasks);
            return result;
        }

        private static void CheckInput(T[,] matrix1, T[,] matrix2)
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

        public static void PrintMatrix(T[,] matrix)
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
    }
}