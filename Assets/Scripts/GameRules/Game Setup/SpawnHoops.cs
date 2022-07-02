using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;


[ExecuteAlways]
[CreateAssetMenu(menuName = "Game Rules/Spawn Hoops", fileName = "New Hoop Properties")]

public class SpawnHoops : GameRule
{
    public override void Init(GameModeController controller)
    {
        HoopBuilder.Create(hoopsGroup.GetObject(), Vector3.zero);
    }

    [SerializeField] public SpawnableObjectsGroup<Hoop> hoopsGroup;

    [SerializeField] private ObjectSpawner spawner;

    [HideInInspector]public const string filePath = "Assets/Resources/Game Properties/Default Hoop Builder Properties.asset";

#if UNITY_EDITOR
    public static SpawnHoops GetOrCreateDefaultInstance()
    {
        var properties = AssetDatabase.LoadAssetAtPath<SpawnHoops>(filePath);

        if (properties == null)
        {
            properties = CreateInstance<SpawnHoops>();

            AssetDatabase.CreateAsset(properties, filePath);

            AssetDatabase.SaveAssets();
        }

        return properties;
    }
# endif
}




