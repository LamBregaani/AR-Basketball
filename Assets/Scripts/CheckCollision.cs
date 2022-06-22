using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    /// <summary>
    /// Checks the reletive direction of a collision and compares it to the passed direction
    /// </summary>
    /// <param name="_hitObjPos"></param>
    /// <param name="normalObj"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static bool CheckDirection(Vector3 _hitObjPos, Vector3 normalObj, Vector3 direction)
    {
        //Create a ray from the obj that hit in the direction of the hit object
        Ray ray = new Ray(_hitObjPos, normalObj - _hitObjPos);

        RaycastHit hit;

        //Fire a raycast to get the normal of the collider that was hit
        Physics.Raycast(ray, out hit);

        //Return true if the normal is the same as the passed direction
        if(hit.normal == direction)
            return true;


        return false;
    }
}
