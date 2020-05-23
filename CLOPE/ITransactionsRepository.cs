using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace CLOPE
{
    interface ITransactionsRepository<T, K> where T : IEnumerable<K>
    {
        IEnumerable<Transaction<T, K>> GetTransactions(); 
    }
}
