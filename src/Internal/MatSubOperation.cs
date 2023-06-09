using System;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics.Arm;

namespace Algebric.Internal;

internal static class MatSubOperations
{
    private const int splitThreshold = 64;

    internal static unsafe void Sub(
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
        
        sub(p, q, n, m);
    }
    
    internal static unsafe void Sub(
        float* p, float* q, 
        int pi, int pj,
        int qi, int qj,
        int n, int m,
        int strP, int strQ
    )
    {
        if (p is null)
            throw new ArgumentNullException("p");
        
        if (q is null)
            throw new ArgumentNullException("q");
        
        if (n < 1 || m < 1)
            throw new InvalidOperationException("size of matriz may be bigger than 0");
        
        sub(p, q, pi, pj, qi, qj, n, m, strP, strQ);
    }

    private static unsafe void sub(
        float* p, float* q,
        int n, int m
    )
    {
        if (n * m < splitThreshold * splitThreshold)
            iterativeSub(p, q, n, m);
        else parallelSub(p, q, n, m);
    }

    private static unsafe void sub(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m,
        int strP, int strQ
    )
    {
        if (n * m < splitThreshold * splitThreshold)
            iterativeSub(p, q, pi, pj, qi, qj, n, m, strP, strQ);
        else parallelSub(p, q, pi, pj, qi, qj, n, m, strP, strQ);
    }
    
    private static unsafe void iterativeSub(
        float* p, float* q,
        int n, int m
    )
    {
        if (AdvSimd.IsSupported)
            simdSub(p, q, n, m);
        else if (Sse42.IsSupported)
            sse42Sub(p, q, n, m);
        else if (Sse41.IsSupported)
            sse41Sub(p, q, n, m);
        else if (Avx2.IsSupported)
            avxSub(p, q, n, m);
        else if (Sse3.IsSupported)
            sse3Sub(p, q, n, m);
        else
            slowSub(p, q, n, m);
    }

    private static unsafe void simdSub(
        float* p, float* q,
        int n, int m
    )
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        do
        {
            var pv0 = AdvSimd.LoadVector128(p);
            var qv0 = AdvSimd.LoadVector128(q);
            var rv0 = AdvSimd.Subtract(pv0, qv0);
            AdvSimd.Store(p, rv0);

            var pv1 = AdvSimd.LoadVector128(p + bitjump);
            var qv1 = AdvSimd.LoadVector128(q + bitjump);
            var rv1 = AdvSimd.Subtract(pv1, qv1);
            AdvSimd.Store(p + bitjump, rv1);

            var pv2 = AdvSimd.LoadVector128(p + 2 * bitjump);
            var qv2 = AdvSimd.LoadVector128(q + 2 * bitjump);
            var rv2 = AdvSimd.Subtract(pv2, qv2);
            AdvSimd.Store(p + 2 * bitjump, rv2);

            var pv3 = AdvSimd.LoadVector128(p + 3 * bitjump);
            var qv3 = AdvSimd.LoadVector128(q + 3 * bitjump);
            var rv3 = AdvSimd.Subtract(pv3, qv3);
            AdvSimd.Store(p + 3 * bitjump, rv3);
        } while (p < end);

        end += jump;
        do
        {
            *p -= *q;
            p++;
            q++;
        } while (p < end);
    }

    private static unsafe void sse42Sub(
        float* p, float* q,
        int n, int m
    )
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        do
        {
            var pv0 = Sse42.LoadVector128(p);
            var qv0 = Sse42.LoadVector128(q);
            var rv0 = Sse42.Subtract(pv0, qv0);
            Sse42.Store(p, rv0);

            var pv1 = Sse42.LoadVector128(p + bitjump);
            var qv1 = Sse42.LoadVector128(q + bitjump);
            var rv1 = Sse42.Subtract(pv1, qv1);
            Sse42.Store(p + bitjump, rv1);

            var pv2 = Sse42.LoadVector128(p + 2 * bitjump);
            var qv2 = Sse42.LoadVector128(q + 2 * bitjump);
            var rv2 = Sse42.Subtract(pv2, qv2);
            Sse42.Store(p + 2 * bitjump, rv2);

            var pv3 = Sse42.LoadVector128(p + 3 * bitjump);
            var qv3 = Sse42.LoadVector128(q + 3 * bitjump);
            var rv3 = Sse42.Subtract(pv3, qv3);
            Sse42.Store(p + 3 * bitjump, rv3);
        } while (p < end);

        end += jump;
        do
        {
            *p -= *q;
            p++;
            q++;
        } while (p < end);
    }

    private static unsafe void sse41Sub(
        float* p, float* q,
        int n, int m
    )
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        do
        {
            var pv0 = Sse41.LoadVector128(p);
            var qv0 = Sse41.LoadVector128(q);
            var rv0 = Sse41.Subtract(pv0, qv0);
            Sse41.Store(p, rv0);

            var pv1 = Sse41.LoadVector128(p + bitjump);
            var qv1 = Sse41.LoadVector128(q + bitjump);
            var rv1 = Sse41.Subtract(pv1, qv1);
            Sse41.Store(p + bitjump, rv1);

            var pv2 = Sse41.LoadVector128(p + 2 * bitjump);
            var qv2 = Sse41.LoadVector128(q + 2 * bitjump);
            var rv2 = Sse41.Subtract(pv2, qv2);
            Sse41.Store(p + 2 * bitjump, rv2);

            var pv3 = Sse41.LoadVector128(p + 3 * bitjump);
            var qv3 = Sse41.LoadVector128(q + 3 * bitjump);
            var rv3 = Sse41.Subtract(pv3, qv3);
            Sse41.Store(p + 3 * bitjump, rv3);
        } while (p < end);

        end += jump;
        do
        {
            *p -= *q;
            p++;
            q++;
        } while (p < end);
    }

    private static unsafe void avxSub(
        float* p, float* q,
        int n, int m
    )
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        do
        {
            var pv0 = Avx2.LoadVector128(p);
            var qv0 = Avx2.LoadVector128(q);
            var rv0 = Avx2.Subtract(pv0, qv0);
            Avx2.Store(p, rv0);

            var pv1 = Avx2.LoadVector128(p + bitjump);
            var qv1 = Avx2.LoadVector128(q + bitjump);
            var rv1 = Avx2.Subtract(pv1, qv1);
            Avx2.Store(p + bitjump, rv1);

            var pv2 = Avx2.LoadVector128(p + 2 * bitjump);
            var qv2 = Avx2.LoadVector128(q + 2 * bitjump);
            var rv2 = Avx2.Subtract(pv2, qv2);
            Avx2.Store(p + 2 * bitjump, rv2);

            var pv3 = Avx2.LoadVector128(p + 3 * bitjump);
            var qv3 = Avx2.LoadVector128(q + 3 * bitjump);
            var rv3 = Avx2.Subtract(pv3, qv3);
            Avx2.Store(p + 3 * bitjump, rv3);
        } while (p < end);

        end += jump;
        do
        {
            *p -= *q;
            p++;
            q++;
        } while (p < end);
    }

    private static unsafe void sse3Sub(
        float* p, float* q,
        int n, int m
    )
    {
        const long bitjump = 4;
        const long jump = 16;
        long len = n * m - jump;
        float* end = p + len;

        do
        {
            var pv0 = Sse3.LoadVector128(p);
            var qv0 = Sse3.LoadVector128(q);
            var rv0 = Sse3.Subtract(pv0, qv0);
            Sse3.Store(p, rv0);

            var pv1 = Sse3.LoadVector128(p + bitjump);
            var qv1 = Sse3.LoadVector128(q + bitjump);
            var rv1 = Sse3.Subtract(pv1, qv1);
            Sse3.Store(p + bitjump, rv1);

            var pv2 = Sse3.LoadVector128(p + 2 * bitjump);
            var qv2 = Sse3.LoadVector128(q + 2 * bitjump);
            var rv2 = Sse3.Subtract(pv2, qv2);
            Sse3.Store(p + 2 * bitjump, rv2);

            var pv3 = Sse3.LoadVector128(p + 3 * bitjump);
            var qv3 = Sse3.LoadVector128(q + 3 * bitjump);
            var rv3 = Sse3.Subtract(pv3, qv3);
            Sse3.Store(p + 3 * bitjump, rv3);
            p += jump;
            q += jump;
        } while (p < end);

        end += jump;
        do
        {
            *p -= *q;
            p++;
            q++;
        } while (p < end);
    }

    private static unsafe void slowSub(
        float* p, float* q,
        long n, long m
    )
    {
        const long jump = 8;
        long len = n * m - jump;
        float* end = p + len;

        do
        {
            *(p + 0) -= *(q + 0);
            *(p + 1) -= *(q + 1);
            *(p + 2) -= *(q + 2);
            *(p + 3) -= *(q + 3);
            *(p + 4) -= *(q + 4);
            *(p + 5) -= *(q + 5);
            *(p + 6) -= *(q + 6);
            *(p + 7) -= *(q + 7);
            p += jump;
            q += jump;
        } while (p < end);

        end += jump;
        do
        {
            *p -= *q;
            p++;
            q++;
        } while (p < end);
    }

    private static unsafe void iterativeSub(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m,
        int strP, int strQ
    )
    {
        if (AdvSimd.IsSupported)
            simdSub(p, q, pi, pj, qi, qj, n, m, strP, strQ);
        else if (Sse42.IsSupported)
            sse42Sub(p, q, pi, pj, qi, qj, n, m, strP, strQ);
        else if (Sse41.IsSupported)
            sse41Sub(p, q, pi, pj, qi, qj, n, m, strP, strQ);
        else if (Avx2.IsSupported)
            avxSub(p, q, pi, pj, qi, qj, n, m, strP, strQ);
        else if (Sse3.IsSupported)
            sse3Sub(p, q, pi, pj, qi, qj, n, m, strP, strQ);
        else
            slowSub(p, q, pi, pj, qi, qj, n, m, strP, strQ);
    }

    private static unsafe void slowSub(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m,
        int strP, int strQ
    )
    {
        p += pj * strP + pi;
        q += qj * strQ + qj;
        strP -= n;
        strQ -= n;

        const long jump = 4;
        float* lineEnd = null;

        for (int j = 0; j < m; j++)
        {
            lineEnd = p + n - jump;
            do
            {
                *(p + 0) -= *(q + 0);
                *(p + 1) -= *(q + 1);
                *(p + 2) -= *(q + 2);
                *(p + 3) -= *(q + 3);
                p += jump;
                q += jump;
            } while (p < lineEnd);

            lineEnd += jump;
            do
            {
                *p -= *q;
                p++;
                q++;
            } while (p < lineEnd);

            p += strP;
            q += strQ;
        }
    }

    private static unsafe void sse3Sub(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m,
        int strP, int strQ
    )
    {
        p += pj * strP + pi;
        q += qj * strQ + qj;
        strP -= n;
        strQ -= n;

        const long jump = 16;
        const long bitJump = 4;
        float* lineEnd = null;

        for (int j = 0; j < m; j++)
        {
            lineEnd = p + n - jump;
            do
            {
                var vp0 = Sse3.LoadVector128(p);
                var vq0 = Sse3.LoadVector128(q);
                var vr0 = Sse3.Subtract(vp0, vq0);
                Sse3.Store(p, vr0);
                
                var vp1 = Sse3.LoadVector128(p + bitJump);
                var vq1 = Sse3.LoadVector128(q + bitJump);
                var vr1 = Sse3.Subtract(vp1, vq1);
                Sse3.Store(p + bitJump, vr1);
                
                var vp2 = Sse3.LoadVector128(p + 2 * bitJump);
                var vq2 = Sse3.LoadVector128(q + 2 * bitJump);
                var vr2 = Sse3.Subtract(vp2, vq2);
                Sse3.Store(p + 2 * bitJump, vr2);
                
                var vp3 = Sse3.LoadVector128(p + 3 * bitJump);
                var vq3 = Sse3.LoadVector128(q + 3 * bitJump);
                var vr3 = Sse3.Subtract(vp3, vq3);
                Sse3.Store(p + 3 * bitJump, vr3);

                p += jump;
                q += jump;
            } while (p < lineEnd);

            lineEnd += jump;
            do
            {
                *p -= *q;
                p++;
                q++;
            } while (p < lineEnd);

            p += strP;
            q += strQ;
        }
    }

    private static unsafe void avxSub(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m,
        int strP, int strQ
    )
    {
        p += pj * strP + pi;
        q += qj * strQ + qj;
        strP -= n;
        strQ -= n;

        const long jump = 16;
        const long bitJump = 4;
        float* lineEnd = null;

        for (int j = 0; j < m; j++)
        {
            lineEnd = p + n - jump;
            do
            {
                var vp0 = Avx2.LoadVector128(p);
                var vq0 = Avx2.LoadVector128(q);
                var vr0 = Avx2.Subtract(vp0, vq0);
                Avx2.Store(p, vr0);
                
                var vp1 = Avx2.LoadVector128(p + bitJump);
                var vq1 = Avx2.LoadVector128(q + bitJump);
                var vr1 = Avx2.Subtract(vp1, vq1);
                Avx2.Store(p + bitJump, vr1);
                
                var vp2 = Avx2.LoadVector128(p + 2 * bitJump);
                var vq2 = Avx2.LoadVector128(q + 2 * bitJump);
                var vr2 = Avx2.Subtract(vp2, vq2);
                Avx2.Store(p + 2 * bitJump, vr2);
                
                var vp3 = Avx2.LoadVector128(p + 3 * bitJump);
                var vq3 = Avx2.LoadVector128(q + 3 * bitJump);
                var vr3 = Avx2.Subtract(vp3, vq3);
                Avx2.Store(p + 3 * bitJump, vr3);

                p += jump;
                q += jump;
            } while (p < lineEnd);

            lineEnd += jump;
            do
            {
                *p -= *q;
                p++;
                q++;
            } while (p < lineEnd);

            p += strP;
            q += strQ;
        }
    }

    private static unsafe void sse41Sub(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m,
        int strP, int strQ
    )
    {
        p += pj * strP + pi;
        q += qj * strQ + qj;
        strP -= n;
        strQ -= n;

        const long jump = 16;
        const long bitJump = 4;
        float* lineEnd = null;

        for (int j = 0; j < m; j++)
        {
            lineEnd = p + n - jump;
            do
            {
                var vp0 = Sse41.LoadVector128(p);
                var vq0 = Sse41.LoadVector128(q);
                var vr0 = Sse41.Subtract(vp0, vq0);
                Sse41.Store(p, vr0);
                
                var vp1 = Sse41.LoadVector128(p + bitJump);
                var vq1 = Sse41.LoadVector128(q + bitJump);
                var vr1 = Sse41.Subtract(vp1, vq1);
                Sse41.Store(p + bitJump, vr1);
                
                var vp2 = Sse41.LoadVector128(p + 2 * bitJump);
                var vq2 = Sse41.LoadVector128(q + 2 * bitJump);
                var vr2 = Sse41.Subtract(vp2, vq2);
                Sse41.Store(p + 2 * bitJump, vr2);
                
                var vp3 = Sse41.LoadVector128(p + 3 * bitJump);
                var vq3 = Sse41.LoadVector128(q + 3 * bitJump);
                var vr3 = Sse41.Subtract(vp3, vq3);
                Sse41.Store(p + 3 * bitJump, vr3);

                p += jump;
                q += jump;
            } while (p < lineEnd);

            lineEnd += jump;
            do
            {
                *p -= *q;
                p++;
                q++;
            } while (p < lineEnd);

            p += strP;
            q += strQ;
        }
    }

    private static unsafe void sse42Sub(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m,
        int strP, int strQ
    )
    {
        p += pj * strP + pi;
        q += qj * strQ + qj;
        strP -= n;
        strQ -= n;

        const long jump = 16;
        const long bitJump = 4;
        float* lineEnd = null;

        for (int j = 0; j < m; j++)
        {
            lineEnd = p + n - jump;
            do
            {
                var vp0 = Sse42.LoadVector128(p);
                var vq0 = Sse42.LoadVector128(q);
                var vr0 = Sse42.Subtract(vp0, vq0);
                Sse42.Store(p, vr0);
                
                var vp1 = Sse42.LoadVector128(p + bitJump);
                var vq1 = Sse42.LoadVector128(q + bitJump);
                var vr1 = Sse42.Subtract(vp1, vq1);
                Sse42.Store(p + bitJump, vr1);
                
                var vp2 = Sse42.LoadVector128(p + 2 * bitJump);
                var vq2 = Sse42.LoadVector128(q + 2 * bitJump);
                var vr2 = Sse42.Subtract(vp2, vq2);
                Sse42.Store(p + 2 * bitJump, vr2);
                
                var vp3 = Sse42.LoadVector128(p + 3 * bitJump);
                var vq3 = Sse42.LoadVector128(q + 3 * bitJump);
                var vr3 = Sse42.Subtract(vp3, vq3);
                Sse42.Store(p + 3 * bitJump, vr3);

                p += jump;
                q += jump;
            } while (p < lineEnd);

            lineEnd += jump;
            do
            {
                *p -= *q;
                p++;
                q++;
            } while (p < lineEnd);

            p += strP;
            q += strQ;
        }
    }

    private static unsafe void simdSub(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m,
        int strP, int strQ
    )
    {
        p += pj * strP + pi;
        q += qj * strQ + qj;
        strP -= n;
        strQ -= n;

        const long jump = 16;
        const long bitJump = 4;
        float* lineEnd = null;

        for (int j = 0; j < m; j++)
        {
            lineEnd = p + n - jump;
            do
            {
                var vp0 = AdvSimd.LoadVector128(p);
                var vq0 = AdvSimd.LoadVector128(q);
                var vr0 = AdvSimd.Subtract(vp0, vq0);
                AdvSimd.Store(p, vr0);
                
                var vp1 = AdvSimd.LoadVector128(p + bitJump);
                var vq1 = AdvSimd.LoadVector128(q + bitJump);
                var vr1 = AdvSimd.Subtract(vp1, vq1);
                AdvSimd.Store(p + bitJump, vr1);
                
                var vp2 = AdvSimd.LoadVector128(p + 2 * bitJump);
                var vq2 = AdvSimd.LoadVector128(q + 2 * bitJump);
                var vr2 = AdvSimd.Subtract(vp2, vq2);
                AdvSimd.Store(p + 2 * bitJump, vr2);
                
                var vp3 = AdvSimd.LoadVector128(p + 3 * bitJump);
                var vq3 = AdvSimd.LoadVector128(q + 3 * bitJump);
                var vr3 = AdvSimd.Subtract(vp3, vq3);
                AdvSimd.Store(p + 3 * bitJump, vr3);

                p += jump;
                q += jump;
            } while (p < lineEnd);

            lineEnd += jump;
            do
            {
                *p -= *q;
                p++;
                q++;
            } while (p < lineEnd);

            p += strP;
            q += strQ;
        }
    }

    private static unsafe void parallelSub(
        float* p, float* q, 
        int n, int m
    )
    {
        Parallel.For(0, 64, s =>
        {
            int i = s % 8;
            int j = s / 8;
            int subN = n / 8;
            int subM = m / 8;

            if (i == 7)
                subN += n % 8;
            
            if (j == 7)
                subM += m % 8;

            int pi = i * subN;
            int pj = j * subM;

            iterativeSub(
                p, q,
                pi, pj, pi, pj,
                subN, subM, n, m
            );
        });
    }

    private static unsafe void parallelSub(
        float* p, float* q,
        int pi, int pj,
        int qi, int qj,
        int n, int m,
        int strP, int strQ
    )
    {
        Parallel.For(0, 64, s =>
        {
            int i = s % 8;
            int j = s / 8;
            int subN = n / 8;
            int subM = m / 8;

            if (i == 7)
                subN += n % 8;
            
            if (j == 7)
                subM += m % 8;

            int stpi = pi + i * subN;
            int stpj = pj + j * subM;
            int stqi = qi + i * subN;
            int stqj = qj + j * subM;

            iterativeSub(
                p, q,
                stpi, stpj, stqi, stqj, 
                subN, subM, n, m
            );
        });
    }
}