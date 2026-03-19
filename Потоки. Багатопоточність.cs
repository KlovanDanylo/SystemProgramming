using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SystemProgramming
{
    // Оголошення класу Point
    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }

        public Point(int x, int y, string name)
        {
            X = x;
            Y = y;
            Name = name;
        }
    }

    internal class ThreadPooling
    {
        private long startTicks;
        private readonly Semaphore semaphore = new(0, 1);

        public void Run()
        {
            startTicks = DateTime.Now.Ticks;
            
            // Правильні аргументи - передаємо об'єкти Point
            ThreadPool.QueueUserWorkItem(PrintPoint, new Point(10, 20, "A"));
            ThreadPool.QueueUserWorkItem(PrintPoint, new Point(30, 40, "B"));
            ThreadPool.QueueUserWorkItem(PrintPoint, new Point(50, 60, "C"));
            
            // Неправильні аргументи - передаємо не Point
            ThreadPool.QueueUserWorkItem(PrintPoint, 123);                    
            ThreadPool.QueueUserWorkItem(PrintPoint, "Hello");               
            ThreadPool.QueueUserWorkItem(PrintPoint, new object());          
            ThreadPool.QueueUserWorkItem(PrintPoint, null);                  

            // Чекаємо завершення всіх потоків
            Thread.Sleep(3000);
        }

        // Потоковий метод, що приймає точку
        private void PrintPoint(object? arg)
        {
            if (arg is Point point)
            {
                // Якщо це Point - виводимо координати
                Console.WriteLine($"Точка \"{point.Name}\" ({point.X};{point.Y})");
            }
            else
            {
                // Якщо це не Point - виводимо повідомлення
                string typeName = arg?.GetType().Name ?? "NULL";
                Console.WriteLine($"Отримано неправильний тип даних: {typeName}. Очікується Point.");
            }
        }

        // Інші методи класу (залишаються без змін)
        private double sum;
        private readonly Object sumLocker = new();
        private int cnt;

        private void LoadPercent(Object? arg)
        {
            if (arg is int month)
            {
                Console.WriteLine($"Load start month {month}");
                Thread.Sleep(1000);
                double percent = month;
                double k = 1.0 + percent / 100.0;
                double res;
                bool isLast;
                lock (sumLocker)
                {
                    res = sum;
                    res = res * k;
                    sum = res;
                    cnt = cnt - 1;
                    isLast = cnt == 0;
                }
                Console.WriteLine($"Load finish month {month}, sum = {res}");

                if (isLast)
                {
                    Console.WriteLine($"Total: {res}");
                    semaphore.Release();
                }
            }
            else
            {
                Console.WriteLine("arg must be int, not " + (arg?.GetType().Name ?? "NULL"));
            }
        }
    }
}