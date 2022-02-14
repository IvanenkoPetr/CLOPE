using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace CLOPE
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new StringTransactionRepository();
            var CLOPE = new CLOPEСlusterer<string, char>(repository, 1.6m);
            var clusters = CLOPE.DoClustering();

            IReultsOutput<string, char> reultsOutput = new ConsoleOutput();
            reultsOutput.Output(clusters);

            Console.ReadKey1();

        }
    }
}
