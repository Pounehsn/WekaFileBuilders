using System.Collections.Generic;

namespace Core
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<int> SumSequence(this IEnumerable<int> source)
        {
            var sum = 0;
            foreach (var i in source)
            {
                yield return sum += i;
            }
        }
    }
}
