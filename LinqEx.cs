using System.Collections.Generic;

public static class LinqEx
{
    public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count)
    {
        IEnumerator<T> enumerator = source.GetEnumerator();
        Queue<T> queue = new Queue<T>(count + 1);
        while (enumerator.MoveNext())
        {
            queue.Enqueue(enumerator.Current);
            if (queue.Count > count)
            {
                yield return queue.Dequeue();
            }
        }
    }
}
