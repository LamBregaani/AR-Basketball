using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "New Object Pool", menuName = "Object Pool")]
public class ObjectPool : ScriptableObject
{
    [Tooltip("The object to spawn")]
    [SerializeField] private GameObject m_Object;

    [Tooltip("Time before objects are returned to the spawn position or 'Despawned'")]
    [SerializeField] private float m_despawnDelay;

    //Max number of objects in the pool
    [SerializeField] private int m_maxObjects;

    //List of spawned objects in the pool
    //NonSerialized field prevents these values from being saved between sessions when in editor mode
    [System.NonSerialized] public List<GameObject> m_spawnedObjects = new List<GameObject>();

    [System.NonSerialized] private int m_currentObjIndex;

    public void Init()
    {
        //Spawn the objects
        for (int i = 0; i < m_maxObjects; i++)
        {
            var ball = Instantiate(m_Object, ObjectPoolProperties.spawnPosition, Quaternion.identity);

            m_spawnedObjects.Add(ball);
        }

    }

    /// <summary>
    /// Reutrns the next instance of the object in the pool
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        var obj = m_spawnedObjects[m_currentObjIndex];

        m_currentObjIndex++;

        //Allows for looping through the list
        m_currentObjIndex %= m_spawnedObjects.Count;

        ObjectPoolProperties.objectPoolMono.StartCoroutine(ObjectDespawn(obj));

        return obj;
    }

    /// <summary>
    /// Returns the passed obj to the starting postion after a set delay
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private IEnumerator ObjectDespawn(GameObject obj)
    {
        yield return new WaitForSeconds(m_despawnDelay);

        obj.transform.position = ObjectPoolProperties.spawnPosition;
    }
}

/// <summary>
/// Global properties for the Object Pools
/// </summary>
public static class ObjectPoolProperties
{
    //Global spawn position for objects
    public static Vector3 spawnPosition = new Vector3(1000, 1000, 1000);

    //Monobehaviour instance so that the object pools have access to coroutines
    public static MonoBehaviour objectPoolMono;

    static ObjectPoolProperties()
    {
        var go = new GameObject("ObjectPoolMonobehaviour");

        objectPoolMono = go.AddComponent<MonoBehaviour>();
    }
}
