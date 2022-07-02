using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectSpawner
{
    [SerializeField] [HideInInspector] public int maxObjects = 1;

    [SerializeField] [HideInInspector] public bool cycleObjects = false;

    [SerializeField] [HideInInspector] public float lifeTime;

    [SerializeField] [HideInInspector] public float respawnTime;
}
