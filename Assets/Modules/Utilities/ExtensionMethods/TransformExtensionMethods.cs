using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class TransformExtensionMethods
{
    /// <summary>
    /// Change position only on x
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_x"></param>
    public static void SetPositionX(this Transform _transform, float _x)
    {
        _transform.position = new Vector3(_x, _transform.position.y, _transform.position.z);
    }

    /// <summary>
    /// Change position only on y
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_y"></param>
    public static void SetPositionY(this Transform _transform, float _y)
    {
        _transform.position = new Vector3(_transform.position.x, _y, _transform.position.z);
    }

    /// <summary>
    /// Change position only on z
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_y"></param>
    public static void SetPositionZ(this Transform _transform, float _z)
    {
        _transform.position = new Vector3(_transform.position.x, _transform.position.y, _z);
    }

    /// <summary>
    /// Set position from 3 param x,y,z, Default is zero.[/3/8]
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    public static void SetPosition(this Transform _transform, float _x = 0, float _y = 0, float _z = 0)
    {
        _transform.position = new Vector3(_x, _y, _z);
    }

    public static void SetPosition(this Transform _transform, Vector3 _pos)
    {
        _transform.position = _pos;
    }

    public static void SetPosition(this Transform _transform, Vector2 _pos)
    {
        _transform.position = _pos;
    }

    public static void AddPositionX(this Transform _transform, float _dx)
    {
        _transform.position += new Vector3(_dx, 0, 0);
    }

    public static void AddPositionY(this Transform _transform, float _dy)
    {
        _transform.position += new Vector3(0, _dy, 0);
    }

    public static void AddPositionZ(this Transform _transform, float _dz)
    {
        _transform.position += new Vector3(0, 0, _dz);
    }

    /// <summary>
    /// Set local Scale
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_uniformScale"></param>
    public static void SetUniformLocalScale(this Transform _transform, float _uniformScale)
    {
        _transform.localScale = Vector3.one * _uniformScale;
    }

    public static void SetLocalPosition(this Transform _transform, float _x = 0, float _y = 0, float _z = 0)
    {
        _transform.localPosition = new Vector3(_x, _y, _z);
    }

    /// <summary>
    /// Change localPosition only on x
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_x"></param>
    public static void SetLocalPositionX(this Transform _transform, float _x)
    {
        _transform.localPosition = new Vector3(_x, _transform.localPosition.y, _transform.localPosition.z);
    }

    /// <summary>
    /// Change localPosition only on y
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_y"></param>
    public static void SetLocalPositionY(this Transform _transform, float _y)
    {
        _transform.localPosition = new Vector3(_transform.localPosition.x, _y, _transform.localPosition.z);
    }

    /// <summary>
    /// Change localPosition only on z
    /// </summary>
    /// <param name="_transform"></param>
    /// <param name="_y"></param>
    public static void SetLocalPositionZ(this Transform _transform, float _z)
    {
        _transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y, _z);
    }

    public static void AddLocalPositionX(this Transform _transform, float _dx)
    {
        _transform.localPosition += new Vector3(_dx, 0, 0);
    }

    public static void AddLocalPositionY(this Transform _transform, float _dy)
    {
        _transform.localPosition += new Vector3(0, _dy, 0);
    }

    public static void AddLocalPositionZ(this Transform _transform, float _dz)
    {
        _transform.localPosition += new Vector3(0, 0, _dz);
    }
}

public static class UITransformExtensionMethods
{
    public static void SetAnchoredPosition(this RectTransform _rectTransform, float _x, float _y)
    {
        _rectTransform.anchoredPosition = new Vector2(_x, _y);
    }

    public static void SetAnchoredPositionX(this RectTransform _rectTransform, float _x)
    {
        _rectTransform.anchoredPosition = new Vector2(_x, _rectTransform.anchoredPosition.y);
    }

    public static void SetAnchoredPositionY(this RectTransform _rectTransform, float _y)
    {
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _y);
    }
}
