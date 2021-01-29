using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextToSrt
{
    public static class EnumerableExtensions
    {
        public static T WithMinimum<T, TKey>(
            this IEnumerable<T> sequence, Func<T, TKey> selector) 
            where TKey : IComparable<TKey> =>
            sequence
                .Select(obj => (element: obj, key: selector(obj)))
                .Aggregate((best, cur) => best.key.CompareTo(cur.key) <= 0 ? best : cur)
                .element;

        public static IEnumerable<T> WithMinimumOrEmpty<T, TKey>(
            this IEnumerable<T> sequence, Func<T, TKey> selector) 
            where TKey : IComparable<TKey> =>
            sequence
                .Select(obj => (element: obj, key: selector(obj)))
                .Aggregate(
                    Enumerable.Empty<(T element, TKey key)>(),
                    (optionalBest, cur) => optionalBest.Where(
                        best => best.key.CompareTo(cur.key) <= 0).DefaultIfEmpty(cur))
                .Select(best => best.element);
    }
}
