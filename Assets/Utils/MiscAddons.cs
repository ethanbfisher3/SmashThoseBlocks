using System.Collections.Generic;
using System;

namespace Utils
{
    public static class MiscAddons
    {
        public static void ForEach<T>(this T[] array, Action<T> Action)
        {
            foreach (T item in array)
                Action(item);
        }
    }
}