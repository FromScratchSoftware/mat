using System;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics.Arm;

namespace Algebric.Internal;

internal static class MatOperations
{
    private const int splitThreshold = 128;
    
    internal static unsafe void Sum(float* p, float* q, float n, float m)
    {
        if (p is null)
            throw new ArgumentNullException("p");
        
        if (q is null)
            throw new ArgumentNullException("q");
        
        if (n < 1 || m < 1)
            throw new InvalidOperationException("size of matriz may be bigger than 0");
        
        if (n == m) sum(p, q, n);
        else sum(p, q, n, m);
    }
    
    internal static unsafe void sum(float* p, float* q, float n, float m)
    {
        
    }

    internal static unsafe void sum(float* p, float* q, float n)
    {
        if (n < splitThreshold || Environment.ProcessorCount < 2)
            iterativeSum(p, q, n);
        else parallelSum(p, q, n);
    }

    internal static unsafe void iterativeSum(float* p, float* q, float n)
    {
        
    }

    internal static unsafe void parallelSum(float* p, float* q, float n)
    {

    }
}