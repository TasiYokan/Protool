using UnityEngine;
using System.Collections;
using System.Linq;

public static class ObjectExtensionMethods
{
	public static T FindObjectOfType<T> (this Object obj) where T:Object
	{
		return Object.FindObjectOfType (typeof(T)) as T;
	}
	
	public static T[] FindObjectsOfType<T> (this Object obj) where T:Object
	{
		return Object.FindObjectsOfType (typeof(T)).Cast<T> ().ToArray ();
	}

    /// <summary>
    /// To select the right destroy method between Editor mode and Game mode
    /// </summary>
    /// <param name="_obj"></param>
    public static void OptimalDestroy(this Object _obj)
    {
        if (Application.isPlaying)
            Object.Destroy(_obj);
        else
            Object.DestroyImmediate(_obj);
    }
}

public static class DebugLogExtensionMethods
{
    public static void Log(this Object obj, string format, params object[] args)
    {
        Debug.Log(string.Format(format, args));
    }
}
