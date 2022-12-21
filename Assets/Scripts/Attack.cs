using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType {
    Physical,
    Magical
}

[CreateAssetMenu(menuName = "Data/Attack")]
public class Attack : ScriptableObject
{
    public string Name;
    public AttackType Type;
}
