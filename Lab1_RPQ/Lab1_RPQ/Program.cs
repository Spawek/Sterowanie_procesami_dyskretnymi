using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab1_RPQ
{
    public class RPQ
    {
        public int R;
        public int P;
        public int Q;

        public RPQ(int _R, int _P, int _Q)
        {
            R = _R;
            P = _P;
            Q = _Q;
        }
    }

    public class RPQOptimization
    {
        private List<RPQ> data = new List<RPQ>(500);

        public void LoadData(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                bool isItFirstLine = true;
                while ((line = sr.ReadLine()) != null)
                {
                    if (isItFirstLine)
                    {
                        isItFirstLine = false;
                    }
                    else
                    {
                        if (line != String.Empty)
                        {
                            string[] splittedLine = line.Split('\t');
                            data.Add(new RPQ(
                                Convert.ToInt32(splittedLine[0]),
                                Convert.ToInt32(splittedLine[1]),
                                Convert.ToInt32(splittedLine[2])
                            ));
                        }
                    }
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
            
        }
    }
}
