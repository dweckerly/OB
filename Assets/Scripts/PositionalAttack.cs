using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Positions {
    Front,
    Middle,
    Back
}

[System.Serializable]
public class PositionalAttack
{
    public Positions Position;
    public Attack Attack;
}
