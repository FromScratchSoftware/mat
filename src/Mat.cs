using System;
using System.Buffers;

namespace Algebric;

using Internal;

public class Mat : IDisposable
{
    private int n;
    private int m;
    private float[] data;

    public Mat(int n, int m)
    {
        this.n = n;
        this.m = m;

        var pool = ArrayPool<float>.Shared;
        int len = n * m;
        
        this.data = pool.Rent(len);
    }

    public unsafe void Add(Mat A)
    {
        if (this.n != A.n || this.m != A.m)
            throw new InvalidOperationException(
                "The mat objects have different sizes."
            );
        
        try
        {
            fixed (float* p = &data[0], q = &A.data[0])
            {
                MatAddOperations.Sum(p, q, n, m);
            }
        }
        catch (Exception e)
        {
            throw new SystemException(e.Message);
        }
    }

    public unsafe void Sub(Mat A)
    {
        if (this.n != A.n || this.m != A.m)
            throw new InvalidOperationException(
                "The mat objects have different sizes."
            );
        
        try
        {
            fixed (float* p = &data[0], q = &A.data[0])
            {
                MatSubOperations.Sub(p, q, n, m);
            }
        }
        catch (Exception e)
        {
            throw new SystemException(e.Message);
        }
    }

    public void Dispose()
    {
        var pool = ArrayPool<float>.Shared;
        pool.Return(data, true);
    }
}