namespace tst;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void MatSum()
    {
        using var A = Mat.Create(5, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5
        );
        
        using var B = Mat.Create(5, 5,
            5, 4, 3, 2, 1,
            5, 4, 3, 2, 1,
            5, 4, 3, 2, 1,
            5, 4, 3, 2, 1,
            5, 4, 3, 2, 1
        );

        using var C = A + B;

        foreach (var value in C.ToMat())
            Assert.AreEqual(6f, value);
    }
    

    [Test]
    public void MatMul()
    {
        using var A = Mat.Create(5, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5,
            1, 2, 3, 4, 5
        );
        
        using var B = Mat.Create(5, 5,
            5, 4, 3, 2, 1,
            5, 4, 3, 2, 1,
            5, 4, 3, 2, 1,
            5, 4, 3, 2, 1,
            5, 4, 3, 2, 1
        );

        using var C = A * B;
        var M = C.ToMat();

        Assert.AreEqual(5f, M[0, 0]);
        Assert.AreEqual(8f, M[1, 1]);
        Assert.AreEqual(9f, M[2, 2]);
        Assert.AreEqual(8f, M[3, 3]);
        Assert.AreEqual(5f, M[4, 4]);
    }
}