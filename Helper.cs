using System;
using System.Collections.Generic;

namespace UncommonSense.Bc.Utils
{
    public static class Helper
    {
        public static IEnumerable<ObjectType> AllObjectTypes() =>
            (ObjectType[])(Enum.GetValues(typeof(ObjectType)));

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }

            return items;
        }
    }
}