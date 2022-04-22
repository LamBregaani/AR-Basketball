using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object Pool", menuName = "Object Pool")]
public class ObjectPoolProperties : ScriptableObject
{
    [Tooltip("The object to spawn")]
    [SerializeField] private GameObject m_PoolObject;

    public GameObject PoolObject { get { return m_PoolObject; } }

    [Tooltip("Time before objects are returned to the spawn position or 'Despawned'")]
    [SerializeField] private float m_despawnDelay;

    public float DespawnDelay { get { return m_despawnDelay; } }

    [Tooltip("Max number of objects in the pool")]
    [SerializeField] private int m_maxObjects;

    public int MaxObjects { get { return m_maxObjects; } }

    //Acts as a public version of instantiate that can be accessed from other scripts 
    public GameObject SpawnObject(GameObject original, Vector3 position, Quaternion rotation)
    {
        var obj = Instantiate(original, position, rotation);

        return obj;
    }
}
