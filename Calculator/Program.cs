namespace program;
public class Calculator
{
    private const int V = 0;

    public int multiply(int v1, int v2)
    {
        return v1 * v2;
    }

    public int divide(int v1, int v2)
    {
        if (v2 == V)
        {
            throw new DivideByZeroException();
        }
        else
        {
            return v1 / v2;
        }
        
    }
    public static void Main(string[] args)
    {

    }

}
