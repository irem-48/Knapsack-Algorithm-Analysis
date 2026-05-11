using System;
using System.Diagnostics;
using System.IO;

namespace KnapsackProject
{
    public class DPSolver
    {
        public static int Coz(string dosya)
        {
            if (!File.Exists(dosya)) return 0;
            string[] s = File.ReadAllLines(dosya);
            string[] ilk = s[0].Split(' ');
            int n = int.Parse(ilk[0]), k = int.Parse(ilk[1]);
            int[] a = new int[n], d = new int[n];
            for (int i = 0; i < n; i++)
            {
                string[] p = s[i + 1].Split(' ');
                a[i] = int.Parse(p[0]); d[i] = int.Parse(p[1]);
            }

            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                int[,] matris = new int[n + 1, k + 1];
                for (int i = 1; i <= n; i++)
                {
                    for (int w = 0; w <= k; w++)
                    {
                        if (a[i - 1] <= w) matris[i, w] = Math.Max(d[i - 1] + matris[i - 1, w - a[i - 1]], matris[i - 1, w]);
                        else matris[i, w] = matris[i - 1, w];
                    }
                }
                sw.Stop();
                Console.WriteLine("--- DP Sonucu --- Deger: " + matris[n, k] + " | Sure: " + sw.Elapsed.TotalMilliseconds.ToString("F2") + " ms");
                return matris[n, k];
            }
            catch
            {
                Console.WriteLine("--- DP Hatasi --- Bellek yetersizligi! (Beklenen Analiz Verisi)");
                return 0;
            }
        }
    }
}