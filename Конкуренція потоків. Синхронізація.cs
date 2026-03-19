using System;
using System.Threading;

namespace SystemProgramming
{
    internal class ThreadPooling
    {
        private long startTicks;
        private readonly Semaphore semaphore = new(0, 1);
        private string result = "";
        private readonly object locker = new();
        private int counter;

        public void Run()
        {
            Console.Write("Enter num: ");
            int n = int.Parse(Console.ReadLine() ?? "10");
            
            startTicks = DateTime.Now.Ticks;
            counter = n;
            string letters = "abcdefghijklmnopqrstuvwxyz"[..n];

            for (int i = 0; i < n; i++)
            {
                int index = i;
                ThreadPool.QueueUserWorkItem(ProcessLetter, new { Index = index, Letter = letters[index] });
            }

            semaphore.WaitOne();
            Console.WriteLine($"Result: {result}");
        }

        private void ProcessLetter(object? arg)
        {
            dynamic data = arg!;
            int index = data.Index;
            char letter = data.Letter;

            Console.WriteLine($"Processing letter {index + 1} ({letter}): started");
            Thread.Sleep(new Random().Next(100, 500));
            
            string current;
            bool isLast;
            
            lock (locker)
            {
                result += letter;
                current = result;
                counter--;
                isLast = counter == 0;
                Console.WriteLine($"Processing letter {index + 1} ({letter}): {current}");
            }
            
            Thread.Sleep(new Random().Next(100, 500));
            
            if (isLast)
            {
                semaphore.Release();
            }
        }
    }
}