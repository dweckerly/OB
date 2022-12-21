using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Slot {
    Weapon,
    Body,
    Accessory,
    Head
}

public enum EquipmentType {
    Clothes,
    LightArmor,
    MediumArmor,
    HeavyArmor,
    Sword,
    Axe,
    Mace,
    Spear,
    Bow,
    Dagger,
    SmallShield,
    LargeShield,
    Helmet,
    Brooch,
    Amulet,
    Book,
    HeadWrap,
    Hat
}

[CreateAssetMenu(menuName = "Data/Equipment")]
public class Equipment : ScriptableObject
{
    public string Name;
    public Slot Slot;
    public EquipmentType Type;
    public List<StatBonus> StatBonuses;
    public List<ResistanceBonus> ResistanceBonuses;
    
}
