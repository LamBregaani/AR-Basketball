using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#if UNITY_EDITOR
[assembly: InternalsVisibleTo("SpawnableObjectsGroupDrawer")]
#endif
[System.Serializable]


public class SpawnableObjectsGroup<T>
{
    [SerializeField] public T[] objects;

    public T[] Objects { get => objects; set => objects = value; }


    [SerializeField] public float[] objectSpawnRates = new float[0];

    public float[] ObjectSpawnRates { get => objectSpawnRates; set => objectSpawnRates = value; }

    public T GetObject()
    {

        var randomVal = Random.Range(0.0001f, 100f);

        var spawnRate = ObjectSpawnRates[0];

        for (int i = 0; i < ObjectSpawnRates.Length; i++)
        {
            if (spawnRate > randomVal)
            {
                return Objects[i];
                Debug.Log($"Random Value = {randomVal} Spawning: #{i}");
                break;
            }
            else
            {
                spawnRate += ObjectSpawnRates[i];
            }
        }

        Debug.Log($"Random Value = {randomVal}");
        return Objects[0];
    }

}
