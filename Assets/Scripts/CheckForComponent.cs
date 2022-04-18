using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckForComponent
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool CheckComponent<T>(GameObject other)
    {

        var component = other.GetComponent(typeof(T));

        if (component != null)
        {
            return true;
        }
        return false;
    }
}
