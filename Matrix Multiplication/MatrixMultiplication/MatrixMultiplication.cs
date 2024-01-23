using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public static class MatrixMultiplication
    {
        static public int[,] MatrixMultiply(int[,] M1, int[,] M2, int N)
        {
            int[,] result = new int[N, N];

            if (N == 1)
            {
                result[0, 0] = M1[0, 0] * M2[0, 0];
                return result;
            }

            if (N <= 128)
            {
                //converting the 2D arrays to 1D arrays
                int[] M1_flat = new int[N * N];
                int[] M2_flat = new int[N * N];
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        M1_flat[i * N + j] = M1[i, j];
                        M2_flat[i * N + j] = M2[i, j];
                    }
                }

                //multiply the 1D arrays
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        int sum = 0;
                        for (int k = 0; k < N; k++)
                        {
                            sum += M1_flat[i * N + k] * M2_flat[k * N + j];
                        }
                        result[i, j] = sum;
                    }
                }

                return result;
            }

            // Allocate memory for temporary matrices to use them
            // quarters
            int[,] sub11M1 = new int[N / 2, N / 2];
            int[,] sub12M1 = new int[N / 2, N / 2];
            int[,] sub21M1 = new int[N / 2, N / 2];
            int[,] sub22M1 = new int[N / 2, N / 2];
            int[,] sub11M2 = new int[N / 2, N / 2];
            int[,] sub12M2 = new int[N / 2, N / 2];
            int[,] sub21M2 = new int[N / 2, N / 2];
            int[,] sub22M2 = new int[N / 2, N / 2];
            // products
            int[,] Product1 = new int[N / 2, N / 2];
            int[,] Product2 = new int[N / 2, N / 2];
            int[,] Product3 = new int[N / 2, N / 2];
            int[,] Product4 = new int[N / 2, N / 2];
            int[,] Product5 = new int[N / 2, N / 2];
            int[,] Product6 = new int[N / 2, N / 2];
            int[,] Product7 = new int[N / 2, N / 2];

            // Split the each matrix into quarters
            Parallel.For(0, N / 2, i =>
            {
                for (int j = 0; j < N / 2; j++)
                {
                    sub11M1[i, j] = M1[i, j];
                    sub12M1[i, j] = M1[i, j + N / 2];
                    sub21M1[i, j] = M1[i + N / 2, j];
                    sub22M1[i, j] = M1[i + N / 2, j + N / 2];
                    sub11M2[i, j] = M2[i, j];
                    sub12M2[i, j] = M2[i, j + N / 2];
                    sub21M2[i, j] = M2[i + N / 2, j];
                    sub22M2[i, j] = M2[i + N / 2, j + N / 2];
                }
            });

            // Parallel Tasks array
            var parallelTasks = new Task[7];

            // Execute each matrix multiplication operation in a separate task using recursive calls
            parallelTasks[0] = Task.Factory.StartNew(() => Product1 = MatrixMultiply(sub11M1, MatricesAddSub(sub12M2, sub22M2, N / 2,false), N / 2));
            parallelTasks[1] = Task.Factory.StartNew(() => Product2 = MatrixMultiply(MatricesAddSub(sub11M1, sub12M1, N / 2, true), sub22M2, N / 2));
            parallelTasks[2] = Task.Factory.StartNew(() => Product3 = MatrixMultiply(MatricesAddSub(sub21M1, sub22M1, N / 2, true), sub11M2, N / 2));
            parallelTasks[3] = Task.Factory.StartNew(() => Product4 = MatrixMultiply(sub22M1, MatricesAddSub(sub21M2, sub11M2, N / 2, false), N / 2));
            parallelTasks[4] = Task.Factory.StartNew(() => Product5 = MatrixMultiply(MatricesAddSub(sub11M1, sub22M1, N / 2,true), MatricesAddSub(sub11M2, sub22M2, N / 2, true), N / 2));
            parallelTasks[5] = Task.Factory.StartNew(() => Product6 = MatrixMultiply(MatricesAddSub(sub12M1, sub22M1, N / 2, false), MatricesAddSub(sub21M2, sub22M2, N / 2, true), N / 2));
            parallelTasks[6] = Task.Factory.StartNew(() => Product7 = MatrixMultiply(MatricesAddSub(sub11M1, sub21M1, N / 2, false), MatricesAddSub(sub11M2, sub12M2, N / 2, true), N / 2));

            // Wait to complete tasks
            Task.WaitAll(parallelTasks);

            // Allocate memory for sub-results
            int[,] subRslt11 = new int[N / 2, N / 2];
            int[,] subRslt12 = new int[N / 2, N / 2];
            int[,] subRslt21 = new int[N / 2, N / 2];
            int[,] subRslt22 = new int[N / 2, N / 2];

            // Compute the sub-results
            Parallel.For(0, N / 2, i =>
            {
                for (int j = 0; j < N / 2; j++)
                {
                    subRslt11[i, j] = Product5[i, j] + Product4[i, j] - Product2[i, j] + Product6[i, j];
                    subRslt12[i, j] = Product1[i, j] + Product2[i, j];
                    subRslt21[i, j] = Product3[i, j] + Product4[i, j];
                    subRslt22[i, j] = Product5[i, j] + Product1[i, j] - Product3[i, j] - Product7[i, j];
                }
            });

            Parallel.For(0, N / 2, x =>
            {
                for (int y = 0; y < N / 2; y++)
                {
                    result[x, y] = subRslt11[x, y];
                    result[x, y + N / 2] = subRslt12[x, y];
                    result[x + N / 2, y] = subRslt21[x, y];
                    result[x + N / 2, y + N / 2] = subRslt22[x, y];
                }
            });

            return result;
        }
        
        // Helper Function to add and subtract two square matrices
        static public int[,] MatricesAddSub(int[,] A, int[,] B, int n,bool sign)
        {
            int[,] result = new int[n, n];
            Parallel.ForEach(Enumerable.Range(0, n), x =>
            {
                for (int y = 0; y < n; y++)
                {
                    if (sign == true)
                    {
                        result[x, y] = A[x, y] + B[x, y];
                    }
                    else if (sign == false)
                    {
                        result[x, y] = A[x, y] - B[x, y];
                    }
                }
            });


            return result;
        }

    }
}
