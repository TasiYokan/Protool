using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class Vector3ExtensionMethods
{
    public static Vector3 AddXYZ(this Vector3 _vec3, float _x = 0, float _y = 0, float _z = 0)
    {
        return new Vector3(_vec3.x + _x, _vec3.y + _y, _vec3.z + _z);
    }

    public static Vector3 AddXY(this Vector3 _vec3, float _x = 0, float _y = 0)
    {
        return new Vector3(_vec3.x + _x, _vec3.y + _y, _vec3.z);
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

    public static Vector3 Reciprocal(this Vector3 _vec)
    {
        return new Vector3(1f / _vec.x, 1f / _vec.y, 1f / _vec.z);
    }

    public static Vector3 Format(this Vector3 _vec3, int _digits)
    {
        return new Vector3((float)System.Math.Round(_vec3.x, _digits),
                           (float)System.Math.Round(_vec3.y, _digits),
                           (float)System.Math.Round(_vec3.z, _digits));
    }
}

public static class Vector2ExtensionMethods
{
    public static Vector2 AddXY(this Vector2 _vec2, int _x = 0, int _y = 0)
    {
        return new Vector2(_vec2.x + _x, _vec2.y + _y);
    }

    public static Vector2 AddX(this Vector2 _vec2, int _dx)
    {
        return new Vector2(_vec2.x + _dx, _vec2.y);
    }

    public static Vector2 AddY(this Vector2 _vec2, int _dy)
    {
        return new Vector2(_vec2.x, _vec2.y + _dy);
    }


    public static Vector2 SetX(this Vector2 _vec2, int _x)
    {
        return new Vector2(_x, _vec2.y);
    }

    public static Vector2 SetY(this Vector2 _vec2, int _y)
    {
        return new Vector2(_vec2.x, _y);
    }

    public static bool Equals(this Vector2 _lhs, Vector2 _rhs)
    {
        return _lhs.x == _rhs.x && _lhs.y == _rhs.y;
    }

    public static bool Greater(this Vector2 _lhs, Vector2 _rhs)
    {
        return _lhs.x > _rhs.x && _lhs.y > _rhs.y;
    }

    public static bool Less(this Vector2 _lhs, Vector2 _rhs)
    {
        return _lhs.x < _rhs.x && _lhs.y < _rhs.y;
    }

    public static bool EqualGreater(this Vector2 _lhs, Vector2 _rhs)
    {
        return _lhs.x >= _rhs.x && _lhs.y >= _rhs.y;
    }

    public static bool EqualLess(this Vector2 _lhs, Vector2 _rhs)
    {
        return _lhs.x <= _rhs.x && _lhs.y <= _rhs.y;
    }

    public static bool AnyGreater(this Vector2 _lhs, Vector2 _rhs)
    {
        return _lhs.x > _rhs.x || _lhs.y > _rhs.y;
    }

    public static bool AnyLess(this Vector2 _lhs, Vector2 _rhs)
    {
        return _lhs.x < _rhs.x || _lhs.y < _rhs.y;
    }

    public static bool AnyEqualGreater(this Vector2 _lhs, Vector2 _rhs)
    {
        return _lhs.x >= _rhs.x || _lhs.y >= _rhs.y;
    }

    public static bool AnyEqualLess(this Vector2 _lhs, Vector2 _rhs)
    {
        return _lhs.x <= _rhs.x || _lhs.y <= _rhs.y;
    }

    public static float Dot(this Vector2 _lhs, Vector2 _rhs)
    {
        return _lhs.x * _rhs.x + _lhs.y * _rhs.y;
    }

    public static float Cross(this Vector2 _lhs, Vector2 _rhs)
    {
        return _lhs.x * _rhs.y - _lhs.y * _rhs.x;
    }

    
    public static Vector2 Scale(this Vector2 _vec2, float _scale)
    {
        return new Vector2(_vec2.x * _scale, _vec2.y * _scale);
    }

    public static Vector2 ScaleX(this Vector2 _vec2, float _factor)
    {
        return new Vector2(_vec2.x * _factor, _vec2.y);
    }

    public static Vector2 ScaleY(this Vector2 _vec2, float _factor)
    {
        return new Vector2(_vec2.x, _vec2.y * _factor);
    }

    public static Vector2 FlipX(this Vector2 _vec2)
    {
        return new Vector2(-_vec2.x, _vec2.y);
    }

    public static Vector2 FlipY(this Vector2 _vec2)
    {
        return new Vector2(_vec2.x, -_vec2.y);
    }
}