using System;

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

Console.WriteLine(D);