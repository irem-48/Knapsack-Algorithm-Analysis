using System;
using System.IO;

namespace KnapsackProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Veri Boyutlarını Belirle
            int[] veriBoyutlari = { 100, 1000, 10000 };

            // 2. Veri Setlerini Üret 
            Console.WriteLine("--- Adım 1: Veri Setleri Hazırlanıyor ---");
            foreach (int n in veriBoyutlari)
            {
                VeriUret(n);
            }

            Console.WriteLine("\n--- Adım 2: Karşılaştırmalı Performans Analizi Başlıyor ---\n");

            foreach (int n in veriBoyutlari)
            {
                string dosyaAdi = "veriseti_" + n + ".txt";
                Console.WriteLine($">>>>> Test Ediliyor: N = {n} <<<<<");

                // 3. Dinamik Programlama
                Console.ForegroundColor = ConsoleColor.Cyan;
                int dpSonuc = DPSolver.Coz(dosyaAdi);

                // 4. Genetik Algoritma (Parametre hatası giderildi: Varsayılan popülasyon 50 olarak eklendi)
                Console.ForegroundColor = ConsoleColor.Green;
                int gaSonuc = GASolver.Coz(dosyaAdi, 50);

                // 5. Accuracy Gap (Doğruluk Oranı) Hesaplama
                Console.ResetColor();
                if (dpSonuc > 0)
                {
                    // Formül: |DP - GA| / DP * 100
                    double gap = (double)Math.Abs(dpSonuc - gaSonuc) / dpSonuc * 100;
                    Console.WriteLine($"[ANALİZ] Accuracy Gap: %{gap:F2}");
                }
                else
                {
                    Console.WriteLine("[ANALİZ] DP sonucu alınamadı (Bellek yetersizliği veya süre aşımı).");
                }

                Console.WriteLine("----------------------------------------------------------\n");
            }

            // --- YENİ BÖLÜM: GA PARAMETRE DUYARLILIK ANALİZİ ---
            // Bu kısım, projenin akademik değerini artıran "Sensitivity Analysis" verilerini üretir.
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n--- ADIM 3: GA POPÜLASYON DUYARLILIK ANALİZİ (N=1000) ---");
            Console.ResetColor();

            string analizDosyasi = "veriseti_1000.txt";
            int[] testPopSize = { 10, 50, 100, 200 };

            foreach (int pSize in testPopSize)
            {
                // GASolver.Coz metodu içinde Stopwatch olduğu için her popülasyonun süresini ekranda göreceksin.
                GASolver.Coz(analizDosyasi, pSize);
            }

            Console.WriteLine("\n----------------------------------------------------------");
            Console.WriteLine("Tüm analizler tamamlandı. Raporlama için verileri tabloya aktarabilirsiniz.");
            Console.WriteLine("Çıkmak için bir tuşa basın...");
            Console.ReadKey();
        }

        // --- Veri üretme metodu orijinal haliyle korunmuştur ---
        static void VeriUret(int n)
        {
            Random rastgele = new Random();
            int toplamAgirlik = 0;

            int[] agirliklar = new int[n];
            int[] degerler = new int[n];

            for (int i = 0; i < n; i++)
            {
                agirliklar[i] = rastgele.Next(1, 51);
                degerler[i] = rastgele.Next(10, 501);
                toplamAgirlik += agirliklar[i];
            }

            int kapasite = (int)(toplamAgirlik * 0.5);
            string dosyaAdi = "veriseti_" + n + ".txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(dosyaAdi))
                {
                    sw.WriteLine(n + " " + kapasite);
                    for (int i = 0; i < n; i++)
                    {
                        sw.WriteLine(agirliklar[i] + " " + degerler[i]);
                    }
                }
                Console.WriteLine(">> " + dosyaAdi + " başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("!! Hata oluştu: " + ex.Message);
            }
        }
    }
}