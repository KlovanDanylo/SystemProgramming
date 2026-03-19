using System;
using System.Threading;

namespace SystemProgramming
{
    internal class ThreadLimitation
    {
        private readonly Semaphore semaphore = new(0, 3); // максимум 3 потоки одночасно
        private long startTicks;
        private string result = "";
        private readonly object locker = new();

        public void Run()
        {
            Console.Write("Enter num: ");
            int n = int.Parse(Console.ReadLine() ?? "10");
            
            startTicks = DateTime.Now.Ticks;
            string letters = "abcdefghijklmnopqrstuvwxyz"[..n];
            
            Thread[] threads = new Thread[n];
            for (int i = 0; i < n; i++)
            {
                int index = i;
                threads[i] = new Thread(() => ProcessLetter(index, letters[index]));
                threads[i].Start();
            }

            Console.WriteLine($"{(DateTime.Now.Ticks - startTicks) / 1e7:F1} Loop finish - threads ready. Pause before start");
            Thread.Sleep(1000);
            Console.WriteLine($"{(DateTime.Now.Ticks - startTicks) / 1e7:F1} Semaphore released");
            semaphore.Release(3);

            foreach (Thread thread in threads)
                thread.Join();

            Console.WriteLine($"Result: {result}");
        }

        private void ProcessLetter(int index, char letter)
        {
            semaphore.WaitOne();
            
            Console.WriteLine($"{(DateTime.Now.Ticks - startTicks) / 1e7:F1} Processing letter {index + 1} ({letter}): starting");
            Thread.Sleep(500);
            
            lock (locker)
            {
                result += letter;
                Console.WriteLine($"Processing letter {index + 1} ({letter}): {result}");
            }
            
            Thread.Sleep(500);
            Console.WriteLine($"{(DateTime.Now.Ticks - startTicks) / 1e7:F1} Thread {index + 1} Finish");
            
            semaphore.Release();
        }
    }
}