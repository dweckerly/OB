using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGenTest : MonoBehaviour
{
    public Job[] jobs;

    void Start()
    {
        Soldier zero = new Soldier(0, "Zero", 1, jobs[0]);
        Soldier one = new Soldier(1, "One", 1, jobs[0]);
        Soldier two = new Soldier(2, "Two", 3, jobs[0]);
        Soldier three = new Soldier(3, "Three", 3, jobs[0]);

        print(JsonUtility.ToJson(zero, true));
        print(JsonUtility.ToJson(one, true));
        print(JsonUtility.ToJson(two, true));
        print(JsonUtility.ToJson(three, true));
    }
}
