using System;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics.Arm;

namespace Algebric.Internal;

internal static class MatOperations
{
    private const int splitThreshold = 128;

    internal static unsafe void Sum(
        float* p, float* q,
        int n, int m
    )
    {
        if (p is null)
            throw new ArgumentNullException("p");
        
        if (q is null)
            throw new ArgumentNullException("q");
        
        if (n < 1 || m < 1)
            throw new InvalidOperationException("size of matriz may be bigger than 0");
        
        sumFullMatrix(p, q, n, m);
    }
    
    internal static unsafe void Sum(
        float* p, float* q, 
        int pi, int pj,
        int qi, int qj,
        int n, int m
    )
    {
        if (p is null)
            throw new ArgumentNullException("p");
        
        if (q is null)
            throw new ArgumentNullException("q");
        
        if (n < 1 || m < 1)
            throw new InvalidOperationException("size of matriz may be bigger than 0");
        
        sumSubMatrix(p, q, pi, pj, qi, qj, n, m);
    }

    internal static unsafe void sumSubMatrix(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m
    )
    {
        if (n * m < splitThreshold * splitThreshold)
            iterativeSum(p, q, pi, pj, qi, qj, n, m);
        else parallelSum(p, q, pi, pj, qi, qj, n, m);
    }
    
    internal static unsafe void sumFullMatrix(
        float* p, float* q, 
        int n, int m
    )
    {
        if (n * m < splitThreshold * splitThreshold)
            iterativeSum(p, q, n, m);
        else parallelSum(p, q, n, m);
    }

    internal static unsafe void iterativeSum(
        float* p, float* q,
        int n, int m
    )
    {
        if (AdvSimd.IsSupported)
            smidSum(p, q, n, m);
        else if (Sse42.IsSupported)
            sse42Sum(p, q, n, m);
        else if (Sse41.IsSupported)
            sse41Sum(p, q, n, m);
        else if (Avx2.IsSupported)
            avxSum(p, q, n, m);
        else if (Sse3.IsSupported)
            sse3Sum(p, q, n, m);
        else
            slowSum(p, q, n, m);
    }

    private static unsafe void smidSum(float* p, float* q, int n, int m)
    {
        throw new NotImplementedException();
    }

    private static unsafe void sse42Sum(float* p, float* q, int n, int m)
    {
        throw new NotImplementedException();
    }

    private static unsafe void sse41Sum(float* p, float* q, int n, int m)
    {
        throw new NotImplementedException();
    }

    private static unsafe void avxSum(float* p, float* q, int n, int m)
    {
        throw new NotImplementedException();
    }

    private static unsafe void sse3Sum(float* p, float* q, int n, int m)
    {
        throw new NotImplementedException();
    }

    internal static unsafe void slowSum(
        float* p, float* q,
        long n, long m
    )
    {
        const long jump = 8;
        long len = n * m - jump;
        float* end = p + len;

        for (; p < end; p += jump, q += jump)
        {
            *(p + 0) += *(q + 0);
            *(p + 1) += *(q + 1);
            *(p + 2) += *(q + 2);
            *(p + 3) += *(q + 3);
            *(p + 4) += *(q + 4);
            *(p + 5) += *(q + 5);
            *(p + 6) += *(q + 6);
            *(p + 7) += *(q + 7);
        }

        for (; p < end; p++, q++)
            *p += *q;
    }

    internal static unsafe void parallelSum(
        float* p, float* q, 
        int n, int m
    )
    {
        throw new NotImplementedException();
    }

    internal static unsafe void iterativeSum(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m
    )
    {
        throw new NotImplementedException();
    }

    internal static unsafe void parallelSum(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m
    )
    {
        throw new NotImplementedException();
    }
}