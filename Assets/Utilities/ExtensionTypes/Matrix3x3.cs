using System.Reflection;
using UnityEngine.Scripting;

namespace UnityEngine
{
    public struct Matrix3x3
    {
        public float m00;
        public float m01;
        public float m02;
        public float m10;
        public float m11;
        public float m12;
        public float m20;
        public float m21;
        public float m22;

        public Matrix3x3(float[] elems)
        {
            m00 = elems[0];
            m01 = elems[1];
            m02 = elems[2];
            m10 = elems[3];
            m11 = elems[4];
            m12 = elems[5];
            m20 = elems[6];
            m21 = elems[7];
            m22 = elems[8];
        }

        //public float this[int index] { get; set; }
        //public float this[int row, int column] { get; set; }

        /// <summary>
        /// Returns the identity matrix (Read Only).
        /// </summary>
        public static readonly Matrix3x3 identity
            = new Matrix3x3(new float[] {
                1, 0, 0,
                0, 1, 0,
                0, 0, 1 });

        /// <summary>
        /// Returns a matrix with all elements set to zero (Read Only).
        /// </summary>
        public static readonly Matrix3x3 zero
            = new Matrix3x3(new float[] {
                0, 0, 0,
                0, 0, 0,
                0, 0, 0 });

        /// <summary>
        /// The determinant of the matrix.
        /// </summary>
        //public float determinant { get; }

        /// <summary>
        /// The inverse of this matrix (Read Only).
        /// </summary>
        //public Matrix3x3 inverse { get; }

        /// <summary>
        /// Is this the identity matrix?
        /// </summary>
        //public bool isIdentity { get; }

        /// <summary>
        /// Creates a scaling matrix.
        /// </summary>
        /// /// <param name="v"></param>
        public static Matrix3x3 Scale(Vector2 v)
        {
            return new Matrix3x3(new float[]
            {
                v.x,    0,      0,
                0,      v.y,    0,
                0,      0,      1
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c">angle in degree</param>
        /// <returns></returns>
        public static Matrix3x3 Rotate(float c)
        {
            c *= Mathf.Deg2Rad;
            return new Matrix3x3(new float[]
            {
                Mathf.Cos(c),   -Mathf.Sin(c),  0,
                Mathf.Sin(c),   Mathf.Cos(c),   0,
                0,              0,              1
            });
        }

        public static Matrix3x3 Translate(Vector2 t)
        {
            return new Matrix3x3(new float[]
            {
                1,    0,    t.x,
                0,    1,    t.y,
                0,    0,    1
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix3x3 Transpose(Matrix3x3 m)
        {
            return new Matrix3x3(new float[]
            {
                m.m00, m.m10, m.m20,
                m.m01, m.m11, m.m21,
                m.m02, m.m12, m.m22,
            });
        }

        //public static Matrix3x3 TRS(Vector3 pos, Quaternion q, Vector3 s);
        public override bool Equals(object oth)
        {
            if(ReferenceEquals(null, oth))
                return false;
            if(ReferenceEquals(this, oth))
                return true;
            return oth.GetType() == typeof(Matrix3x3) && Equals((Matrix3x3)oth);
        }

        public bool Equals(Matrix3x3 oth)
        {
            return m00.FloatEquals(oth.m00) && m01.FloatEquals(oth.m01) && m02.FloatEquals(oth.m02)
                && m10.FloatEquals(oth.m10) && m11.FloatEquals(oth.m11) && m12.FloatEquals(oth.m12)
                && m20.FloatEquals(oth.m20) && m21.FloatEquals(oth.m21) && m22.FloatEquals(oth.m22);
        }


        public override int GetHashCode()
        {
            return (int)m00 ^ (int)(m01 + m02) * (int)m10 ^ (int)(m11 + m12);
        }
        //public Vector4 GetColumn(int i);
        //public Vector4 GetRow(int i);

        //
        // 摘要:
        //     ///
        //     Transforms a position by this matrix (generic).
        //     ///
        //
        // 参数:
        //   v:
        private Vector2 MultiplyVector(Vector2 v)
        {
            return new Vector2(
                m00 * v.x + m01 * v.y + m02 * 1,
                m10 * v.x + m11 * v.y + m12 * 1
                );
        }

        private Matrix3x3 MultiplyMatrix3x3(Matrix3x3 m)
        {
            return new Matrix3x3(new float[]
            {
                m00*m.m00+m01*m.m10+m02*m.m20, m00*m.m01+m01*m.m11+m02*m.m21, m00*m.m02+m01*m.m12+m02*m.m22,
                m10*m.m00+m11*m.m10+m12*m.m20, m10*m.m01+m11*m.m11+m12*m.m21, m10*m.m02+m11*m.m12+m12*m.m22,
                m20*m.m00+m21*m.m10+m22*m.m20, m20*m.m01+m21*m.m11+m22*m.m21, m20*m.m02+m21*m.m12+m22*m.m22,
            }
                );
        }

        /// <summary>
        /// Returns a nicely formatted string for this matrix.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("| {0:N}, \t{1:N}, \t{2:N} |\n| {3:N}, \t{4:N}, \t{5:N} |\n| {6:N}, \t{7:N}, \t{8:N} |\n",
                m00, m01, m02,
                m10, m11, m12,
                m20, m21, m22);
        }

        public static Vector2 operator *(Matrix3x3 lhs, Vector2 v)
        {
            return lhs.MultiplyVector(v);
        }
        public static Matrix3x3 operator *(Matrix3x3 lhs, Matrix3x3 rhs)
        {
            return lhs.MultiplyMatrix3x3(rhs);
        }
        public static bool operator ==(Matrix3x3 lhs, Matrix3x3 rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(Matrix3x3 lhs, Matrix3x3 rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}