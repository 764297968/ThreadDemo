using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadDemo
{
    public class Plinq
    {
        //并行安全集合和默认集合
        public void AddList()
        {
            List<int> list = new List<int>();
            Parallel.For(1, 10000, item =>
            {
                list.Add(item);
            });
            Console.WriteLine("count" + list.Count);
        }
        public void ConcurrentBag()
        {
            ConcurrentBag<int> list = new ConcurrentBag<int>();
            Parallel.For(1, 100000, item =>
            {
                list.Add(item);
                //Console.WriteLine(item);
            });
            Console.WriteLine("count" + list.Count);
            Console.WriteLine(list.Distinct().Count());
        }
    }
}
