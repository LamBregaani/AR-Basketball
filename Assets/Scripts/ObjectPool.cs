using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class ObjectPool<T> 
{
    //List of the deasired compoents on the spawned obejcts
    private List<T> m_spawnedObjectComponents;

    //List of the spawned gameobjects
    private List<GameObject> m_spawnedObjects = new List<GameObject>();

    private int m_currentObjIndex;

    //Properties for this object pool
    private ObjectPoolProperties m_poolProperties;

    public ObjectPool(ObjectPoolProperties _props)
    {
        m_poolProperties = _props;

        //Create a new list based on the passed generic type
        m_spawnedObjectComponents = new List<T>();

        //Spawn the objects
        for (int i = 0; i < m_poolProperties.MaxObjects; i++)
        {
            //Essentially instatiates an object into the world
            var obj = m_poolProperties.SpawnObject(m_poolProperties.PoolObject, ObjectPoolGlobalProperties.spawnPosition, Quaternion.identity);

            m_spawnedObjects.Add(obj);

            //Gets the desired component based on the passed generic type
            var objComponent = obj.GetComponent<T>();

            m_spawnedObjectComponents.Add(objComponent);

            //Disables the object until it is needed
            obj.SetActive(false);
        }

    }

    /// <summary>
    /// Reutrns the next instance of a desired component on an object in the pool
    /// </summary>
    /// <returns></returns>
    public T GetObjectComponent()
    {
        var objComp = m_spawnedObjectComponents[m_currentObjIndex];

        var objGO = m_spawnedObjects[m_currentObjIndex];

        m_currentObjIndex++;

        //Allows for looping through the list
        m_currentObjIndex %= m_spawnedObjectComponents.Count;

        ObjectPoolGlobalProperties.objectPoolMono.StartCoroutine(ObjectDespawn(objGO));

        objGO.SetActive(true);

        return objComp;
    }

    /// <summary>
    /// Returns the next instance of a gameobject in the pool
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        var objGO = m_spawnedObjects[m_currentObjIndex];

        m_currentObjIndex++;

        //Allows for looping through the list
        m_currentObjIndex %= m_spawnedObjectComponents.Count;

        ObjectPoolGlobalProperties.objectPoolMono.StartCoroutine(ObjectDespawn(objGO));

        objGO.SetActive(true);

        return objGO;
    }

    /// <summary>
    /// Returns the passed obj to the starting postion after a set delay
    /// </summary>
    /// <param name="_obj"></param>
    /// <returns></returns>
    private IEnumerator ObjectDespawn(GameObject _obj)
    { 

        yield return new WaitForSeconds(m_poolProperties.DespawnDelay);

        _obj.SetActive(false);

        _obj.transform.position = ObjectPoolGlobalProperties.spawnPosition;
    }
}

/// <summary>
/// Global properties for the Object Pools
/// </summary>
public static class ObjectPoolGlobalProperties
{
    //Global spawn position for objects
    public static Vector3 spawnPosition = new Vector3(1000, 1000, 1000);

    //Monobehaviour instance so that the object pools have access to coroutines
    public static ObjectPoolController objectPoolMono;

    static ObjectPoolGlobalProperties()
    {
        //Creates an instance of the ObjectPoolController
        var go = new GameObject("ObjectPoolMonobehaviour");

        objectPoolMono = go.AddComponent<ObjectPoolController>();
    }
}

/// <summary>
/// Used so that the Onnject Pool class has access to StartCoroutine()
/// Will probably be removed later
/// </summary>
public class ObjectPoolController : MonoBehaviour
{

}


