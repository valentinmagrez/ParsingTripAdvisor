using System;
using System.Diagnostics;

namespace ConsoleApp2
{
    public class PerformanceAnalyser
    {
        public T Measure<T>(Func<T> f, string method)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var result = f();

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var ts = stopWatch.Elapsed;
            // Format and display the TimeSpan value.
            var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("-------->  RunTime "+ method +" : "+ elapsedTime);
            return result;
        }
    }
}
