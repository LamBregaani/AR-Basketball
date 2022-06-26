using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;


[ExecuteAlways]
[CreateAssetMenu(menuName = "Game Rules/Spawn Hoops", fileName = "New Hoop Properties")]

public class HoopProperties : GameRule
{

    //[HideInInspector] [SerializeField] public List<Hoop> m_uniqueHoops = new List<Hoop>();

    //[HideInInspector] private float[] uniqueHoopSpawnRates;


    //public float[] UniqueHoopSpawnRates { get => uniqueHoopSpawnRates; set => uniqueHoopSpawnRates = value; }

    //public List<Hoop> UniqueHoops { get => m_uniqueHoops; set => m_uniqueHoops = value; }

    public override void Init(GameModeController controller)
    {
        HoopBuilder.Create(hoopsGroup.GetObject(), Vector3.zero);


    }

    [SerializeField] public SpawnableObjectsGroup<Hoop> hoopsGroup;

    [HideInInspector]public const string filePath = "Assets/Resources/Game Properties/Default Hoop Builder Properties.asset";

#if UNITY_EDITOR
    public static HoopProperties GetOrCreateDefaultInstance()
    {
        var properties = AssetDatabase.LoadAssetAtPath<HoopProperties>(filePath);

        if (properties == null)
        {
            properties = CreateInstance<HoopProperties>();

            AssetDatabase.CreateAsset(properties, filePath);

            AssetDatabase.SaveAssets();
        }

        return properties;
    }
# endif
}


[System.Serializable]
public class Hoop
{
    //[SerializeField] public GameObject[] test;


    public GameObject hoopObj;

    public GameObject baseObj;

    public GameObject poseObj;

    public GameObject postBendObj;

    public bool randomizeHeight;

    [DualSlider(0.5f, 5f, 0.5f)]
    public Vector2 randomHeightSlider;

    [Tooltip("Height in meteres. Increments by 0.5m")]
    [RangeEx(0.5f, 5, 0.5f)]
    public float height = 1f;

    public bool canMove;

    public enum MovementType { None, BackAndForth, Circular}

    public MovementType movementType;

    public bool randomizeSpeed;

    [DualSlider(0.1f, 10f, 0.1f)]
    public Vector2 randomSpeedSlider;

    [RangeEx(0.1f, 10f, 0.1f)]
    public float movementSpeed = 1;

    public bool hasBase;
}

