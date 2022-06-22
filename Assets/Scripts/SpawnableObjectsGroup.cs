using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableObjectsGroup<T>
{
    [SerializeField] public T[] objects;

    public T[] Objects { get => objects; set => objects = value; }


    [SerializeField] public float[] objectSpawnRates = new float[0];

    public float[] ObjectSpawnRates { get => objectSpawnRates; set => objectSpawnRates = value; }

}
