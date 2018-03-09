using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class MiscExtensionMethods
{
    public static bool Is<T>(this object _obj)
    {
        return (_obj is T);
    }

    public static bool IsNot<T>(this object _obj)
    {
        return !(_obj is T);
    }

    public static List<T> GetRangeFrom<T>(this List<T> _list, int _firstIndex)
    {
        return _list.GetRange(_firstIndex, _list.Count - _firstIndex);
    }

    public static List<T> GetRangeTill<T>(this List<T> _list, int _lastIndex)
    {
        return _list.GetRange(0, _list.Count - _lastIndex - 1);
    }

    public static void RemoveRangeFrom<T>(this List<T> _list, int _firstIndex)
    {
        _list.RemoveRange(_firstIndex, _list.Count - _firstIndex);
    }
}