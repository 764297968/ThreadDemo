using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadDemo
{
    public class ThreadDemo
    {
        //并行明显优势
        private Stopwatch watch = new Stopwatch();
        public void Run1()
        {
            Thread.Sleep(2000);
            Console.WriteLine("con 1");
        }
        public void Run2()
        {
            Thread.Sleep(3000);
            Console.WriteLine("con 2");
        }
        public void ParallelInvokeMethod()
        {
            watch.Start();
            Parallel.Invoke(Run1, Run2);
            watch.Stop();
            Console.WriteLine("parallel run "+watch.ElapsedMilliseconds+"ms");

            watch.Restart();
            Run1();
            Run2();
            watch.Stop();
            Console.WriteLine("run "+watch.ElapsedMilliseconds+"ms");
            Console.ReadLine();
        }
    }
}
