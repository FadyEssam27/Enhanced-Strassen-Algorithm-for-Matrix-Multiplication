# Enhanced Strassen Algorithm for Matrix Multiplication

[![Build Status](https://travis-ci.org/FadyEssam27/Enhanced-Strassen-Algorithm-for-Matrix-Multiplication.svg?branch=master)](https://travis-ci.org/FadyEssam27/Enhanced-Strassen-Algorithm-for-Matrix-Multiplication)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A highly optimized implementation of the Strassen algorithm for matrix multiplication, achieving a remarkable 30x speedup compared to the naive solution. This implementation has been tested extensively and even secured the 3rd place among 512 competitors.

## Features

- **High Performance**: Achieves a 30x speedup compared to the naive matrix multiplication algorithm.
- **Test Cases**: Includes a comprehensive set of test cases to ensure correctness and performance.
- **Optimized Parallelization**: Utilizes parallelization for improved efficiency.
- **Competition Ranking**: Secured the 3rd place among 512 competitors in a benchmark.

## Usage

```csharp
// Example code demonstrating how to use the MatrixMultiply function
int[,] matrix1 = /* initialize your matrix */;
int[,] matrix2 = /* initialize your matrix */;
int N = /* size of the matrices */;

int[,] result = MatrixMultiplication.MatrixMultiply(matrix1, matrix2, N);
```
##Performance
The algorithm has been optimized for performance through parallelization and efficient memory usage.

##Contributing
If you find any issues or have suggestions for improvement, feel free to open an issue or create a pull request.

##License
This project is licensed under the MIT License - see the LICENSE file for details.
