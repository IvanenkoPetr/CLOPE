using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CLOPE
{
    class CLOPEСlusterer<T, K> where T : IEnumerable<K>
    {
        private readonly ITransactionsRepository<T, K> repository;
        private readonly IList<Cluster<T, K>> clusters;
        private readonly IEnumerable<Transaction<T,K>> transactions;
        private readonly decimal r;
        private bool isAlreadyClustered = false;

        private readonly Action AddEmptyCluster;

 
        public CLOPEСlusterer(ITransactionsRepository<T, K> repository, decimal r)
        {
            this.repository = repository;
            this.r = r;

            int id = 1;
            AddEmptyCluster = delegate ()
            {
                var newCluster = new Cluster<T, K>(id++);
                clusters.Add(newCluster);
            };

            transactions = repository.GetTransactions();
            clusters = new List<Cluster<T, K>>();
            AddEmptyCluster();
        }

        public IEnumerable<Cluster<T,K>> DoClustering()
        {
            if(!isAlreadyClustered)
            {
                FirstPhase();
                SecondPhase();
            }

            var result = clusters.Where(a => !a.IsEmpty);
            return result;

        }

        private void FirstPhase()
        {
            var bestChoice = 0;
            foreach (var tran in transactions)
            {
                var maxCost = 0m;
                foreach (var elem in clusters.Select((a, b) => new { cluster = a, index = b }))
                {
                    var addCost = elem.cluster.AddCost(tran, r);
                    if (addCost > maxCost)
                    {
                        maxCost = addCost;
                        bestChoice = elem.index;
                    }
                }
                if (clusters[bestChoice].IsEmpty)
                {
                    AddEmptyCluster();
                }
                clusters[bestChoice].AddTransaction(tran);
            }
        }

        private void SecondPhase()
        {
            bool moved;
            do
            {
                moved = false;
                foreach (var tran in transactions)
                {
                    var maxCost = 0m;
                    var clusterOfTransaction = GetClusterOfTransaction(tran);
                    var removeCost = clusterOfTransaction.RemoveCost(tran, r);
                    var bestChoice = 0;
                    foreach (var elem in clusters.Select((a, b) => new {index = b, cluster = a }))
                    {
                        if(elem.cluster.Id == clusterOfTransaction.Id)
                        {
                            continue;
                        }

                        var addCost = elem.cluster.AddCost(tran, r);
                        if(addCost + removeCost > maxCost)
                        {
                            maxCost = addCost + removeCost;
                            bestChoice = elem.index;
                        }
                    }

                    if(maxCost > 0)
                    {
                        if (clusters[bestChoice].IsEmpty)
                        {
                            AddEmptyCluster();
                        }

                        clusterOfTransaction.RemoveTransaction(tran);
                        clusters[bestChoice].AddTransaction(tran);
                        moved = true;
                    }                    
                }
            } while (moved);
        }

        private Cluster<T,K> GetClusterOfTransaction(Transaction<T,K> transaction)
        {
            var result = clusters.First(a => a.IsTransactionInCluster(transaction.Id));
            return result;
        }


    }
}
