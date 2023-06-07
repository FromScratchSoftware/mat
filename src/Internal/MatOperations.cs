using System;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics.Arm;

namespace Algebric.Internal;

internal static class MatOperations
{
    private const int splitThreshold = 128;

    internal static unsafe void Sum(float* p, float* q, int n, int m)
    {
        if (p is null)
            throw new ArgumentNullException("p");
        
        if (q is null)
            throw new ArgumentNullException("q");
        
        if (n < 1 || m < 1)
            throw new InvalidOperationException("size of matriz may be bigger than 0");
        
        sum(p, q, n, m);
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
        
        sum(p, q, pi, pj, qi, qj, n, m);
    }

    internal static unsafe void sum(float* p, float* q, int n, int m)
    {
        if (n * m < splitThreshold * splitThreshold)
            iterativeSum(p, q, n, m);
        else parallelSum(p, q, n, m);
    }

    internal static unsafe void sum(
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
    
    internal static unsafe void iterativeSum(float* p, float* q,int n, int m)
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
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        for (; p < end; p += jump, q += jump)
        {
            var pv0 = AdvSimd.LoadVector128(p);
            var qv0 = AdvSimd.LoadVector128(q);
            var rv0 = AdvSimd.Add(pv0, qv0);
            AdvSimd.Store(p, rv0);

            var pv1 = AdvSimd.LoadVector128(p + bitjump);
            var qv1 = AdvSimd.LoadVector128(q + bitjump);
            var rv1 = AdvSimd.Add(pv1, qv1);
            AdvSimd.Store(p + bitjump, rv1);

            var pv2 = AdvSimd.LoadVector128(p + 2 * bitjump);
            var qv2 = AdvSimd.LoadVector128(q + 2 * bitjump);
            var rv2 = AdvSimd.Add(pv2, qv2);
            AdvSimd.Store(p + 2 * bitjump, rv2);

            var pv3 = AdvSimd.LoadVector128(p + 3 * bitjump);
            var qv3 = AdvSimd.LoadVector128(q + 3 * bitjump);
            var rv3 = AdvSimd.Add(pv3, qv3);
            AdvSimd.Store(p + 3 * bitjump, rv3);
        }

        end += jump;
        for (; p < end; p++, q++)
            *p += *q;
    }

    private static unsafe void sse42Sum(float* p, float* q, int n, int m)
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        for (; p < end; p += jump, q += jump)
        {
            var pv0 = Sse42.LoadVector128(p);
            var qv0 = Sse42.LoadVector128(q);
            var rv0 = Sse42.Add(pv0, qv0);
            Sse42.Store(p, rv0);

            var pv1 = Sse42.LoadVector128(p + bitjump);
            var qv1 = Sse42.LoadVector128(q + bitjump);
            var rv1 = Sse42.Add(pv1, qv1);
            Sse42.Store(p + bitjump, rv1);

            var pv2 = Sse42.LoadVector128(p + 2 * bitjump);
            var qv2 = Sse42.LoadVector128(q + 2 * bitjump);
            var rv2 = Sse42.Add(pv2, qv2);
            Sse42.Store(p + 2 * bitjump, rv2);

            var pv3 = Sse42.LoadVector128(p + 3 * bitjump);
            var qv3 = Sse42.LoadVector128(q + 3 * bitjump);
            var rv3 = Sse42.Add(pv3, qv3);
            Sse42.Store(p + 3 * bitjump, rv3);
        }

        end += jump;
        for (; p < end; p++, q++)
            *p += *q;
    }

    private static unsafe void sse41Sum(float* p, float* q, int n, int m)
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        for (; p < end; p += jump, q += jump)
        {
            var pv0 = Sse41.LoadVector128(p);
            var qv0 = Sse41.LoadVector128(q);
            var rv0 = Sse41.Add(pv0, qv0);
            Sse41.Store(p, rv0);

            var pv1 = Sse41.LoadVector128(p + bitjump);
            var qv1 = Sse41.LoadVector128(q + bitjump);
            var rv1 = Sse41.Add(pv1, qv1);
            Sse41.Store(p + bitjump, rv1);

            var pv2 = Sse41.LoadVector128(p + 2 * bitjump);
            var qv2 = Sse41.LoadVector128(q + 2 * bitjump);
            var rv2 = Sse41.Add(pv2, qv2);
            Sse41.Store(p + 2 * bitjump, rv2);

            var pv3 = Sse41.LoadVector128(p + 3 * bitjump);
            var qv3 = Sse41.LoadVector128(q + 3 * bitjump);
            var rv3 = Sse41.Add(pv3, qv3);
            Sse41.Store(p + 3 * bitjump, rv3);
        }

        end += jump;
        for (; p < end; p++, q++)
            *p += *q;
    }

    private static unsafe void avxSum(float* p, float* q, int n, int m)
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        for (; p < end; p += jump, q += jump)
        {
            var pv0 = Avx2.LoadVector128(p);
            var qv0 = Avx2.LoadVector128(q);
            var rv0 = Avx2.Add(pv0, qv0);
            Avx2.Store(p, rv0);

            var pv1 = Avx2.LoadVector128(p + bitjump);
            var qv1 = Avx2.LoadVector128(q + bitjump);
            var rv1 = Avx2.Add(pv1, qv1);
            Avx2.Store(p + bitjump, rv1);

            var pv2 = Avx2.LoadVector128(p + 2 * bitjump);
            var qv2 = Avx2.LoadVector128(q + 2 * bitjump);
            var rv2 = Avx2.Add(pv2, qv2);
            Avx2.Store(p + 2 * bitjump, rv2);

            var pv3 = Avx2.LoadVector128(p + 3 * bitjump);
            var qv3 = Avx2.LoadVector128(q + 3 * bitjump);
            var rv3 = Avx2.Add(pv3, qv3);
            Avx2.Store(p + 3 * bitjump, rv3);
        }

        end += jump;
        for (; p < end; p++, q++)
            *p += *q;
    }

    private static unsafe void sse3Sum(float* p, float* q, int n, int m)
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        for (; p < end; p += jump, q += jump)
        {
            var pv0 = Sse3.LoadVector128(p);
            var qv0 = Sse3.LoadVector128(q);
            var rv0 = Sse3.Add(pv0, qv0);
            Sse3.Store(p, rv0);

            var pv1 = Sse3.LoadVector128(p + bitjump);
            var qv1 = Sse3.LoadVector128(q + bitjump);
            var rv1 = Sse3.Add(pv1, qv1);
            Sse3.Store(p + bitjump, rv1);

            var pv2 = Sse3.LoadVector128(p + 2 * bitjump);
            var qv2 = Sse3.LoadVector128(q + 2 * bitjump);
            var rv2 = Sse3.Add(pv2, qv2);
            Sse3.Store(p + 2 * bitjump, rv2);

            var pv3 = Sse3.LoadVector128(p + 3 * bitjump);
            var qv3 = Sse3.LoadVector128(q + 3 * bitjump);
            var rv3 = Sse3.Add(pv3, qv3);
            Sse3.Store(p + 3 * bitjump, rv3);
        }

        end += jump;
        for (; p < end; p++, q++)
            *p += *q;
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

        end += jump;
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