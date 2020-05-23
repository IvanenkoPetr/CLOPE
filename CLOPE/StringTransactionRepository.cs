using System;
using System.Collections.Generic;
using System.Text;

namespace CLOPE
{
    class StringTransactionRepository : ITransactionsRepository<string, char>
    {
        public IEnumerable<Transaction<string, char>> GetTransactions()
        {
            var result = new List<Transaction<string, char>>()
            {
                new Transaction<string, char>("abc", 1),
                new Transaction<string, char>("ab", 2),
                new Transaction<string, char>("cdq", 3),
                new Transaction<string, char>("qwz", 4),
                new Transaction<string, char>("a", 5),
                new Transaction<string, char>("q", 6),
                new Transaction<string, char>("qd", 7),
                new Transaction<string, char>("aq", 8),
                new Transaction<string, char>("f", 9),
                new Transaction<string, char>("acf", 10),
                new Transaction<string, char>("zzq", 11)
            };

            return result;
        }


    }
}
