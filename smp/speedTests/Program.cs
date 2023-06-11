using System;
using System.Diagnostics;
using Scratch.Mathematics.Mat;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

var rand = Random.Shared;

float[] temp = new float[100];
for (int i = 0; i < temp.Length; i++)
    temp[i] = rand.NextSingle();
float[][] data1 = new float[100][];
for (int i = 0; i < data1.Length; i++)
    data1[i] = temp;

float[] data2 = new float[10_000];
for (int i = 0; i < data2.Length; i++)
    data2[i] = rand.NextSingle();

Matrix<float> m1 = DenseMatrix.OfRowArrays(data1);

using var m2 = Mat.Create(100, 100, data2);

int N = 1_000_000;
Stopwatch sw = new Stopwatch();

sw.Start();
for (int i = 0; i < N; i++)
{
    var r1 = m1 + m1;
}
sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds);
sw.Reset();

sw.Start();
for (int i = 0; i < N; i++)
{
    try
    {
        using var r2 = m2 + m2;
    }
    catch
    {
        
    }
}
sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds);