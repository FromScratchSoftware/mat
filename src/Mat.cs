using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Algebric;

using Internal;

public unsafe class Mat : IMat
{
    private int n;
    private int m;
    private float* data;

    public int N => this.n;

    public int M => this.m;

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

    public void Add(IMat A)
    {
        if (this.n != A.N || this.m != A.M)
            throw new InvalidOperationException(
                "The mat objects have different sizes."
            );
        
        try
        {
            var mat = A.ToMat();
            MatAddOperations.Sum(this.data, mat.data, n, m);
        }
        catch (Exception e)
        {
            throw new SystemException(e.Message);
        }
    }

    public void Subtract(IMat A)
    {
        if (this.n != A.N || this.m != A.M)
            throw new InvalidOperationException(
                "The mat objects have different sizes."
            );
        
        try
        {
            var mat = A.ToMat();
            MatSubOperations.Sub(this.data, mat.data, n, m);
        }
        catch (Exception e)
        {
            throw new SystemException(e.Message);
        }
    }

    public void Multiply(IMat A)
    {
        if (this.n != A.N || this.m != A.M)
            throw new InvalidOperationException(
                "The mat objects have different sizes."
            );
        
        try
        {
            var mat = A.ToMat();
            MatMulOperations.Mul(this.data, mat.data, n, m);
        }
        catch (Exception e)
        {
            throw new SystemException(e.Message);
        }
    }

    public void Multiply(float scalar)
    {
        try
        {
            MatScalarMulOperations.Mul(this.data, scalar, n, m);
        }
        catch (Exception e)
        {
            throw new SystemException(e.Message);
        }
    }

    public void Product(IMat A)
    {
        throw new NotImplementedException();
    }

    public void Copy(IMat A)
    {
        if (this.n != A.N || this.m != A.M)
            throw new InvalidOperationException(
                "The mat objects have different sizes."
            );
        
        try
        {
            var mat = A.ToMat();
            MatCopyOperations.Copy(mat.data, this.data, this.n, this.m);
        }
        catch (Exception e)
        {
            throw new SystemException(e.Message);
        }
    }

    public IMat Clone()
    {
        var newMat = Zeros(this.n, this.m);
        newMat.Copy(this);
        return newMat;
    }
    
    public Mat ToMat()
        => this;

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

    public static IMat Zeros(int n, int m)
        => new Mat(n, m);
        
    public static IMat Create(int n, int m, params float[] data)
        => new Mat(n, m, data);
}