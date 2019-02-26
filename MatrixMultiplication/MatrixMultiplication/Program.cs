using System;
using System.Diagnostics;

namespace MatrixMultiplication
{
    class Program
    {
        private static void MakeExperiment<T>(Func<int, int, T[,]> generator,
            Func<T[,], T[,], T[,]> multiplicator, Func<T[,], T[,], T[,]> parallelMultiplicator,
            int matrix1Width, int matrix1Height, int matrix2Width, int matrix2Height)
            where T : struct, IFormattable
        {
            var watch = new Stopwatch();
            var squareMatrixOfInts1 = generator(matrix1Width, matrix1Height);
            var squareMatrixOfInts2 = generator(matrix2Width, matrix2Height);
            watch.Start();
            multiplicator(squareMatrixOfInts1, squareMatrixOfInts2);
            watch.Stop();
            var result1 = watch.ElapsedMilliseconds;
            Console.WriteLine($"Multiplicate of matrix size {matrix1Width}x{matrix2Height} " +
                              $"take:\n{result1} ms");
            watch.Restart();
            parallelMultiplicator(squareMatrixOfInts1, squareMatrixOfInts2);
            watch.Stop();
            var result2 = watch.ElapsedMilliseconds;
            Console.WriteLine($"PARALLEL Multiplicate with 2 Threads of matrix size {matrix1Width}x{matrix2Height} " +
                              $"take:\n{result2} ms");
            watch.Reset();
            var percent = (result1 - result2) * 100 / result1;
            Console.WriteLine($"What is faster on {percent}% than regular multiplication");
            Console.WriteLine("===== Experiment Over =====");
            Console.WriteLine();
        }

        static void Main()
        {
            Console.WriteLine("Multiplicate matrices of ints\n");
            MakeExperiment(MatrixGenerator.GenerateRectangleMatrixOfInts,
                MatrixOperations.MultiplicateMatrix, MatrixOperations.ParallelMultiplicateMatrix,
                300, 300, 300, 300);
            
            Console.WriteLine("Multiplicate matrices of doubles\n");
            MakeExperiment(MatrixGenerator.GenerateRectangleMatrixOfDoubles,
                MatrixOperations.MultiplicateMatrix, MatrixOperations.ParallelMultiplicateMatrix,
                200, 100, 100, 200);
            
            Console.WriteLine("Smart multiplicate matrices of doubles\n");
            MakeExperiment(MatrixGenerator.GenerateRectangleMatrixOfDoubles,
                MatrixOperations.SmartMultiplicateMatrix, MatrixOperations.SmartParallelMultiplicateMatrix,
                200, 100, 100, 200);
        }
    }
}