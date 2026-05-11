using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace KnapsackProject
{
    public class GASolver
    {
        // Sabit parametreler (Analiz için Coz metoduna taşınabilirler)
        static int nesilSayisi = 100;
        static double mutasyonOrani = 0.05;

        // Metot artık dışarıdan popBoyutu parametresini alıyor
        public static int Coz(string dosyaAdi, int popBoyutu)
        {
            if (!File.Exists(dosyaAdi)) return 0;

            // 1. Veri Okuma
            string[] satirlar = File.ReadAllLines(dosyaAdi);
            string[] ilkSatir = satirlar[0].Split(' ');
            int n = int.Parse(ilkSatir[0]);
            int kapasite = int.Parse(ilkSatir[1]);

            int[] agirliklar = new int[n];
            int[] degerler = new int[n];

            for (int i = 0; i < n; i++)
            {
                string[] parcalar = satirlar[i + 1].Split(' ');
                agirliklar[i] = int.Parse(parcalar[0]);
                degerler[i] = int.Parse(parcalar[1]);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // 2. İlk Popülasyon (popBoyutu parametresi kullanılıyor)
            Random rnd = new Random();
            List<bool[]> populasyon = new List<bool[]>();
            for (int i = 0; i < popBoyutu; i++)
            {
                populasyon.Add(RastgeleCozumUret(n, agirliklar, kapasite, rnd));
            }

            // 3. Evrim Döngüsü
            for (int g = 0; g < nesilSayisi; g++)
            {
                // En iyileri sırala
                populasyon = populasyon.OrderByDescending(c => UygunlukHesapla(c, degerler, agirliklar, kapasite)).ToList();

                // Elitizm: En iyi 10'u koru (Popülasyon 10'dan küçükse tamamını korur)
                List<bool[]> yeniNesil = populasyon.Take(Math.Min(10, popBoyutu)).ToList();

                while (yeniNesil.Count < popBoyutu)
                {
                    // Seçilim ve Çaprazlama
                    // Seçilim havuzunu popülasyon boyutuna göre dinamik tutuyoruz
                    int secimSiniri = Math.Min(20, popBoyutu);
                    bool[] anne = populasyon[rnd.Next(0, secimSiniri)];
                    bool[] baba = populasyon[rnd.Next(0, secimSiniri)];
                    bool[] cocuk = Caprazla(anne, baba, rnd);

                    // Mutasyon
                    if (rnd.NextDouble() < mutasyonOrani) MutasyonYap(cocuk, rnd);

                    yeniNesil.Add(cocuk);
                }
                populasyon = yeniNesil;
            }

            sw.Stop();
            var enIyiBirey = populasyon.OrderByDescending(c => UygunlukHesapla(c, degerler, agirliklar, kapasite)).First();
            int bulunanDeger = UygunlukHesapla(enIyiBirey, degerler, agirliklar, kapasite);

            Console.WriteLine($"--- {dosyaAdi} Sonuclari (GA | Pop: {popBoyutu}) ---");
            Console.WriteLine($"Bulunan En İyi Değer: {bulunanDeger}");
            Console.WriteLine($"Hesaplama Süresi: {sw.Elapsed.TotalMilliseconds:F2} ms\n");

            return bulunanDeger;
        }

        // --- Değiştirilmemesi gereken yardımcı metotlar aynen korunmuştur ---
        static bool[] RastgeleCozumUret(int n, int[] agirliklar, int kapasite, Random rnd)
        {
            bool[] cozum = new bool[n];
            int mevcutAgirlik = 0;
            for (int i = 0; i < n; i++)
            {
                if (rnd.Next(0, 2) == 1 && mevcutAgirlik + agirliklar[i] <= kapasite)
                {
                    cozum[i] = true;
                    mevcutAgirlik += agirliklar[i];
                }
            }
            return cozum;
        }

        static int UygunlukHesapla(bool[] cozum, int[] degerler, int[] agirliklar, int kapasite)
        {
            int toplamDeger = 0, toplamAgirlik = 0;
            for (int i = 0; i < cozum.Length; i++)
            {
                if (cozum[i])
                {
                    toplamAgirlik += agirliklar[i];
                    toplamDeger += degerler[i];
                }
            }
            return toplamAgirlik > kapasite ? 0 : toplamDeger;
        }

        static bool[] Caprazla(bool[] anne, bool[] baba, Random rnd)
        {
            int kesim = rnd.Next(1, anne.Length - 1);
            bool[] cocuk = new bool[anne.Length];
            Array.Copy(anne, 0, cocuk, 0, kesim);
            Array.Copy(baba, kesim, cocuk, kesim, anne.Length - kesim);
            return cocuk;
        }

        static void MutasyonYap(bool[] cozum, Random rnd)
        {
            int index = rnd.Next(cozum.Length);
            cozum[index] = !cozum[index];
        }
    }
}