using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLOPE
{
    class Cluster<T, K> where T : IEnumerable<K>
    {
        private readonly Dictionary<K, int> ElementsInCluster = new Dictionary<K, int>();
        private Dictionary<int, Transaction<T,K>> transactions;
      
        public IEnumerable<Transaction<T, K>> Transactions => transactions.Select(a=> a.Value);
        public int Id;
        public decimal S => ElementsInCluster.Sum(a => a.Value);
        public decimal N => transactions.Count;
        public decimal W => ElementsInCluster.Count();
        public bool IsEmpty => !transactions.Any();

        public Cluster(int id)
        {
            Id = id;
            transactions = new Dictionary<int, Transaction<T,K>>();
        }

        public bool IsTransactionInCluster(int id)
        {
            var result = transactions.Keys.Any(a => a == id);
            return result;
        }
        
        public void AddTransaction(Transaction<T,K> transaction)
        {
            transactions.Add(transaction.Id, transaction);

            foreach (var elem in transaction.Value)
            {
                if (ElementsInCluster.TryGetValue(elem, out var _))
                {
                    ElementsInCluster[elem] = ++ElementsInCluster[elem];
                }
                else
                {
                    ElementsInCluster.Add(elem, 1);
                }
            }
        }

        public void RemoveTransaction(Transaction<T, K> transaction)
        {
            transactions.Remove(transaction.Id);

            foreach (var elem in transaction.Value)
            {
                if (ElementsInCluster.TryGetValue(elem, out var _))
                {
                    ElementsInCluster[elem] = --ElementsInCluster[elem];
                }
            }
        }

        public decimal AddCost(Transaction<T, K> transaction, decimal r)
        {
            decimal S_new = S + transaction.Value.Count();
            decimal N_new = N + 1;
            decimal W_new = W;

            foreach (var elem in transaction.Value)
            {
                if (!ElementsInCluster.TryGetValue(elem, out var elemInDictionary) || elemInDictionary == default)
                {
                    W_new++;
                }
            }

            decimal result;
            if(S == default)
            {
                result = (S_new * N_new) / (decimal)Math.Pow((double)W_new, (double)r);
            }
            else
            {
                 result = (S_new * N_new) / (decimal)Math.Pow((double)W_new, (double)r) -
                    S * N / (decimal)Math.Pow((double)W, (double)r);
            }

            return result;
        }

        public decimal RemoveCost(Transaction<T, K> transaction, decimal r)
        {
            decimal S_new = S - transaction.Value.Count();
            decimal N_new = N - 1;
            decimal W_new = W;

            foreach (var elem in transaction.Value)
            {
                if (ElementsInCluster.TryGetValue(elem, out var elemInDictionary) && elemInDictionary == 1)
                {
                    W_new--;
                }
            }

            decimal result;
            if (S_new == default)
            {
                result = - S * N / (decimal)Math.Pow((double)W, (double)r);
            }
            else
            {
                result = (S_new * N_new) / (decimal)Math.Pow((double)W_new, (double)r) -
                   S * N / (decimal)Math.Pow((double)W, (double)r);
            }

            return result;
        }

    }
}
