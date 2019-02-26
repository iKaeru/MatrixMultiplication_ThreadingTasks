using System;
using NUnit.Framework;

namespace MatrixMultiplication.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void EmptyResult_EmptyMatrices()
        {
            var array1 = new double[0, 0];
            var array2 = new double[0, 0];

            Assert.IsEmpty(MatrixOperations.MultiplicateMatrix(array1, array2));
            Assert.IsEmpty(MatrixOperations.SmartMultiplicateMatrix(array1, array2));
        }

        [Test]
        public void NullException_NullFirstArgument()
        {
            double[,] array1 = null;
            var array2 = new double[0, 0];

            Assert.Throws<ArgumentNullException>(() => MatrixOperations.MultiplicateMatrix(array1, array2));
            Assert.Throws<ArgumentNullException>(() => MatrixOperations.SmartMultiplicateMatrix(array1, array2));
        }

        [Test]
        public void NullException_NullSecondArgument()
        {
            var array1 = new double[0, 0];
            double[,] array2 = null;

            Assert.Throws<ArgumentNullException>(() => MatrixOperations.MultiplicateMatrix(array1, array2));
            Assert.Throws<ArgumentNullException>(() => MatrixOperations.SmartMultiplicateMatrix(array1, array2));
        }

        [Test]
        public void CorrectResult_VerticalMatricesOfInts()
        {
            var array1 = new[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };
            var array2 = new[,]
            {
                {1, 0},
                {1, 1},
                {0, 1}
            };
            var result = new[,]
            {
                {3, 5},
                {9, 11},
                {15, 17}
            };

            Assert.AreEqual(result, MatrixOperations.MultiplicateMatrix(array1, array2));
            Assert.AreEqual(result, MatrixOperations.SmartMultiplicateMatrix(array1, array2));
        }
        
        [Test]
        public void CorrectResult_HorizontalMatricesOfInts()
        {
            var array1 = new[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };
            var array2 = new[,]
            {
                {1, 0, 1, 2},
                {1, 1, 3, 4},
                {0, 1, 3, 1}
            };
            var result = new[,]
            {
                {3, 5, 16, 13},
                {9, 11, 37, 34},
                {15, 17, 58, 55}
            };

            Assert.AreEqual(result, MatrixOperations.MultiplicateMatrix(array1, array2));
            Assert.AreEqual(result, MatrixOperations.SmartMultiplicateMatrix(array1, array2));
        }

        [Test]
        public void CorrectResult_SameSizeMatricesOfInts()
        {
            var array1 = new[,]
            {
                {1, 2},
                {4, 5},
            };
            var array2 = new[,]
            {
                {1, 0},
                {1, 1}
            };
            var result = new[,]
            {
                {3, 2},
                {9, 5}
            };

            Assert.AreEqual(result, MatrixOperations.MultiplicateMatrix(array1, array2));
            Assert.AreEqual(result, MatrixOperations.SmartMultiplicateMatrix(array1, array2));
        }

        [Test]
        public void CorrectResult_SameSizeMatricesOfDoubles()
        {
            var array1 = new[,]
            {
                {2.1, 3.2},
                {4.4, 0.3},
            };
            var array2 = new[,]
            {
                {0.2, 0.1},
                {1.2, 0.21}
            };
            var result = new[,]
            {
                {4.26, 0.882},
                {1.24, 0.503}
            };

            var calculatedMatrix = MatrixOperations.MultiplicateMatrix(array1, array2);
            var smartCalculatedMatrix = MatrixOperations.SmartMultiplicateMatrix(array1, array2);
            
            AssertMatrixElementsWithDelta(result, calculatedMatrix);
            AssertMatrixElementsWithDelta(result, smartCalculatedMatrix);
        }

        private void AssertMatrixElementsWithDelta(double[,] expected, double[,] actual)
        {
            for (int i = 0; i < expected.GetLength(0); i++)
            {
                for (int j = 0; j < expected.GetLength(1); j++)
                {
                    Assert.AreEqual(expected[i, j], actual[i, j], 1e-7);
                }
            }
        }
        
        [Test]
        public void CorrectResult_ParallelMultiplicate_SameSizeMatricesOfInts()
        {
            var array1 = new[,]
            {
                {1, 2},
                {4, 5},
            };
            var array2 = new[,]
            {
                {1, 0},
                {1, 1}
            };
            var result = new[,]
            {
                {3, 2},
                {9, 5}
            };

            Assert.AreEqual(result, MatrixOperations.ParallelMultiplicateMatrix(array1, array2));
            Assert.AreEqual(result, MatrixOperations.SmartParallelMultiplicateMatrix(array1, array2));
        }
        
        [Test]
        public void CorrectResult_ParallelMultiplicate_VerticalMatricesOfInts()
        {
            var array1 = new[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };
            var array2 = new[,]
            {
                {1, 0},
                {1, 1},
                {0, 1}
            };
            var result = new[,]
            {
                {3, 5},
                {9, 11},
                {15, 17}
            };

            Assert.AreEqual(result, MatrixOperations.ParallelMultiplicateMatrix(array1, array2));
            Assert.AreEqual(result, MatrixOperations.SmartParallelMultiplicateMatrix(array1, array2));
        }
        
        [Test]
        public void CorrectResult_ParallelMultiplicate_HorizontalMatricesOfInts()
        {
            var array1 = new[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };
            var array2 = new[,]
            {
                {1, 0, 1, 2},
                {1, 1, 3, 4},
                {0, 1, 3, 1}
            };
            var result = new[,]
            {
                {3, 5, 16, 13},
                {9, 11, 37, 34},
                {15, 17, 58, 55}
            };

            Assert.AreEqual(result, MatrixOperations.ParallelMultiplicateMatrix(array1, array2));
            Assert.AreEqual(result, MatrixOperations.SmartParallelMultiplicateMatrix(array1, array2));
        }
    }
}