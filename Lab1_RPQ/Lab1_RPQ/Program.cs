using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// task avitable at: http://dominik.zelazny.staff.iiar.pwr.wroc.pl/pliki/zad1/Zadanie_1.pdf

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
            var sortedByQ = data.OrderBy(x => x.Q).ToList();

            LinkedList<RPQ> outputBeg = new LinkedList<RPQ>();
            LinkedList<RPQ> outputEnd = new LinkedList<RPQ>();

            int i = 0, j = 0;
            while (outputBeg.Count + outputEnd.Count < data.Count)
            {
                while (outputBeg.Contains(sortedByR[i]) || outputEnd.Contains(sortedByR[i])) //find 1st elem from R sorted list wchich is not at output list
                {
                    i++;
                }
                if (i < data.Count)
                {
                    outputBeg.AddLast(sortedByR[i]);
                }

                while (outputBeg.Contains(sortedByQ[j]) || outputEnd.Contains(sortedByQ[j])) //find 1st elem from Q sorted list wchich is not at output list
                {
                    j++;
                }
                if (j < data.Count)
                {
                    outputEnd.AddFirst(sortedByQ[j]);
                }
            }

            data = outputBeg.Concat(outputEnd).ToList();
        }

        public int CalcCMax()
        {
            CheckPermutationCorectness();

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

        public void SavePermutationToFile(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                foreach (var item in data)
                {
                    sw.Write(item.number + "\t");
                }
            }
        }

        public void CheckPermutationCorectness()
        {
            List<int> occurances = new List<int>();
            for (int i = 0; i < data.Count; i++)
            {
                occurances.Add(0);
            }

            for (int i = 0; i < data.Count; i++)
            {
                occurances[data[i].number-1]++;
            }

            for (int i = 0; i < data.Count; i++)
            {
                if (occurances[i] != 1)
                {
                    throw new ApplicationException("wrong permutation!");
                }
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "rpq_100.txt";

            RPQOptimization opt1 = new RPQOptimization();
            opt1.LoadData(fileName);

            Console.WriteLine("Start val: " + opt1.CalcCMax());

            opt1.TryToRandomlyImproveResoultBySwapping2Elements((int)Math.Pow(10, 5));
            Console.WriteLine("After 10^5 random swaps to improve: " + opt1.CalcCMax());

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

            opt1.TryToRandomlyImproveResoultBySwapping2Elements((int)Math.Pow(10,5));
            Console.WriteLine("After 10^5 random swaps to improve: " + opt1.CalcCMax());

            string outputFileName = fileName.Replace("rpq", "out");
            opt1.SavePermutationToFile(outputFileName);
            Console.WriteLine("Last data set has been saved to file: {0}", outputFileName);

            Console.ReadKey();
        }
    }
}

