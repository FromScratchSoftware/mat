using System;

namespace Scratch.Mat;

public interface IMat : IDisposable
{
    int N { get; }
    int M { get; }

    IMat Clone();
    Mat ToMat();
    void Add(IMat A);
    void Subtract(IMat A);
    void Multiply(IMat A);
    void Multiply(float scalar);
    void Product(IMat A);
    void Copy(IMat A);

    static IMat operator +(IMat A, IMat B)
    {
        var newMat = A.Clone();
        newMat.Add(B);
        return newMat;
    }
    
    static IMat operator -(IMat A, IMat B)
    {
        var newMat = A.Clone();
        newMat.Subtract(B);
        return newMat;
    }
    
    static IMat operator *(IMat A, IMat B)
    {
        var newMat = A.Clone();
        newMat.Multiply(B);
        return newMat;
    }
    
    static IMat operator *(IMat A, float a)
    {
        var newMat = A.Clone();
        newMat.Multiply(a);
        return newMat;
    }
    
    static IMat operator *(float a, IMat A)
    {
        var newMat = A.Clone();
        newMat.Multiply(a);
        return newMat;
    }
}