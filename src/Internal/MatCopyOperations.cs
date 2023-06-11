using System.Threading.Tasks;

namespace Scratch.Mathematics.Mat.Internal;

internal static class MatCopyOperations
{
    internal static unsafe void Copy(
        float* target, float* source,
        int n, int m
    )
    {
        int len = n * m;
        if (len < 256)
            iterativeCopy(target, source, n, m);
        else parallelCopy(target, source, n, m);
    }

    private static unsafe void iterativeCopy(
        float* target, float* source,
        int n, int m
    )
    {
        const int jump = 8;
        int len = n * m;
        var end = source + len - jump;

        do 
        {
            *(target + 0) = *(source + 0);
            *(target + 1) = *(source + 1);
            *(target + 2) = *(source + 2);
            *(target + 3) = *(source + 3);
            *(target + 4) = *(source + 4);
            *(target + 5) = *(source + 5);
            *(target + 6) = *(source + 6);
            *(target + 7) = *(source + 7);

            source += jump;
            target += jump;
        }
        while(source < end);

        end += jump;
        do
        {
            *source = *target;

            source++;
            target++;
        } while (source < end);
    }

    private static unsafe void parallelCopy(
        float* target, float* source,
        int n, int m
    )
    {
        int len = n * m;
        const int threadData = 256;

        Parallel.For(0, len / threadData, k =>
        {
            const int jump = 8;
            var st = source + k * threadData;
            var tg = target + k * threadData;
            var end = st + threadData - jump;

            do 
            {
                *(tg + 0) = *(st + 0);
                *(tg + 1) = *(st + 1);
                *(tg + 2) = *(st + 2);
                *(tg + 3) = *(st + 3);
                *(tg + 4) = *(st + 4);
                *(tg + 5) = *(st + 5);
                *(tg + 6) = *(st + 6);
                *(tg + 7) = *(st + 7);

                st += jump;
                tg += jump;
            }
            while(st < end);

            end += jump;
            do
            {
                *st = *tg;

                st++;
                tg++;
            } while (st < end);
        });
    }
}