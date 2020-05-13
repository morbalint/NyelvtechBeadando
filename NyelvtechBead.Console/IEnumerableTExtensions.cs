using System;
using System.Collections.Generic;
using System.Linq;

namespace NyelvtechBead.Console
{
    public static class IEnumerableTExtensions 
    {
        public static IEnumerable<U> Select<T, S, U>(this IEnumerable<T> source, Func<S, T, (S, U)> selector, S initialState = default)
        {
            foreach(var item in source)
            {
                var (state, output) = selector(initialState, item);
                yield return output;
                initialState = state;
            }
        }

        public static IEnumerable<U> Select<T, S, U>(this IEnumerable<T> source, Func<int, S, T, (S, U)> selector, S initialState = default)
        {
            int i = default;
            foreach (var item in source)
            {
                var (state, output) = selector(i++, initialState, item);
                yield return output;
                initialState = state;
            }
        }

        public static IEnumerable<T[]> Window<T>(this IEnumerable<T>source, int width)
        {
            var buffer = new Queue<T>();
            foreach (var item in source)
            {
                buffer.Enqueue(item);
                if(buffer.Count == width)
                {
                    yield return buffer.ToArray();
                    buffer.Dequeue();
                }
            }
        }

        public static IEnumerable<T> Pad<T>(this IEnumerable<T> source, T padding = default, int width = 1) 
            => Enumerable.Repeat(padding, width).Concat(source).Concat(Enumerable.Repeat(padding, width));
    }
}
