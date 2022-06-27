using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class HoopBuilder
{
    private static SpawnHoops m_hoopProperties;


    static HoopBuilder()
    {
        m_hoopProperties = SpawnHoops.GetOrCreateDefaultInstance();

    }

    private static Vector3 CreateBase(Hoop _hoop, Vector3 _pos, Transform _parent)
    {
        //Create the hoop base and get its pivots
        var firstPivot = GameObject.Instantiate(_hoop.baseObj, _pos, Quaternion.identity, _parent).GetComponent<ObjectPivotPoints>();

        ObjectPivotPoints secondPivot;

        //Get the amount of posts pieces needed for the desired height
        var postAmount = _hoop.GetHeight() / 0.5f;

        //Create the amount of posts needed
        for (int i = 0; i < postAmount; i++)
        {
            var post = GameObject.Instantiate(_hoop.poseObj, _parent);

            secondPivot = post.GetComponent<ObjectPivotPoints>();

            post.transform.position = firstPivot.GetPivotPosition(Pivot.PivotPosition.Top) + (post.transform.position - secondPivot.GetPivotPosition(Pivot.PivotPosition.Bottom));

            firstPivot = secondPivot; 
        }

        var currentObj = GameObject.Instantiate(_hoop.postBendObj, _parent);

        secondPivot = currentObj.GetComponent<ObjectPivotPoints>();

        currentObj.transform.position = firstPivot.GetPivotPosition(Pivot.PivotPosition.Top) + (currentObj.transform.position - secondPivot.GetPivotPosition(Pivot.PivotPosition.Bottom));

        return secondPivot.GetPivotPosition(Pivot.PivotPosition.Front);

    }

    public static GameObject Create(Hoop _hoop, Vector3 _pos)
    {
        //Create an object to organize the hoop pieces
        var hoopParent = new GameObject("Hoop");

        //Check if the hoop needs a base, if so, create one
        if(_hoop.hasBase)
            _pos = CreateBase(_hoop, _pos, hoopParent.transform);

        //Create the hoop and backboard gameobject
        var currentObj = GameObject.Instantiate(_hoop.hoopObj, hoopParent.transform);

        //Get the pivot on the hoop
        var pivot = currentObj.GetComponent<ObjectPivotPoints>();

        //Move the hoop to the set position with an offset based on the pivot
        currentObj.transform.position = _pos - pivot.GetPivotPosition(Pivot.PivotPosition.Back);

        return hoopParent;
    }
}


