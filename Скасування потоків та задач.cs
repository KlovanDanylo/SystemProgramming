using System;
using System.Threading;
using System.Diagnostics;

namespace SystemProgramming
{
    internal class Breakfast
    {
        private Stopwatch stopwatch = new();

        public void Run()
        {
            Console.WriteLine("=== СНІДАНОК ===\n");
            
            Console.WriteLine("1. Послідовне приготування:");
            SequentialBreakfast();
            
            Console.WriteLine("\n2. Оптимальне приготування:");
            ParallelBreakfast();
        }

        private void SequentialBreakfast()
        {
            stopwatch.Restart();
            
            MakeToast();
            MakeBacon();
            MakeCoffee();
            
            Console.WriteLine($"Загальний час: {stopwatch.Elapsed.TotalSeconds:F1} сек");
        }

        private void ParallelBreakfast()
        {
            stopwatch.Restart();
            
            Thread toastThread = new(MakeToast);
            Thread baconThread = new(MakeBacon);
            Thread coffeeThread = new(MakeCoffee);
            
            toastThread.Start();
            baconThread.Start();
            coffeeThread.Start();
            
            toastThread.Join();
            baconThread.Join();
            coffeeThread.Join();
            
            Console.WriteLine($"Загальний час: {stopwatch.Elapsed.TotalSeconds:F1} сек");
        }

        private void MakeToast()
        {
            Console.WriteLine($"[{stopwatch.Elapsed.TotalSeconds:F1} сек] Починаю смажити тост (5 сек)");
            Thread.Sleep(5000);
            Console.WriteLine($"[{stopwatch.Elapsed.TotalSeconds:F1} сек] Тост готовий!");
        }

        private void MakeBacon()
        {
            Console.WriteLine($"[{stopwatch.Elapsed.TotalSeconds:F1} сек] Починаю розігрівати бекон (3 сек)");
            Thread.Sleep(3000);
            Console.WriteLine($"[{stopwatch.Elapsed.TotalSeconds:F1} сек] Бекон готовий!");
        }

        private void MakeCoffee()
        {
            Console.WriteLine($"[{stopwatch.Elapsed.TotalSeconds:F1} сек] Ставлю чайник (10 сек)");
            Thread.Sleep(10000);
            Console.WriteLine($"[{stopwatch.Elapsed.TotalSeconds:F1} сек] Кава готова!");
        }
    }
}