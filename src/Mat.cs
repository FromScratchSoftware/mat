namespace Algebric;

public class Mat
{
    private int len;
    private float[] data;

    public Mat(int a, int b)
    {
        this.len = a * b;
        this.data = new float[this.len];
    }
}