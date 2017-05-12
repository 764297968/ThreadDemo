using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ThreadPool
{
    public class ParallelDemo
      {
         private Stopwatch stopWatch = new Stopwatch();

         public void Run1()
         {
            Thread.Sleep(2000);
            Console.WriteLine("Task 1 is cost 2 sec");
            throw new Exception("Exception in task 1");
         }
         public void Run2()
         {
            Thread.Sleep(3000);
            Console.WriteLine("Task 2 is cost 3 sec");
            throw new Exception("Exception in task 2");
         }

         public void ParallelInvokeMethod()
         {
            stopWatch.Start();
            try
            {
               Parallel.Invoke(Run1, Run2);
            }
            catch (AggregateException aex)
            {
               foreach (var ex in aex.InnerExceptions)
               {
                  Console.WriteLine(ex.Message);
               }
            }
            stopWatch.Stop();
            Console.WriteLine("Parallel run " + stopWatch.ElapsedMilliseconds + " ms.");

            stopWatch.Reset();
            stopWatch.Start();
            try
            {
               Run1();
               Run2();
            }
            catch(Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
            stopWatch.Stop();
            Console.WriteLine("Normal run " + stopWatch.ElapsedMilliseconds + " ms.");
         }


         public void ParallelForMethod()
         {
            Parallel.For(0, 100, i =>
            {
               Console.Write(i + "\t");
            });
            var obj = new Object();
            long num = 0;
            ConcurrentBag<long> bag = new ConcurrentBag<long>();

            stopWatch.Start();
            for (int i = 0; i < 10000; i++)
            {
               for (int j = 0; j < 60000; j++)
               {
                  //int sum = 0;
                  //sum += item;
                  num++;
               }
            }
            stopWatch.Stop();
            Console.WriteLine("NormalFor run " + stopWatch.ElapsedMilliseconds + " ms.");

            stopWatch.Reset();
            stopWatch.Start();
            Parallel.For(0, 10000, item =>
            {
               for (int j = 0; j < 60000; j++)
               {
                  //int sum = 0;
                  //sum += item;
                  lock (obj)
                  {
                     num++;
                  }
               }
            });
            stopWatch.Stop();
            Console.WriteLine("ParallelFor run " + stopWatch.ElapsedMilliseconds + " ms.");
           
         }

         public void ParallelForeachMethod()
         {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            stopWatch.Start();

            for (int i = 0; i < 3000000; i++)
            {
               bag.Add(i);
            }

            stopWatch.Stop();
            Console.WriteLine("NormalForeach run " + stopWatch.ElapsedMilliseconds + " ms.");
            GC.Collect();

            bag = new ConcurrentBag<int>();

            stopWatch.Restart();

            Parallel.ForEach(Partitioner.Create(0, 3000000), i =>
            {
               for (int m = i.Item1; m < i.Item2; m++)
               {
                  bag.Add(m);
               }
            });

            stopWatch.Stop();
            Console.WriteLine("ParallelForeach run " + stopWatch.ElapsedMilliseconds + " ms.");
            GC.Collect();
         }

         public void ParallelBreak()
         {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();
            stopWatch.Start();
            Parallel.For(0, 1000, (i, state) =>
            {
               if (bag.Count == 30)
               {
                  state.Stop();
                  return;
               }
               bag.Add(i);
            });
            stopWatch.Stop();
            Console.WriteLine("Bag count is " + bag.Count + ", " + stopWatch.ElapsedMilliseconds);
         }
         
      }
   }
