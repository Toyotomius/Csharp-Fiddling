using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SillyBenchmark
{
    internal class Program
    {


        private static void Main()
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            var lotto649 = new Lotto649();
            var lottoExample = new LottoExample();

            var tasks = new List<Task>();
            for (int i = 0; i < 30; i++)
            {
                tasks.Add(Task.Run(() => lotto649.L649Async()));
                tasks.Add(Task.Run(() => lottoExample.LEAsync()));
            }

            var t = Task.WhenAll(tasks);
            t.Wait();
            watch.Stop();
            Console.WriteLine($"Total Execution Time: {watch.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }
    }
}
// Execution Time: ~305 ms. When published as exe & run from cmd.
