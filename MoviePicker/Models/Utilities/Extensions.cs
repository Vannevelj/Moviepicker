using System.Collections.Generic;

namespace Models.Utilities
{
    public static class Extensions
    {
        public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            var list = destination as List<T>;

            if (list != null)
            {
                list.AddRange(source);
            }
            else
            {
                foreach (var item in source)
                {
                    destination.Add(item);
                }
            }
        }
    }
}