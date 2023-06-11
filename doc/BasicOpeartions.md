# Basic Operations

## Create Mat

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