using System;
using UnityEngine;

public enum EDirection
{
    Any,
    Front,
    Right,
    Back,
    Left,
    Up,
    Down
}

/// <summary>
/// using the right-handed coordinate system which is different from that in unity.[12/27]
/// </summary>
[Serializable]
public struct Vector3_int : System.IEquatable<Vector3_int>
{
    #region CONST
    /// <summary>
    /// (0, 0, 0)
    /// </summary>
    public static readonly Vector3_int zero = new Vector3_int(0, 0, 0);
    /// <summary>
    /// (0, 1, 0)
    /// </summary>
    public static readonly Vector3_int front = new Vector3_int(0, 1, 0);
    /// <summary>
    /// (1, 0, 0)
    /// </summary>
    public static readonly Vector3_int right = new Vector3_int(1, 0, 0);
    /// <summary>
    /// (0, -1, 0)
    /// </summary>
    public static readonly Vector3_int back = new Vector3_int(0, -1, 0);
    /// <summary>
    /// (-1, 0, 0)
    /// </summary>
    public static readonly Vector3_int left = new Vector3_int(-1, 0, 0);
    /// <summary>
    /// (0, 0, 1)
    /// </summary>
    public static readonly Vector3_int up = new Vector3_int(0, 0, 1);
    /// <summary>
    /// (0, 0, -1)
    /// </summary>
    public static readonly Vector3_int down = new Vector3_int(0, 0, -1);
    #endregion const

    public int x;
    public int y;
    public int z;


    public Vector3_int(int _x, int _y, int _z = 0)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public Vector3_int(Vector3_int _previousVector3)
    {
        x = _previousVector3.x;
        y = _previousVector3.y;
        z = _previousVector3.z;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1}, {2})", x, y, z);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y);
    }

    /// <summary>
    /// Vector3 -> Vector3_int
    /// </summary>
    /// <param name="_vec3"></param>
    public static explicit operator Vector3_int(Vector3 _vec3)
    {
        return new Vector3_int((int)_vec3.x, (int)_vec3.y, (int)_vec3.z);
    }

    /// <summary>
    /// Vector2_int -> Vector3_int
    /// </summary>
    /// <param name="_vec2"></param>
    public static implicit operator Vector3_int(Vector2_int _vec2)
    {
        return new Vector3_int(_vec2.x, _vec2.y, 0);
    }

    public override bool Equals(object obj)
    {
        if(obj is Vector3_int)
        {
            return this.Equals((Vector3_int)obj);
        }
        else if(obj is Vector2_int)
        {
            return this.Equals((Vector2_int)obj);
        }
        return false;
    }

    public bool Equals(Vector3_int _rhs)
    {
        return (x == _rhs.x
                && y == _rhs.y
                && z == _rhs.z);
    }

    public override int GetHashCode()
    {
        return x ^ y ^ z;
    }

    public static Vector3_int operator -(Vector3_int _lhs)
    {
        return new Vector3_int(-_lhs.x, -_lhs.y, -_lhs.z);
    }

    public static Vector3_int operator +(Vector3_int _lhs, Vector3_int _rhs)
    {
        return new Vector3_int(_lhs.x + _rhs.x, _lhs.y + _rhs.y, _lhs.z + _rhs.z);
    }

    public static Vector3_int operator -(Vector3_int _lhs, Vector3_int _rhs)
    {
        return new Vector3_int(_lhs.x - _rhs.x, _lhs.y - _rhs.y, _lhs.z - _rhs.z);
    }
    //public bool Equals(Vector2_int _rhs)
    //{
    //    return (x == _rhs.x
    //         && y == _rhs.y);
    //}
    /// <summary>
    /// override to compare the value.[11/26]
    /// </summary>
    /// <param name="a"></param>
    /// <param name="_rhs"></param>
    /// <returns></returns>
    public static bool operator ==(Vector3_int _lhs, Vector3_int _rhs)
    {
        return _lhs.Equals(_rhs);
    }

    /// <summary>
    /// override to compare the value.[11/26]
    /// </summary>
    /// <param name="a"></param>
    /// <param name="_rhs"></param>
    /// <returns></returns>
    public static bool operator !=(Vector3_int _lhs, Vector3_int _rhs)
    {
        return !_lhs.Equals(_rhs);
    }

    public Vector3_int Scale(float _scale)
    {
        return new Vector3_int((int)(x * _scale), (int)(y * _scale), (int)(z * _scale));
    }

    //public bool EqualWithXY(int _x, int _y)
    //{
    //    if (x == _x && y == _y)
    //        return true;
    //    else
    //        return false;
    //}

    /// <summary>
    /// returns 7 direcitons in 3D space. use it carefully.[12/28]
    /// </summary>
    /// <returns></returns>
    public Vector3_int GetNormalized3D()
    {
        //vertical
        if(x == 0 && y == 0)
        {
            if(z != 0)
                return new Vector3_int(0, 0, z / Mathf.Abs(z));
            else
                return new Vector3_int(0, 0, 0);
        }
        else if(x == 0)
        {
            int ay = Mathf.Abs(y);
            int az = Mathf.Abs(z);

            if(ay > az)
                return new Vector3_int(0, y / ay, 0);
            else
                return new Vector3_int(0, 0, z / az);
        }
        else if(y == 0)
        {
            int ax = Mathf.Abs(x);
            int az = Mathf.Abs(z);

            if(ax > az)
                return new Vector3_int(x / ax, 0, 0);
            else
                return new Vector3_int(0, 0, z / az);
        }
        else
        {
            int ax = Mathf.Abs(x);
            int ay = Mathf.Abs(y);
            int az = Mathf.Abs(z);

            if(ax > ay && ax > az)
                return new Vector3_int(x / ax, 0, 0);
            else if(ay > ax && ay > az)
                return new Vector3_int(0, y / ay, 0);
            else if(az > ax && az > ay)
                return new Vector3_int(0, 0, z / az);
            else
                return new Vector3_int(0, 0, 0);
        }
    }

    public EDirection GetDirection3D()
    {
        Vector3_int normedDir = this.GetNormalized3D();

        if(normedDir == front)
        {
            return EDirection.Front;
        }
        else if(normedDir == right)
        {
            return EDirection.Right;
        }
        else if(normedDir == back)
        {
            return EDirection.Back;
        }
        else if(normedDir == left)
        {
            return EDirection.Left;
        }
        else if(normedDir == up)
        {
            return EDirection.Up;
        }
        else if(normedDir == down)
        {
            return EDirection.Down;
        }
        else//if (normedDir==zero)
        {
            return EDirection.Any;
        }
    }
}
