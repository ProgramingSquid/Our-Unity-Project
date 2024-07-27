using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplexNoise
{
    private int[] perm;
    private int[] permMod12;

    public SimplexNoise(int seed)
    {
        perm = new int[512];
        permMod12 = new int[512];
        System.Random random = new System.Random(seed);
        int[] p = new int[256];
        for (int i = 0; i < 256; i++)
        {
            p[i] = i;
        }
        for (int i = 255; i > 0; i--)
        {
            int swapIndex = random.Next(i + 1);
            int temp = p[i];
            p[i] = p[swapIndex];
            p[swapIndex] = temp;
        }
        for (int i = 0; i < 512; i++)
        {
            perm[i] = p[i & 255];
            permMod12[i] = perm[i] % 12;
        }
    }

    private float dot(int[] g, float x, float y)
    {
        return g[0] * x + g[1] * y;
    }

    private static int[][] grad3 = {
        new int[] {1,1,0}, new int[] {-1,1,0}, new int[] {1,-1,0}, new int[] {-1,-1,0},
        new int[] {1,0,1}, new int[] {-1,0,1}, new int[] {1,0,-1}, new int[] {-1,0,-1},
        new int[] {0,1,1}, new int[] {0,-1,1}, new int[] {0,1,-1}, new int[] {0,-1,-1}
    };

    public float Calculate(float xin, float yin)
    {
        float s = (xin + yin) * 0.5f * (Mathf.Sqrt(3.0f) - 1.0f);
        int i = Mathf.FloorToInt(xin + s);
        int j = Mathf.FloorToInt(yin + s);
        float t = (i + j) * (3.0f - Mathf.Sqrt(3.0f)) / 6.0f;
        float X0 = i - t;
        float Y0 = j - t;
        float x0 = xin - X0;
        float y0 = yin - Y0;

        int i1, j1;
        if (x0 > y0)
        {
            i1 = 1; j1 = 0;
        }
        else
        {
            i1 = 0; j1 = 1;
        }

        float x1 = x0 - i1 + (3.0f - Mathf.Sqrt(3.0f)) / 6.0f;
        float y1 = y0 - j1 + (3.0f - Mathf.Sqrt(3.0f)) / 6.0f;
        float x2 = x0 - 1.0f + (3.0f - Mathf.Sqrt(3.0f)) / 3.0f;
        float y2 = y0 - 1.0f + (3.0f - Mathf.Sqrt(3.0f)) / 3.0f;

        int ii = i & 255;
        int jj = j & 255;
        int gi0 = permMod12[ii + perm[jj]];
        int gi1 = permMod12[ii + i1 + perm[jj + j1]];
        int gi2 = permMod12[ii + 1 + perm[jj + 1]];

        float t0 = 0.5f - x0 * x0 - y0 * y0;
        float n0;
        if (t0 < 0) n0 = 0.0f;
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * dot(grad3[gi0], x0, y0);
        }

        float t1 = 0.5f - x1 * x1 - y1 * y1;
        float n1;
        if (t1 < 0) n1 = 0.0f;
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * dot(grad3[gi1], x1, y1);
        }

        float t2 = 0.5f - x2 * x2 - y2 * y2;
        float n2;
        if (t2 < 0) n2 = 0.0f;
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * dot(grad3[gi2], x2, y2);
        }

        return 70.0f * (n0 + n1 + n2);
    }
}
