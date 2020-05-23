using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CLOPE
{
    class Transaction<T, K> where T: IEnumerable<K>
    {
        public int Id { get; }
        public T Value { get; }

        public Transaction(T value, int id)
        {
            (Id, Value) = (id, value);
        }
    }
}
