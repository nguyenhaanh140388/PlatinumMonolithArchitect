using System;
using System.Collections.Generic;
using System.Linq;
using Anhny010920.Core.Extensions;
using Platinum.Core.AAA;

namespace Platinum.Core.Extensions
{
    /// <summary>
    /// Provides extensions for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary> An Enumerable extension method that determine if two Enumerables are equal. </summary>
        /// <remarks> The order of the items must also match. </remarks>
        /// <param name="collectionA"> The Enumerable a to act on. </param>
        /// <param name="collectionB"> The Enumerable b to compare with. </param>
        /// <returns> true if equal, false if not. </returns>
        public static bool AreEqual(this IEnumerable<object> collectionA, IEnumerable<object> collectionB)
        {
            IEnumerator<object> aEnumerator = null;
            IEnumerator<object> bEnumerator = null;

            try
            {
                aEnumerator = collectionA.GetEnumerator();
                bEnumerator = collectionB.GetEnumerator();

                while (aEnumerator.MoveNext() && bEnumerator.MoveNext())
                {
                    if (aEnumerator.Current != null && bEnumerator.Current != null && aEnumerator.Current.Equals(bEnumerator.Current)) continue;
                    return false;
                }

                return true;
            }
            finally
            {
                aEnumerator?.Dispose();
                bEnumerator?.Dispose();
            }
        }

        /// <summary> An Enumerable extension method that splits a list into smaller Enumerables. </summary>
        /// <param name="source"> The source to act on. </param>
        /// <param name="splitSize"> Size of the split. </param>
        /// <returns> An Enumerable with Enumerables </returns>
        public static IEnumerable<IEnumerable<object>> Split(this IEnumerable<object> source, int splitSize) =>
            source.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / splitSize).Select(x => x.Select(v => v.Value));

        /// <summary>
        ///     Check if an enumerable is empty.
        /// </summary>
        /// <remarks> Inverse method of System.Linq.Any. </remarks>
        /// <param name="source"> The source to act on. </param>
        /// <returns> True if the enumerable is empty, otherwise false. </returns>
        public static bool Empty<T>(this IEnumerable<T> source) => !source.Any();

        /// <summary>
        ///     Check if none of the elements in <paramref name="source" /> match the <paramref name="criteria" />.
        /// </summary>
        /// <remarks> Inverse method of `System.Linq.Any` with a <paramref name="criteria" /> as input. </remarks>
        /// <example>
        ///     `System.Linq.Any`
        ///     <code>
        ///     if (!list.Any(criteria)) { ... }
        ///     </code>
        ///     Exstension method
        ///     <code>
        ///     if (list.None(criteria)) { ... }
        ///     </code>
        ///     One can argue that the extension method is much more readable, it is more clear what the intent is, and you have
        ///     less chance of missing a `!` which you need to add to `System.Linq.Any` for the inverse result.
        /// </example>
        /// <param name="source"> The source to act on. </param>
        /// <param name="criteria"> The criteria to match on. </param>
        /// <returns>
        ///     True if none of the elements in <paramref name="source" /> match the <paramref name="criteria" />, otherwise
        ///     false.
        /// </returns>
        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> criteria) => !source.Any(criteria);

        /// <summary>
        /// Creates a new Array containing the <typeparamref name="T"/> item.
        /// </summary>
        public static T[] AsArray<T>(this T item)
        {
            return new[] { item };
        }

        /// <summary>
        /// Creates a new List containing the <typeparamref name="T"/> item.
        /// </summary>
        public static List<T> AsList<T>(this T item)
        {
            return new List<T> { item };
        }

        /// <summary>
        /// Performs a DistinctBy the specified key instead of using an IEqualityComparer.
        /// </summary>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            var seenKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (seenKeys.Add(selector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Orders the items in the IEnumerable in a random sequence.
        /// </summary>
        public static IEnumerable<T> OrderByRandom<T>(this IEnumerable<T> query)
        {
            return query.OrderBy(q => Guid.NewGuid());
        }

        /// <summary>
        /// Removes all matching items from an <see cref="IList{T}"/>.
        /// </summary>
        public static void RemoveAll<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            for (var i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i--);
                }
            }
        }

        /// <summary>
        /// Removes all matching items from an <see cref="ICollection{T}"/>.
        /// </summary>
        public static void RemoveAll<T>(this ICollection<T> list, Func<T, bool> predicate)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var matches = list.Where(predicate).ToArray();
            foreach (var match in matches)
            {
                list.Remove(match);
            }
        }

        /// <summary>
        /// If <paramref name="input"/> is null, an empty instance of <see cref="IEnumerable{T}"/> will be returned. Otherwise, the existing instance will be returned.
        /// </summary>
        public static IEnumerable<T> IfNullThenEmpty<T>(this IEnumerable<T> input)
        {
            return input ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Applies a filter on a sequence only if <paramref name="condition"/> is met.
        /// </summary>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return condition
                ? source.Where(predicate)
                : source;
        }

        /// <summary>
        /// Applies a filter on a sequence only if <paramref name="condition"/> is met.
        /// </summary>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return condition
                ? source.Where(predicate)
                : source;
        }

        /// <summary>
        /// Bypasses a specified number of items in a sequence only if <paramref name="condition"/> is met.
        /// </summary>
        public static IEnumerable<TSource> SkipIf<TSource>(this IEnumerable<TSource> source, bool condition, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return condition
                ? source.Skip(count)
                : source;
        }

        /// <summary>
        /// Returns true if all items in the other enumerable exist in the source enumerable.
        /// </summary>
        public static bool ContainsAll<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return !other.Except(source).Any();
        }

        /// <summary>
        /// Returns true if the source enumerable contains any of the items in the other enumerable.
        /// </summary>
        public static bool ContainsAny<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return other.Any(source.Contains);
        }

        /// <summary>
        /// Splits the source enumerable into a series of partitions.
        /// </summary>
        /// <param name="source">IEnumerable instance to split</param>
        /// <param name="size">Size of each partition.</param>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater than 0.");
            }

            var chunk = new List<T>(size);

            foreach (var x in source)
            {
                chunk.Add(x);

                if (chunk.Count < size)
                {
                    continue;
                }

                yield return chunk;

                chunk = new List<T>(size);
            }

            if (chunk.Any())
            {
                yield return chunk;
            }
        }

        /// <summary>
        /// Filters a sequence to ignore items which are null.
        /// </summary>
        /// <returns>A filtered sequence, where all items are not null.</returns>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> source)
            where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(x => x != null);
        }
    }
}
