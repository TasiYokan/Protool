using System;
using System.Collections.Generic;

public static class ListExtensionMethods
{
    private static Random rand = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        for(int i = 0; i < list.Count; ++i)
        {
            int k = rand.Next(i);
            T temp = list[k];
            list[k] = list[i];
            list[i] = temp;
        }
    }
}