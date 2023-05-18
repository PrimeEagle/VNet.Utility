using System.Collections.Generic;
using System.Linq;
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace VNet.Utility.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool AllExistsIn<T>(this IEnumerable<T> sourceList, IEnumerable<T> secondList)
        {
            return sourceList.All(x => secondList.Any(y => x.Equals(y)));
        }

        public static bool NoneExistsIn<T>(this IEnumerable<T> sourceList, IEnumerable<T> secondList)
        {
            return !sourceList.Any(x => secondList.Any(y => x.Equals(y)));
        }
    }
}
