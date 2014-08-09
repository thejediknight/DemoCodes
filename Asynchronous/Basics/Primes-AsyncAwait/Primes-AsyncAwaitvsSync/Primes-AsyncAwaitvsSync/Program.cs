using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Primes_AsyncAwaitvsSync
{
    class Program
    {
        //Synchronous method to find primes
        public static IEnumerable<int> getPrimes(int min, int count)
        {
            return Enumerable.Range(min, count).Where(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
        }

        //Doing it with Asynchronously. Simply change the return type to Task<>
        public static Task<IEnumerable<int>> getPrimesAsync(int min, int count)
        {
            return Task.Run(()=> Enumerable.Range(min, count).Where(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }

        //Sync Caller
        public static void PrintPrimes()
        {
            for (int i = 0; i < 10; i++)
            {
                getPrimes(i * 100000 + 1, i * 1000000).ToList().ForEach(x => Trace.WriteLine(x));
            }
        }

        //Async Caller. Await can only be used within an async function. 
        //'result' will be of type Task<>, and lines after await line will be executed on that Task<>
        public static async void PrintPrimesAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                var result = await getPrimesAsync(i * 100000 + 1, i * 1000000);
                result.ToList().ForEach(x => Trace.WriteLine(x));
            }
        }


        static void Main(string[] args)
        {
            Stopwatch s = new Stopwatch();

            s.Start();
            PrintPrimes();
            s.Stop();
            Console.WriteLine("Synchronous Prime Calculation took {0}",s.ElapsedMilliseconds/1000);
            Console.ReadKey();

            s.Reset();
            s.Start();
            PrintPrimesAsync();
            s.Stop();
            Console.WriteLine("Asynchronous Prime Calculation took {0}", s.ElapsedMilliseconds );

            Console.ReadKey();

        }
    }
}
