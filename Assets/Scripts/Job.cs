using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Terrains {
    Plains,
    Forest,
    Mountains
}

[CreateAssetMenu(menuName = "Data/Job")]
public class Job : ScriptableObject
{
    public string Name;
    public int PhysicalAttack;
    public int MagicalAttack;
    public int PhysicalDefense;
    public int MagicalDefense;
    public int ItemSlots;
    public Terrains TerrainType;
    public List<StatBonus> StatProgressions;
    public List<ResistanceBonus> ResistanceBonuses;
    public List<PositionalAttack> PositionalAttacks;
    public List<EquipmentSlotType> EquipmentTypes;
    public List<SlotEquipment> DefaultEquipment;
}
