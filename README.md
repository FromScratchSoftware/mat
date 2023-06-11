# Scratch.Mathematics.Mat

A high performance library for matrix manipulation with .NET.

## Install

To install this library you can use the follow command line to use nuget inside of your project:

```
dotnet add package Scratch.Mathematics.Mat
```

To include the main classes added a using in your files:

```cs
using Scratch.Mathematics.Mat; 
```

Se the [versions sections](#versions) to see details of features in each version.

## How to use

### Create Mat (all versions)

Mat needed disposed because use unmanaged data. 
So is recommended the use of using keyword in declarations.

```cs
using Scratch.Mathematics.Mat; 

using var A = Mat.Create(5, 5, // Create a 5x5 mat with the values below
    1, 2, 3, 4, 5,
    1, 2, 3, 4, 5,
    1, 2, 3, 4, 5,
    1, 2, 3, 4, 5,
    1, 2, 3, 4, 5
);

using var B = Mat.Zeros(5, 5); // Create a 5x5 mat with zero in all positions
```

### Basic operations (all versions)

The basic operations how +, -, * are implemented in IMat interface.
While A.Add(B) update the A object A + B copy the A object and update their copy.
This means that in every operation a new unmanaged data is allocated by a Mat object
and we need be careful to avoid memory leak. In nexts versions lazy operations will
be simplify this operations.

```cs
using Scratch.Mathematics.Mat;

using var A = Mat.Zeros(5, 5);

using var B = Mat.Create(5, 5,
    1, 2, 3, 4, 5, 
    6, 7, 8, 9, 0,
    2, 2, 3, 4, 5,
    6, 7, 8, 9, 0,
    2, 2, 3, 4, 5
);

using var C = A + B;

using var D = 4 * C;
```

## Versions

### V 0.1.0 Alfa (Current)

- IMat base inteface with Clone, ToMat, Add, Subtract, Multiply, Product, Copy and Compare operations.
- Mat concrete class with high performance implementations of IMat except Product of matrix.
- Added a empty structure for future implementation of Lazy opeartions.
- Zeros and Create static method in Mat class to create concrete matrix.
- ToString implemetation to visualize a Mat.