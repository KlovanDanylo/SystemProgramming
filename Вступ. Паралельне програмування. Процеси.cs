using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SystemProgramming
{
    internal class ThreadingDemo
    {
        public void Run()
        {
            Console.WriteLine("Threading Demo");

            try
            {
                new Thread(ThreadAction2).Start(10);
                new Thread(ThreadAction2).Start("A");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Threading Demo End");
        }

        private void ThreadAction2(Object? arg)
        {
            if (arg is int)
            {
                Console.WriteLine($"Threading Action2 Begin with arg={arg}");
                Thread.Sleep(1000);
                Console.WriteLine($"Threading Action2 End with arg={arg}");
            }
            else
            {
                throw new ArgumentException("Only int arg");
            }
        }

        public void ShowProcesses()
        {
            var processes = Process.GetProcesses()
                .OrderBy(p => p.ProcessName)
                .Select(p => $"{p.ProcessName,-30} {p.Id,8} {p.WorkingSet64 / 1024 / 1024,5} MB");

            foreach (var p in processes.Take(20))
                Console.WriteLine(p);
        }

        public void OpenWebsite()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://itstep.org",
                    UseShellExecute = true
                });
                Console.WriteLine("Сайт itstep.org відкрито у браузері");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        public void OpenProcessesInNotepad()
        {
            try
            {
                string fileName = $"processes_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string filePath = Path.Combine(Path.GetTempPath(), fileName);

                var processes = Process.GetProcesses()
                    .OrderBy(p => p.ProcessName)
                    .Select(p => $"{p.ProcessName,-30} {p.Id,8} {p.WorkingSet64 / 1024 / 1024,5} MB");

                File.WriteAllLines(filePath, processes);
                Console.WriteLine($"Файл збережено: {filePath}");

                Process.Start(new ProcessStartInfo
                {
                    FileName = "notepad.exe",
                    Arguments = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}