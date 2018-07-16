using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class VectorExtensionMethods
{
    public static Vector3 AddXYZ(this Vector3 _vec3, float _x = 0, float _y = 0, float _z = 0)
    {
        return new Vector3(_vec3.x + _x, _vec3.y + _y, _vec3.z + _z);
    }

    public static Vector3 AddX(this Vector3 _vec3, float _dx)
    {
        return new Vector3(_vec3.x + _dx, _vec3.y, _vec3.z);
    }

    public static Vector3 AddY(this Vector3 _vec3, float _dy)
    {
        return new Vector3(_vec3.x, _vec3.y + _dy, _vec3.z);
    }

    public static Vector3 AddZ(this Vector3 _vec3, float _dz)
    {
        return new Vector3(_vec3.x, _vec3.y, _vec3.z + _dz);
    }

    public static Vector2 AddXY(this Vector2 _vec2, float _x = 0, float _y = 0)
    {
        return new Vector2(_vec2.x + _x, _vec2.y + _y);
    }



    public static Vector3 SetX(this Vector3 _vec3, float _x)
    {
        return new Vector3(_x, _vec3.y, _vec3.z);
    }

    public static Vector3 SetY(this Vector3 _vec3, float _y)
    {
        return new Vector3(_vec3.x, _y, _vec3.z);
    }

    public static Vector3 SetZ(this Vector3 _vec3, float _z)
    {
        return new Vector3(_vec3.x, _vec3.y,  _z);
    }

    public static Vector3 Format(this Vector3 _vec3, int _digits)
    {
        return new Vector3((float)System.Math.Round(_vec3.x, _digits),
                           (float)System.Math.Round(_vec3.y, _digits),
                           (float)System.Math.Round(_vec3.z, _digits));
    }
}

public static class Vector2IntExtensionMethods
{
    public static bool Equal(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return _lhs.x == _rhs.x && _lhs.y == _rhs.y;
    }

    public static bool Greater(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return _lhs.x > _rhs.x && _lhs.y > _rhs.y;
    }

    public static bool Less(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return _lhs.x < _rhs.x && _lhs.y < _rhs.y;
    }

    public static bool EqualGreater(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return _lhs.x >= _rhs.x && _lhs.y >= _rhs.y;
    }

    public static bool EqualLess(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return _lhs.x <= _rhs.x && _lhs.y <= _rhs.y;
    }

    public static bool AnyGreater(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return _lhs.x > _rhs.x || _lhs.y > _rhs.y;
    }

    public static bool AnyLess(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return _lhs.x < _rhs.x || _lhs.y < _rhs.y;
    }

    public static bool AnyEqualGreater(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return _lhs.x >= _rhs.x || _lhs.y >= _rhs.y;
    }

    public static bool AnyEqualLess(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return _lhs.x <= _rhs.x || _lhs.y <= _rhs.y;
    }

    public static int Dot(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return _lhs.x * _rhs.x + _lhs.y * _rhs.y;
    }

    public static int Cross(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return _lhs.x * _rhs.y - _lhs.y * _rhs.x;
    }

    /// <summary>
    /// Returns 8 directinos in plane.
    /// </summary>
    /// <returns></returns>
    public static Vector2Int GetNormalized2D(this Vector2Int _vec)
    {
        if (_vec.x == 0 && _vec.y == 0)
        {
            return new Vector2Int(0, 0);
        }
        //vertical
        else if (_vec.x == 0)
        {
            return new Vector2Int(0, _vec.y / Mathf.Abs(_vec.y));
        }
        //horizontal
        else if (_vec.y == 0)
        {
            return new Vector2Int(_vec.x / Mathf.Abs(_vec.x), 0);
        }
        //diagonal
        else
        {
            return new Vector2Int(_vec.x / Mathf.Abs(_vec.x), _vec.y / Mathf.Abs(_vec.y));
        }
    }

    /// <summary>
    /// Returns 4 directions
    /// </summary>
    /// <param name="_vec"></param>
    /// <returns></returns>
    public static Vector2Int GetNormalizedSimple2D(this Vector2Int _vec)
    {
        if (_vec.x == 0 && _vec.y == 0)
        {
            return new Vector2Int(0, 0);
        }
        //vertical
        else if (_vec.x == 0)
        {
            return new Vector2Int(0, _vec.y / Mathf.Abs(_vec.y));
        }
        //horizontal
        else if (_vec.y == 0)
        {
            return new Vector2Int(_vec.x / Mathf.Abs(_vec.x), 0);
        }
        else
        {
            return Mathf.Abs(_vec.x) >= Mathf.Abs(_vec.y) ?
                new Vector2Int(_vec.x / Mathf.Abs(_vec.x), 0)
                : new Vector2Int(0, _vec.y / Mathf.Abs(_vec.y));
        }
    }

    public static int ManhattanDist(this Vector2Int _lhs, Vector2Int _rhs)
    {
        return Mathf.Abs(_lhs.x - _rhs.x) + Mathf.Abs(_lhs.y - _rhs.y);
    }

    /// <summary>
    /// Vector2 -> Vector2Int
    /// It's a round convertion
    /// </summary>
    /// <param name="_vec2"></param>
    public static Vector2Int ToVector2Int(this Vector2 _vec2)
    {
        return new Vector2Int(Mathf.RoundToInt(_vec2.x), Mathf.RoundToInt(_vec2.y));
    }

    public static Vector2Int GetNormalized2DInt(this Vector2 _vec)
    {
        if (_vec.x == 0 && _vec.y == 0)
        {
            return new Vector2Int(0, 0);
        }
        //vertical
        else if (_vec.x == 0)
        {
            return new Vector2Int(0, Mathf.RoundToInt(_vec.y / Mathf.Abs(_vec.y)));
        }
        //horizontal
        else if (_vec.y == 0)
        {
            return new Vector2Int(Mathf.RoundToInt(_vec.x / Mathf.Abs(_vec.x)), 0);
        }
        //diagonal
        else
        {
            return new Vector2Int(Mathf.RoundToInt(_vec.x / Mathf.Abs(_vec.x)), Mathf.RoundToInt(_vec.y / Mathf.Abs(_vec.y)));
        }
    }
}