using System;
using System.Collections.Generic;
using System.Text;

namespace CLOPE
{
    class ConsoleOutput : IReultsOutput<string, char>
    {
        public void Output(IEnumerable<Cluster<string, char>> clusters)
        {
            foreach (var cluster in clusters)
            {
                foreach(var tran in cluster.Transactions)
                {
                    Console.Write(tran.Value);
                    Console.Write(" | ");
                }

                Console.WriteLine();
            }
        }
    }
}
