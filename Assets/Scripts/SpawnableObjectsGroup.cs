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

    [SerializeField] public bool[] lockedSpawnRates = new bool[0];

    public T GetObject()
    {
        var randomVal = Random.Range(0.0001f, 100f);

        var spawnRate = ObjectSpawnRates[0];

        for (int i = 0; i < ObjectSpawnRates.Length; i++)
        {
            if (spawnRate > randomVal)
            {
                return Objects[i];

            }
            else
            {
                spawnRate += ObjectSpawnRates[i];
            }
        }

        //Default to first item in list in the case of an error
        return Objects[0];
    }

#if UNITY_EDITOR
    public enum valueLockType { unlocked = 0, locked = 1, lockedRuntime = 2 }
    public static float[] DistributeValuesEditor(valueLockType[] lockedValues, float[] values, float newValue, float oldValue, int index)
    {
        var lockedValuesBool = new bool[lockedValues.Length];

        for (int i = 0; i < lockedValues.Length; i++)
        {
            if(lockedValues[i] != valueLockType.unlocked)
                lockedValuesBool[i] = true; 
        }

        return DistributeValues(lockedValuesBool, values, newValue, oldValue, index);
    }
#endif

    public static float[] DistributeValues(bool[] lockedValues, float[] values, float newValue, float oldValue, int index)
    {

        var unlockedCount = 0;

        var availableValues = new List<float>();

        var maxValue = 100f;


        for (int i = 0; i < lockedValues.Length; i++)
        {
            if (lockedValues[i] == false && i != index)
            {
                unlockedCount++;
                availableValues.Add(values[i]);
            }
            else if (lockedValues[i] == true)
            {
                maxValue -= values[i];
            }
        }


        if (availableValues.Count == 1 || maxValue == 0)
        {
            values[index] = oldValue;

            return values;
        }

        bool Adding = true;

        if (newValue > oldValue)
            Adding = false;



        if (Adding)
        {
            //var amount = (oldValue - newValue) / (unlockedCount);

            var amount = (oldValue - newValue) / (availableValues.Count);

            //for (int i = 0; i < values.Length; i++)
            //{


            //    if (lockedValues[i] == false)
            //    {

            //        if (i != index)
            //            values[i] += amount;

            //        values[i] = Mathf.Clamp(values[i], 0f, maxValue);
            //    }
            //}

            for (int i = 0; i < availableValues.Count; i++)
            {
                availableValues[i] += amount;

                availableValues[i] = Mathf.Clamp(values[i], 0f, maxValue);
            }
        }
        else
        {
            var takeAway = Mathf.Abs(oldValue - newValue);

            var iter = 0;

            while (takeAway > 0)
            {
                if (iter > 10)
                    break;
                iter++;
                //var amount = (takeAway) / (unlockedCount - 1);

                var amount = (takeAway) / (availableValues.Count);

                //for (int i = 0; i < unlockedList.Count; i++)
                //{

                //    if (unlockedList[i] >= amount)
                //    {

                //        unlockedList[i] += amount * sign;

                //        takeAway -= amount;


                //        unlockedList[i] = Mathf.Clamp(unlockedList[i], 0f, maxValue);

                //    }
                //    else if (unlockedList[i] < amount)
                //    {
                //        takeAway -= unlockedList[i];

                //        unlockedList[i] = 0;
                //        unlockedList.RemoveAt(i);
                //    }


                //}

                for (int i = 0; i < values.Length; i++)
                {
                    if (lockedValues[i] == false)
                    {

                        if (values[i] >= amount)
                        {
                            if (i != index)
                            {
                                values[i] -= amount;

                                takeAway -= amount;
                            }

                            values[i] = Mathf.Clamp(values[i], 0f, maxValue);

                        }
                        else if (values[i] != 0 && values[i] < amount)
                        {
                            takeAway -= values[i];

                            values[i] = 0;
                            unlockedCount--;
                        }

                    }
                }
            }
        }


        var control = 0f;

        for (int i = 0; i < values.Length; i++)
        {
            control += values[i];
        }

        //if (control != 100)
        //{
        //    for (int i = 0; i < availableValues.Count; i++)
        //    {
        //        var difference = control - 100;

        //        if(difference >= 0)
        //        {
        //            if (availableValues[1] > 0 + difference)
        //            {
        //                availableValues[i] -= difference;
        //                break;
        //            }
        //        }
        //        else
        //        {
        //            if (availableValues[1] < 100 - difference)
        //            {
        //                availableValues[i] += difference;
        //                break;
        //            }
        //        }

        //    }
        //}

        return values;
    }
}
