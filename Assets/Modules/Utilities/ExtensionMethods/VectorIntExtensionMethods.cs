using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum EDirection
{
    Any,
    Forward,
    Right,
    Back,
    Left,
    Up,
    Down
}

public static class Vector3IntExtensionMethods
{
    public static Vector3Int AddXYZ(this Vector3Int _vec3, int _x = 0, int _y = 0, int _z = 0)
    {
        return new Vector3Int(_vec3.x + _x, _vec3.y + _y, _vec3.z + _z);
    }

    public static Vector3Int AddXY(this Vector3Int _vec3, int _x = 0, int _y = 0)
    {
        return new Vector3Int(_vec3.x + _x, _vec3.y + _y, _vec3.z);
    }

    public static Vector3Int AddX(this Vector3Int _vec3, int _dx)
    {
        return new Vector3Int(_vec3.x + _dx, _vec3.y, _vec3.z);
    }

    public static Vector3Int AddY(this Vector3Int _vec3, int _dy)
    {
        return new Vector3Int(_vec3.x, _vec3.y + _dy, _vec3.z);
    }

    public static Vector3Int AddZ(this Vector3Int _vec3, int _dz)
    {
        return new Vector3Int(_vec3.x, _vec3.y, _vec3.z + _dz);
    }


    public static Vector3Int SetX(this Vector3Int _vec3, int _x)
    {
        return new Vector3Int(_x, _vec3.y, _vec3.z);
    }

    public static Vector3Int SetY(this Vector3Int _vec3, int _y)
    {
        return new Vector3Int(_vec3.x, _y, _vec3.z);
    }

    public static Vector3Int SetZ(this Vector3Int _vec3, int _z)
    {
        return new Vector3Int(_vec3.x, _vec3.y,  _z);
    }

    public static Vector3Int Scale(this Vector3Int _vec3, float _scale)
    {
        return new Vector3Int((int)(_vec3.x * _scale), (int)(_vec3.y * _scale), (int)(_vec3.z * _scale));
    }

    
    /// <summary>
    /// returns 7 direcitons in 3D space. use it carefully.[12/28]
    /// </summary>
    /// <returns></returns>
    public static Vector3Int GetNormalized3D(this Vector3Int _vec3)
    {
        //vertical
        if(_vec3.x == 0 && _vec3.y == 0)
        {
            if(_vec3.z != 0)
                return new Vector3Int(0, 0, _vec3.z / Mathf.Abs(_vec3.z));
            else
                return new Vector3Int(0, 0, 0);
        }
        else if(_vec3.x == 0)
        {
            int ay = Mathf.Abs(_vec3.y);
            int az = Mathf.Abs(_vec3.z);

            if(ay > az)
                return new Vector3Int(0, _vec3.y / ay, 0);
            else
                return new Vector3Int(0, 0, _vec3.z / az);
        }
        else if(_vec3.y == 0)
        {
            int ax = Mathf.Abs(_vec3.x);
            int az = Mathf.Abs(_vec3.z);

            if(ax > az)
                return new Vector3Int(_vec3.x / ax, 0, 0);
            else
                return new Vector3Int(0, 0, _vec3.z / az);
        }
        else
        {
            int ax = Mathf.Abs(_vec3.x);
            int ay = Mathf.Abs(_vec3.y);
            int az = Mathf.Abs(_vec3.z);

            if(ax > ay && ax > az)
                return new Vector3Int(_vec3.x / ax, 0, 0);
            else if(ay > ax && ay > az)
                return new Vector3Int(0, _vec3.y / ay, 0);
            else if(az > ax && az > ay)
                return new Vector3Int(0, 0, _vec3.z / az);
            else
                return new Vector3Int(0, 0, 0);
        }
    }

    
    public static EDirection GetDirection3D(this Vector3Int _vec3)
    {
        Vector3Int normedDir = _vec3.GetNormalized3D();

        if(normedDir == Vector3Int.forward)
        {
            return EDirection.Forward;
        }
        else if(normedDir == Vector3Int.right)
        {
            return EDirection.Right;
        }
        else if(normedDir == Vector3Int.back)
        {
            return EDirection.Back;
        }
        else if(normedDir == Vector3Int.left)
        {
            return EDirection.Left;
        }
        else if(normedDir == Vector3Int.up)
        {
            return EDirection.Up;
        }
        else if(normedDir == Vector3Int.down)
        {
            return EDirection.Down;
        }
        else//if (normedDir==zero)
        {
            return EDirection.Any;
        }
    }
}


public static class Vector2IntExtensionMethods
{
    public static Vector2Int AddXY(this Vector2Int _vec2, int _x = 0, int _y = 0)
    {
        return new Vector2Int(_vec2.x + _x, _vec2.y + _y);
    }

    public static Vector2Int AddX(this Vector2Int _vec2, int _dx)
    {
        return new Vector2Int(_vec2.x + _dx, _vec2.y);
    }

    public static Vector2Int AddY(this Vector2Int _vec2, int _dy)
    {
        return new Vector2Int(_vec2.x, _vec2.y + _dy);
    }


    public static Vector2Int SetX(this Vector2Int _vec2, int _x)
    {
        return new Vector2Int(_x, _vec2.y);
    }

    public static Vector2Int SetY(this Vector2Int _vec2, int _y)
    {
        return new Vector2Int(_vec2.x, _y);
    }

    public static bool Equals(this Vector2Int _lhs, Vector2Int _rhs)
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

    
    public static Vector2Int Scale(this Vector2Int _vec2, float _scale)
    {
        return new Vector2Int((int)(_vec2.x * _scale), (int)(_vec2.y * _scale));
    }

    public static Vector2Int Scale(this Vector2Int _vec2, int _scale_int)
    {
        return new Vector2Int(_vec2.x * _scale_int, _vec2.y * _scale_int);
    }

    public static Vector2Int ScaleX(this Vector2Int _vec2, int _factor)
    {
        return new Vector2Int(_vec2.x * _factor, _vec2.y);
    }

    public static Vector2Int ScaleY(this Vector2Int _vec2, int _factor)
    {
        return new Vector2Int(_vec2.x, _vec2.y * _factor);
    }

    public static Vector2Int FlipX(this Vector2Int _vec2)
    {
        return new Vector2Int(-_vec2.x, _vec2.y);
    }

    public static Vector2Int FlipY(this Vector2Int _vec2)
    {
        return new Vector2Int(_vec2.x, -_vec2.y);
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

    
    /// <summary>
    /// return the nearest orientation from its direction vecotr.[/2/1]
    /// </summary>
    /// <returns></returns>
    public static EDirection GetDirection2D(this Vector2Int _vec2)
    {
        Vector2Int normedDir = GetNormalized2D(_vec2);

        if (normedDir == Vector2Int.up)
        {
            return EDirection.Up;
        }
        else if (normedDir == Vector2Int.right)
        {
            return EDirection.Right;
        }
        else if (normedDir == Vector2Int.down)
        {
            return EDirection.Down;
        }
        else if (normedDir == Vector2Int.left)
        {
            return EDirection.Left;
        }
        else
        {
            //return EDirection.Any;
            if (Mathf.Abs(normedDir.x) > Mathf.Abs(normedDir.y))
            {
                if (normedDir.x < 0)
                {
                    return EDirection.Left;
                }
                else
                {
                    return EDirection.Right;
                }
            }
            else
            {
                if (normedDir.y < 0)
                {
                    return EDirection.Back;
                }
                else
                {
                    return EDirection.Forward;
                }
            }
        }
    }

    /// <summary>
    /// Get the unit vector in the direction
    /// </summary>
    /// <param name="_dir"></param>
    /// <returns></returns>
    public static Vector2Int GetVectorFromDirection(EDirection _dir)
    {
        switch (_dir)
        {
            case EDirection.Any:
                return Vector2Int.zero;
            case EDirection.Up:
                return Vector2Int.up;
            case EDirection.Down:
                return Vector2Int.down;
            case EDirection.Left:
                return Vector2Int.left;
            case EDirection.Right:
                return Vector2Int.right;
            default:
                return Vector2Int.zero;
        }
    }
}