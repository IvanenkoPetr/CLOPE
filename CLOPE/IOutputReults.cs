using System;
using System.Collections.Generic;
using System.Text;

namespace CLOPE
{
    interface IReultsOutput<T, K> where T : IEnumerable<K>
    {
        void  Output(IEnumerable<Cluster<T, K>> clusters);
    }
}
