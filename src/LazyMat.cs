namespace Scratch.Mathematics.Mat;

public abstract class LazyMat : IMat
{
    public int N => throw new System.NotImplementedException();

    public int M => throw new System.NotImplementedException();

    public void Add(IMat A)
    {
        throw new System.NotImplementedException();
    }

    public IMat Clone()
    {
        throw new System.NotImplementedException();
    }

    public void Copy(IMat A)
    {
        throw new System.NotImplementedException();
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public void Multiply(IMat A)
    {
        throw new System.NotImplementedException();
    }

    public void Multiply(float scalar)
    {
        throw new System.NotImplementedException();
    }

    public void Product(IMat A)
    {
        throw new System.NotImplementedException();
    }

    public void Subtract(IMat A)
    {
        throw new System.NotImplementedException();
    }

    public Mat ToMat()
    {
        throw new System.NotImplementedException();
    }
}