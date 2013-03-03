using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab1_RPQ
{
    public struct RPQ
    {
        public int R;
        public int P;
        public int Q;
        public int number;

        public RPQ(int _R, int _P, int _Q, int _number)
        {
            R = _R;
            P = _P;
            Q = _Q;
            number = _number;
        }
    }

    public class RPQOptimization
    {
        private List<RPQ> data;

        public void LoadData(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                bool isItFirstLine = true;
                int number = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (isItFirstLine)
                    {
                        isItFirstLine = false;
                        string[] splittedLine = line.Split('\t');
                        data = new List<RPQ>(Convert.ToInt32(splittedLine[0]));
                    }
                    else
                    {
                        if (line != String.Empty)
                        {
                            string[] splittedLine = line.Split('\t');
                            data.Add(new RPQ(
                                Convert.ToInt32(splittedLine[0]),
                                Convert.ToInt32(splittedLine[1]),
                                Convert.ToInt32(splittedLine[2]),
                                number++
                            ));
                        }
                    }
                }
            }
        }

        public void SortByP()
        {
            data = data.OrderBy(x => x.P).ToList();
        }

        public void SortByR()
        {
            data = data.OrderBy(x => x.R).ToList();
        }

        public void SortByMinusQ()
        {
            data = data.OrderBy(x => -x.Q).ToList();
        }

        public void SortByRMinusQ()
        {
            data = data.OrderBy(x => x.R - x.Q).ToList();
        }

        public void SortToHaveLowRAtStartAndLowQAtEnd()
        {
            var sortedByR = data.OrderBy(x => x.R).ToList();
            var sortedBQ = data.OrderBy(x => x.Q).ToList();

            LinkedList<RPQ> outputBeg = new LinkedList<RPQ>();
            LinkedList<RPQ> outputEnd = new LinkedList<RPQ>();

            int i = 0, j = 0;
            while (outputBeg.Count + outputEnd.Count < data.Count)
            {
                while (i < data.Count && outputBeg.Contains(sortedByR[i]) && outputEnd.Contains(sortedByR[i]))
                {
                    i++;
                }
                if (i < data.Count)
                {
                    outputBeg.AddLast(sortedByR[i]);
                }

                while (j < data.Count && outputBeg.Contains(sortedBQ[j]) && outputEnd.Contains(sortedBQ[j]))
                {
                    j++;
                }
                if (j < data.Count)
                {
                    outputEnd.AddFirst(sortedBQ[j]);
                }
            }

            data = outputBeg.Concat(outputEnd).ToList();
        }

        public int CalcCMax()
        {
            int t = 0;
            int CMax = 0;
            foreach (RPQ item in data)
            {
                t = Math.Max(item.R, t) + item.P;
                CMax = Math.Max(CMax, t + item.Q);
            }

            return CMax;
        }

        static void Swap(IList<RPQ> list, int indexA, int indexB)
        {
            RPQ tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public void TryToRandomlyImproveResoultBySwapping2Elements(int tries)
        {
            Random r = new Random();

            for (int i = 0; i < tries; i++)
            {
                int a = r.Next(data.Count);
                int b = r.Next(data.Count);
                int oldCMax = CalcCMax();

                Swap(data, a, b);
                int newCMax = CalcCMax();

                if (oldCMax < newCMax)
                {
                    Swap(data, a, b);
                }
            }
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            RPQOptimization opt1 = new RPQOptimization();
            opt1.LoadData("rpq_100.txt");

            Console.WriteLine("Start val: " + opt1.CalcCMax());

            opt1.TryToRandomlyImproveResoultBySwapping2Elements((int)Math.Pow(10, 6));
            Console.WriteLine("After 10^6 random swaps to improve: " + opt1.CalcCMax());

            opt1.SortByR();
            Console.WriteLine("Sorted by R: " + opt1.CalcCMax());

            opt1.SortByP();
            Console.WriteLine("Sorted by P: " + opt1.CalcCMax());

            opt1.SortByMinusQ();
            Console.WriteLine("Sorted by -Q: " + opt1.CalcCMax());

            opt1.SortByRMinusQ();
            Console.WriteLine("Sorted by R-Q: " + opt1.CalcCMax());

            opt1.SortToHaveLowRAtStartAndLowQAtEnd();
            Console.WriteLine("Spawek sort oO: " + opt1.CalcCMax());

            opt1.TryToRandomlyImproveResoultBySwapping2Elements((int)Math.Pow(10,6));
            Console.WriteLine("After 10^6 random swaps to improve: " + opt1.CalcCMax());

            Console.ReadKey();
        }
    }
}

