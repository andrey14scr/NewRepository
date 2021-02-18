using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PapaMama
{
    class Program
    {
        public class SayClass
        {
            int ms;
            String word;
            int amount;
            private static int total;
            private static int max;
            public static object locker = new object();
            public SayClass(int ms, String word, int amount)
            {
                this.ms = ms;
                this.word = word;
                this.amount = amount;
            }
            public int MS { get { return ms; } }
            public static int Total { get { return total; } set { total = value; } }
            public String Word { get { return word; } }
            public int Amount { get { return amount; } }
            public static int Max { get { return max; } set { max = value; } }
        }
        public static void Say(object ob)
        {
            for (int i = 0; i < (ob as SayClass).Amount; ++i)
            {
                if (SayClass.Total >= SayClass.Max)
                {
                    Thread.CurrentThread.Abort();
                }
                lock (SayClass.locker)
                {
                    Console.WriteLine(Thread.CurrentThread.Name + "(" + (i + 1).ToString() + "/" + (ob as SayClass).Amount.ToString() + "): " + (ob as SayClass).Word);
                    SayClass.Total++;
                }
                Thread.Sleep((ob as SayClass).MS);
            }

        }
        static void Main(string[] args)
        {
            string[] tosaystr = new string[3];

            Console.Write("Введите количество потоков: ");
            int size = Convert.ToInt32(Console.ReadLine());
            Thread[] Threads = new Thread[size];
            SayClass[] SC = new SayClass[size];

            Console.Write("Введите общее количество выводов: ");
            SayClass.Max = Convert.ToInt32(Console.ReadLine());
            
            for (int i = 0; i < size; i++)
            {
                Console.Write("Введите " + (i+1).ToString() + " строку, интервал(мс) и количесвто повторений через пробел: ");
                tosaystr = Console.ReadLine().Split(' ');
                SC[i] = new SayClass(Convert.ToInt32(tosaystr[1]), tosaystr[0], Convert.ToInt32(tosaystr[2]));

                Threads[i] = new Thread(Say);
                Threads[i].Name = "Поток " + (i+1).ToString();
            }

            for (int i = 0; i < size; i++)
            {
                Threads[i].Start(SC[i]);
            }

            for (int i = 0; i < size; i++)
            {
                Threads[i].Join();
            }
        }
    }
}
