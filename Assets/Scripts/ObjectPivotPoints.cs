using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class ObjectPivotPoints : MonoBehaviour
{
    [SerializeField] private List<Pivot> m_pivotsUnsorted = new List<Pivot>();


    private Dictionary<Pivot.PivotPosition, Pivot> m_pivots = new Dictionary<Pivot.PivotPosition, Pivot>();

    public Dictionary<Pivot.PivotPosition, Pivot> Pivots { get => m_pivots; private set => m_pivots = value; }

    private void Awake()
    {
        foreach (var pivot in m_pivotsUnsorted)
        {
            Pivots.Add(pivot.GetPivotPosition, pivot);
        }
    }

    public Vector3 GetPivotPosition(Pivot.PivotPosition pivotPosition)
    {
        return Pivots[pivotPosition].PivotObj.transform.position;
    }

}
[System.Serializable]
public class Pivot
{
    [SerializeField] private GameObject m_pivotObj;

    public GameObject PivotObj { get => m_pivotObj; private set => m_pivotObj = value; }

    [SerializeField] public enum PivotPosition { Center, Top, Bottom, Left, Right, Front, Back }

    [SerializeField] private PivotPosition pivotPosition;

    public PivotPosition GetPivotPosition { get => pivotPosition; set => pivotPosition = value; }

}

