using System;
using UnityEngine;

[Serializable]
public struct Vector2_int : System.IEquatable<Vector2_int>
{
    #region CONST
    /// <summary>
    /// (0, 0)
    /// </summary>
    public static readonly Vector2_int zero = new Vector2_int(0, 0);
    /// <summary>
    /// (0, 1)
    /// </summary>
    public static readonly Vector2_int up = new Vector2_int(0, 1);
    /// <summary>
    /// (1, 0)
    /// </summary>
    public static readonly Vector2_int right = new Vector2_int(1, 0);
    /// <summary>
    /// (0, -1)
    /// </summary>
    public static Vector2_int down = new Vector2_int(0, -1);
    /// <summary>
    /// (-1, 0)
    /// </summary>
    public static readonly Vector2_int left = new Vector2_int(-1, 0);

    /// <summary>
    /// (1, 1)
    /// </summary>
    public static readonly Vector2_int upRight = new Vector2_int(1, 1);
    /// <summary>
    /// (-1, 1)
    /// </summary>
    public static readonly Vector2_int upLeft = new Vector2_int(-1, 1);
    /// <summary>
    /// (1, -1)
    /// </summary>
    public static readonly Vector2_int downRight = new Vector2_int(1, -1);
    /// <summary>
    /// (-1, -1)
    /// </summary>
    public static readonly Vector2_int downLeft = new Vector2_int(-1, -1);
    #endregion CONST

    public int x;
    public int y;

    public Vector2_int(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", x, y);
    }

    public Vector2_int(Vector2_int _previousVector2)
    {
        x = _previousVector2.x;
        y = _previousVector2.y;
    }

    /// <summary>
    /// Vector2_int -> Vector2
    /// </summary>
    /// <returns></returns>
    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }

    /// <summary>
    /// Vector2 -> Vector2_int
    /// </summary>
    /// <param name="_vec2"></param>
    public static explicit operator Vector2_int(Vector2 _vec2)
    {
        return new Vector2_int((int)_vec2.x, (int)_vec2.y);
    }

    /// <summary>
    /// Vector3_int -> Vector2_int
    /// </summary>
    /// <param name="_vec3"></param>
    public static explicit operator Vector2_int(Vector3_int _vec3)
    {
        return new Vector2_int(_vec3.x, _vec3.y);
    }

    /// <summary>
    /// Vector3 -> Vector2_int
    /// </summary>
    /// <param name="_vec3"></param>
    public static explicit operator Vector2_int(Vector3 _vec3)
    {
        return new Vector2_int((int)_vec3.x, (int)_vec3.y);
    }

    public bool Equals(Vector2_int _rhs)
    {
        if (ReferenceEquals(null, _rhs))
            return false;
        return (x == _rhs.x && y == _rhs.y);
    }

    public bool NotEquals(Vector2_int _rhs)
    {
        if (ReferenceEquals(null, _rhs))
            return false;
        return !(x == _rhs.x && y == _rhs.y);
    }

    public bool EqualsValue(int _x, int _y)
    {
        return (x == _x && y == _y);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == typeof(Vector2_int) && Equals((Vector2_int)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (x * 397) ^ y;
        }
    }

    /// <summary>
    /// override to compare the value.[11/26]
    /// </summary>
    /// <param name="a"></param>
    /// <param name="_rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Vector2_int _lhs, Vector2_int _rhs)
    {
        return _lhs.Equals(_rhs);
    }

    /// <summary>
    /// override to compare the value.[11/26]
    /// </summary>
    /// <param name="a"></param>
    /// <param name="_rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Vector2_int _lhs, Vector2_int _rhs)
    {
        return !_lhs.Equals(_rhs);
    }

    public static Vector2_int operator -(Vector2_int _lhs)
    {
        return new Vector2_int(-_lhs.x, -_lhs.y);
    }

    public static Vector2_int operator +(Vector2_int _lhs, Vector2_int _rhs)
    {
        return new Vector2_int(_lhs.x + _rhs.x, _lhs.y + _rhs.y);
    }

    public static Vector2_int operator -(Vector2_int _lhs, Vector2_int _rhs)
    {
        return new Vector2_int(_lhs.x - _rhs.x, _lhs.y - _rhs.y);
    }

    public Vector2_int Add(int _x, int _y)
    {
        return new Vector2_int(x + _x, y + _y);
    }

    public Vector2_int Scale(float _scale)
    {
        return new Vector2_int((int)(x * _scale), (int)(y * _scale));
    }

    public Vector2_int Scale(int _scale_int)
    {
        return new Vector2_int(x * _scale_int, y * _scale_int);
    }

    public Vector2_int ScaleX(int _factor)
    {
        return new Vector2_int(x * _factor, y);
    }

    public Vector2_int ScaleY(int _factor)
    {
        return new Vector2_int(x, y * _factor);
    }

    public Vector2_int FlipX()
    {
        return new Vector2_int(-x, y);
    }

    public Vector2_int FlipY()
    {
        return new Vector2_int(x, -y);
    }

    /// <summary>
    /// Return a new Vector2_int
    /// </summary>
    /// <param name="_newX"></param>
    /// <returns></returns>
    public Vector2_int SetX(int _newX)
    {
        return new Vector2_int(_newX, y);
    }

    /// <summary>
    /// Return a new Vector2_int
    /// </summary>
    /// <param name="_newY"></param>
    /// <returns></returns>
    public Vector2_int SetY(int _newY)
    {
        return new Vector2_int(x, _newY);
    }

    public int Dot(Vector2_int _other)
    {
        return x * _other.x + y * _other.y;
    }

    public int CrossNorm(Vector2_int _other)
    {
        return x * _other.y - y * _other.x;
    }

    public static int DotProduct(Vector2_int _lhs, Vector2_int _rhs)
    {
        return _lhs.x * _rhs.x + _lhs.y * _rhs.y;
    }

    public static int ManhattanDistance(Vector2_int _lhs, Vector2_int _rhs)
    {
        Vector2_int dist = _lhs - _rhs;
        return Mathf.Abs(dist.x) + Mathf.Abs(dist.y);
    }

    public static int LongerLegDistance(Vector2_int _lhs, Vector2_int _rhs)
    {
        Vector2_int dist = _lhs - _rhs;
        return Mathf.Abs(dist.x) > Mathf.Abs(dist.y)
            ? Mathf.Abs(dist.x) : Mathf.Abs(dist.y);
    }

    /// <summary>
    /// The same as -this
    /// </summary>
    /// <returns></returns>
    public Vector2_int Inverse()
    {
        return -this;
    }

    /// <summary>
    /// returns 8 directinos in plane.[12/28]
    /// </summary>
    /// <returns></returns>
    public Vector2_int GetNormalized2D()
    {
        if (x == 0 && y == 0)
        {
            return new Vector2_int(0, 0);
        }
        //vertical
        else if (x == 0)
        {
            return new Vector2_int(0, y / Mathf.Abs(y));
        }
        //horizontal
        else if (y == 0)
        {
            return new Vector2_int(x / Mathf.Abs(x), 0);
        }
        //diagonal
        else
        {
            return new Vector2_int(x / Mathf.Abs(x), y / Mathf.Abs(y));
        }
    }

    public Vector2_int GetNormalizedSimple2D()
    {
        if (x == 0 && y == 0)
        {
            return new Vector2_int(0, 0);
        }
        //vertical
        else if (x == 0)
        {
            return new Vector2_int(0, y / Mathf.Abs(y));
        }
        //horizontal
        else if (y == 0)
        {
            return new Vector2_int(x / Mathf.Abs(x), 0);
        }
        else
        {
            return Mathf.Abs(x) >= Mathf.Abs(y) ?
                new Vector2_int(x / Mathf.Abs(x), 0)
                : new Vector2_int(0, y / Mathf.Abs(y));
        }
    }

    /// <summary>
    /// return the nearest orientation from its direction vecotr.[/2/1]
    /// </summary>
    /// <returns></returns>
    public EDirection GetDirection2D()
    {
        Vector2_int normedDir = this.GetNormalized2D();

        if (normedDir == up)
        {
            return EDirection.Up;
        }
        else if (normedDir == right)
        {
            return EDirection.Right;
        }
        else if (normedDir == down)
        {
            return EDirection.Down;
        }
        else if (normedDir == left)
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
                    return EDirection.Front;
                }
            }
        }
    }

    /// <summary>
    /// Get the unit vector in the direction
    /// </summary>
    /// <param name="_dir"></param>
    /// <returns></returns>
    public static Vector2_int GetVectorFromDirection(EDirection _dir)
    {
        switch (_dir)
        {
            case EDirection.Any:
                return zero;
            case EDirection.Up:
                return up;
            case EDirection.Down:
                return down;
            case EDirection.Left:
                return left;
            case EDirection.Right:
                return right;
            default:
                return zero;
        }
    }

}
