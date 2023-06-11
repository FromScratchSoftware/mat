using System;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics.Arm;

namespace Scratch.Mathematics.Mat.Internal;

// TODO: Improve this operation with paralelization
internal static class MatScalarMulOperations
{
    private const int splitThreshold = 64;

    internal static unsafe void Mul(
        float* p, float scalar, 
        int n, int m
    )
    {
        if (p is null)
            throw new ArgumentNullException("p");
        
        if (n < 1 || m < 1)
            throw new InvalidOperationException("size of matriz may be bigger than 0");
        
        mul(p, scalar, n, m);
    }

    private static unsafe void mul(
        float* p, float scalar,
        int n, int m
    )
    {
        iterativeMul(p, scalar, n, m);
    }
    
    private static unsafe void iterativeMul(
        float* p, float scalar,
        int n, int m
    )
    {
        if (AdvSimd.IsSupported)
            simdMul(p, scalar, n, m);
        else if (Sse42.IsSupported)
            sse42Mul(p, scalar, n, m);
        else if (Sse41.IsSupported)
            sse41Mul(p, scalar, n, m);
        else if (Avx2.IsSupported)
            avxMul(p, scalar, n, m);
        else if (Sse3.IsSupported)
            sse3Mul(p, scalar, n, m);
        else
            slowMul(p, scalar, n, m);
    }

    private static unsafe void simdMul(
        float* p, float scalar,
        int n, int m
    )
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        var scalarArr = new float[] {
            scalar, scalar, scalar, scalar
        };
        fixed (float* scalarPtr = scalarArr)
        {
            float* q = scalarPtr;
            do
            {
                var pv0 = AdvSimd.LoadVector128(p);
                var qv0 = AdvSimd.LoadVector128(q);
                var rv0 = AdvSimd.Multiply(pv0, qv0);
                AdvSimd.Store(p, rv0);

                var pv1 = AdvSimd.LoadVector128(p + bitjump);
                var qv1 = AdvSimd.LoadVector128(q + bitjump);
                var rv1 = AdvSimd.Multiply(pv1, qv1);
                AdvSimd.Store(p + bitjump, rv1);

                var pv2 = AdvSimd.LoadVector128(p + 2 * bitjump);
                var qv2 = AdvSimd.LoadVector128(q + 2 * bitjump);
                var rv2 = AdvSimd.Multiply(pv2, qv2);
                AdvSimd.Store(p + 2 * bitjump, rv2);

                var pv3 = AdvSimd.LoadVector128(p + 3 * bitjump);
                var qv3 = AdvSimd.LoadVector128(q + 3 * bitjump);
                var rv3 = AdvSimd.Multiply(pv3, qv3);
                AdvSimd.Store(p + 3 * bitjump, rv3);

                p += jump;
                q += jump;
            } while (p < end);

            end += jump;
            do
            {
                *p *= *q;
                p++;
                q++;
            } while (p < end);
        }
    }

    private static unsafe void sse42Mul(
        float* p, float scalar,
        int n, int m
    )
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        var scalarArr = new float[] {
            scalar, scalar, scalar, scalar
        };
        fixed (float* scalarPtr = scalarArr)
        {
            float* q = scalarPtr;
            do
            {
                var pv0 = Sse42.LoadVector128(p);
                var qv0 = Sse42.LoadVector128(q);
                var rv0 = Sse42.Multiply(pv0, qv0);
                Sse42.Store(p, rv0);

                var pv1 = Sse42.LoadVector128(p + bitjump);
                var qv1 = Sse42.LoadVector128(q + bitjump);
                var rv1 = Sse42.Multiply(pv1, qv1);
                Sse42.Store(p + bitjump, rv1);

                var pv2 = Sse42.LoadVector128(p + 2 * bitjump);
                var qv2 = Sse42.LoadVector128(q + 2 * bitjump);
                var rv2 = Sse42.Multiply(pv2, qv2);
                Sse42.Store(p + 2 * bitjump, rv2);

                var pv3 = Sse42.LoadVector128(p + 3 * bitjump);
                var qv3 = Sse42.LoadVector128(q + 3 * bitjump);
                var rv3 = Sse42.Multiply(pv3, qv3);
                Sse42.Store(p + 3 * bitjump, rv3);

                p += jump;
                q += jump;
            } while (p < end);

            end += jump;
            do
            {
                *p *= *q;
                p++;
                q++;
            } while (p < end);
        }
    }

    private static unsafe void sse41Mul(
        float* p, float scalar,
        int n, int m
    )
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        var scalarArr = new float[] {
            scalar, scalar, scalar, scalar
        };
        fixed (float* scalarPtr = scalarArr)
        {
            float* q = scalarPtr;
            do
            {
                var pv0 = Sse41.LoadVector128(p);
                var qv0 = Sse41.LoadVector128(q);
                var rv0 = Sse41.Multiply(pv0, qv0);
                Sse41.Store(p, rv0);

                var pv1 = Sse41.LoadVector128(p + bitjump);
                var qv1 = Sse41.LoadVector128(q + bitjump);
                var rv1 = Sse41.Multiply(pv1, qv1);
                Sse41.Store(p + bitjump, rv1);

                var pv2 = Sse41.LoadVector128(p + 2 * bitjump);
                var qv2 = Sse41.LoadVector128(q + 2 * bitjump);
                var rv2 = Sse41.Multiply(pv2, qv2);
                Sse41.Store(p + 2 * bitjump, rv2);

                var pv3 = Sse41.LoadVector128(p + 3 * bitjump);
                var qv3 = Sse41.LoadVector128(q + 3 * bitjump);
                var rv3 = Sse41.Multiply(pv3, qv3);
                Sse41.Store(p + 3 * bitjump, rv3);

                p += jump;
                q += jump;
            } while (p < end);

            end += jump;
            do
            {
                *p *= *q;
                p++;
                q++;
            } while (p < end);
        }
    }

    private static unsafe void avxMul(
        float* p, float scalar,
        int n, int m
    )
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;


        var scalarArr = new float[] {
            scalar, scalar, scalar, scalar
        };
        fixed (float* scalarPtr = scalarArr)
        {
            float* q = scalarPtr;
            do
            {
                var pv0 = Avx2.LoadVector128(p);
                var qv0 = Avx2.LoadVector128(q);
                var rv0 = Avx2.Multiply(pv0, qv0);
                Avx2.Store(p, rv0);

                var pv1 = Avx2.LoadVector128(p + bitjump);
                var qv1 = Avx2.LoadVector128(q + bitjump);
                var rv1 = Avx2.Multiply(pv1, qv1);
                Avx2.Store(p + bitjump, rv1);

                var pv2 = Avx2.LoadVector128(p + 2 * bitjump);
                var qv2 = Avx2.LoadVector128(q + 2 * bitjump);
                var rv2 = Avx2.Multiply(pv2, qv2);
                Avx2.Store(p + 2 * bitjump, rv2);

                var pv3 = Avx2.LoadVector128(p + 3 * bitjump);
                var qv3 = Avx2.LoadVector128(q + 3 * bitjump);
                var rv3 = Avx2.Multiply(pv3, qv3);
                Avx2.Store(p + 3 * bitjump, rv3);

                p += jump;
                q += jump;
            } while (p < end);

            end += jump;
            do
            {
                *p *= *q;
                p++;
                q++;
            } while (p < end);
        }
    }

    private static unsafe void sse3Mul(
        float* p, float scalar,
        int n, int m
    )
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        var scalarArr = new float[] {
            scalar, scalar, scalar, scalar
        };
        fixed (float* scalarPtr = scalarArr)
        {
            float* q = scalarPtr;
            do
            {
                var pv0 = Sse3.LoadVector128(p);
                var qv0 = Sse3.LoadVector128(q);
                var rv0 = Sse3.Multiply(pv0, qv0);
                Sse3.Store(p, rv0);

                var pv1 = Sse3.LoadVector128(p + bitjump);
                var qv1 = Sse3.LoadVector128(q + bitjump);
                var rv1 = Sse3.Multiply(pv1, qv1);
                Sse3.Store(p + bitjump, rv1);

                var pv2 = Sse3.LoadVector128(p + 2 * bitjump);
                var qv2 = Sse3.LoadVector128(q + 2 * bitjump);
                var rv2 = Sse3.Multiply(pv2, qv2);
                Sse3.Store(p + 2 * bitjump, rv2);

                var pv3 = Sse3.LoadVector128(p + 3 * bitjump);
                var qv3 = Sse3.LoadVector128(q + 3 * bitjump);
                var rv3 = Sse3.Multiply(pv3, qv3);
                Sse3.Store(p + 3 * bitjump, rv3);

                p += jump;
                q += jump;
            } while (p < end);

            end += jump;
            do
            {
                *p *= *q;
                p++;
                q++;
            } while (p < end);
        }
    }

    private static unsafe void slowMul(
        float* p, float scalar,
        long n, long m
    )
    {
        const long jump = 8;
        long len = n * m - jump;
        float* end = p + len;

        do
        {
            *(p + 0) *= scalar;
            *(p + 1) *= scalar;
            *(p + 2) *= scalar;
            *(p + 3) *= scalar;
            *(p + 4) *= scalar;
            *(p + 5) *= scalar;
            *(p + 6) *= scalar;
            *(p + 7) *= scalar;
            p += jump;
        } while (p < end);

        end += jump;
        do
        {
            *p *= scalar;
            p++;
        } while (p < end);
    }
}