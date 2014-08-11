using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class IEnumerableExt {
    public static IEnumerable<TSource> SelectManyRecursive<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> getChildrenFunction) {
        List<TSource> list = new List<TSource>();
        foreach (var item in source) {
            // Add what we have to the stack
            list.Add(item);

            // Go through the input enumerable looking for children,
            // and add those if we have them
            var children = getChildrenFunction(item);
            if (children != null) {
                foreach (TSource element in children) {
                    list.AddRange(element.SelectManyRecursive(getChildrenFunction));
                }
            }
        }
        return list;
    }
    public static IEnumerable<TSource> SelectManyRecursive<TSource>(this TSource source, Func<TSource, IEnumerable<TSource>> getChildrenFunction) {
        // Add what we have to the stack
        List<TSource> list = new List<TSource>();
        list.Add(source);

        // Go through the input enumerable looking for children,
        // and add those if we have them
        var children = getChildrenFunction(source);
        if (children != null) {
            foreach (TSource element in children) {
                list.AddRange(element.SelectManyRecursive(getChildrenFunction));
            }
        }
        return list;
    }

    private static Random rand = new Random();
    public static TSource SelectRandom<TSource>(this IList<TSource> source) {
        return source[rand.Next(0, source.Count)];
    }
    public static TSource SelectRandom<TSource>(this TSource[] source) {
        return source[rand.Next(0, source.Length)];
    }
}
