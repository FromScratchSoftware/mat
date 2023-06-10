using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Algebric;

using Internal;

public unsafe class Mat : IDisposable
{
    private int n;
    private int m;
    private float* data;

    private Mat(int n, int m)
    {
        this.n = n;
        this.m = m;
        int len = n * m;
        
        this.data = (float*)Marshal.AllocHGlobal(len * sizeof(float));
        for (int i = 0; i < len; i++)
            this.data[i] = 0f;
    }

    private Mat(int n, int m, float[] data)
    {
        this.n = n;
        this.m = m;
        int len = n * m;
        
        this.data = (float*)Marshal.AllocHGlobal(len * sizeof(float));

        len = len > data.Length ? data.Length : len;
        Marshal.Copy(data, 0, (nint)this.data, len);
    }

    public unsafe void Add(Mat A)
    {
        if (this.n != A.n || this.m != A.m)
            throw new InvalidOperationException(
                "The mat objects have different sizes."
            );
        
        try
        {
            MatAddOperations.Sum(this.data, A.data, n, m);
        }
        catch (Exception e)
        {
            throw new SystemException(e.Message);
        }
    }

    public unsafe void Subtract(Mat A)
    {
        if (this.n != A.n || this.m != A.m)
            throw new InvalidOperationException(
                "The mat objects have different sizes."
            );
        
        try
        {
            MatSubOperations.Sub(this.data, A.data, n, m);
        }
        catch (Exception e)
        {
            throw new SystemException(e.Message);
        }
    }

    public unsafe void Multiply(Mat A)
    {
        if (this.n != A.n || this.m != A.m)
            throw new InvalidOperationException(
                "The mat objects have different sizes."
            );
        
        try
        {
            MatMulOperations.Mul(this.data, A.data, n, m);
        }
        catch (Exception e)
        {
            throw new SystemException(e.Message);
        }
    }

    public unsafe Mat Clone()
    {
        var newMat = Zeros(this.n, this.m);
        MatCopyOperations.Copy(newMat.data, this.data, this.n, this.m);
        return newMat;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        int sizeN = n > 10 ? 10 : n;
        int sizeM = m > 10 ? 10 : m;
        
        sb.Append("┌─");
        sb.Append(' ', 9 * sizeN - 1);
        sb.AppendLine("─┐");
        for (int j = 0; j < sizeM; j++)
        {
            sb.Append("│ ");
            for (int i = 0; i < sizeN; i++)
            {
                int index = i + n * j;
                if (j == sizeM - 2 && m > sizeM)
                {
                    sb.Append("   ...   ");
                    continue;
                }

                if (j == sizeM - 1 && m > sizeM)
                {
                    index += n * (m - 1 - j);
                }

                if (i == sizeN - 2 && n > sizeN)
                {
                    sb.Append("   ...   ");
                    continue;
                }

                if (i == sizeN - 1 && n > sizeN)
                {
                    index += n - 1 - i;
                }

                var value = data[index];
                sb.Append($"{value:E1} ");
            }
            sb.AppendLine("│");
        }
        sb.Append("└─");
        sb.Append(' ', 9 * sizeN - 1);
        sb.Append("─┘");

        return sb.ToString();
    }

    public void Dispose()
    {
        var dataPointer = (nint)this.data;
        Marshal.FreeHGlobal(dataPointer);
    }

    public static Mat Zeros(int n, int m)
        => new Mat(n, m);
        
    public static Mat Create(int n, int m, params float[] data)
        => new Mat(n, m, data);
}