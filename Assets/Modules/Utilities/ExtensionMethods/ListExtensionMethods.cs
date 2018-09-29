using System;
using System.Collections.Generic;
using System.Linq;

public static class CollectionExtensionMethods
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

    /// <summary>
    /// Try Deep copy. However, it does not guarantee a deep copy, and the .MemberwiseClone() method create a shallow copy of internal members.
    /// <seealso cref="https://msdn.microsoft.com/en-us/library/system.object.memberwiseclone.aspx"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_srcCollection"></param>
    /// <returns></returns>
    public static List<T> Clone<T>(this List<T> _srcCollection) where T : ICloneable
    {
        // Return a new list
        return _srcCollection.Select(item => (T)item.Clone()).ToList();
    }

    public static bool OptimalContainsKey<TKey, TValue>(this Dictionary<TKey, TValue> _dict, TKey _key)
    {
        TValue value;
        return _dict.TryGetValue(_key, out value);
    }

    public static void Resize<T>(this List<T> _list, int _size, T _newFill)
    {
        int cur = _list.Count;
        if (_size < cur)
            _list.RemoveRange(_size, cur - _size);
        else if (_size > cur)
        {
            if (_size > _list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                _list.Capacity = _size;
            _list.AddRange(Enumerable.Repeat(_newFill, _size - cur));
        }
    }
    public static void Resize<T>(this List<T> _list, int _size) where T : new()
    {
        Resize(_list, _size, new T());
    }
}