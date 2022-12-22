using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BaseResistances {
    Physical,
    Magical,
    Fire,
    Cold,
    Wind,
    Earth
}

public enum BaseStats {
    Strength,
    Vitality,
    Intelligence,
    Mentality,
    Agility,
    Dexterity,
    Luck
}

public class Soldier : MonoBehaviour
{
    const int BASE_STAT_LOWER = 3;
    const int BASE_STAT_UPPER = 18;
    const int MAX_STAT_VARIANCE = 3;
    const int PRIMARY_STAT_MOD = 100;
    const int SECONDARY_STAT_MOD = 25;
    const int STAT_DIVISOR = 376;

    public int Number;
    public string Name;
    public int Level;
    public int Experience = 0;
    public int Alignment = 50;
    public Job Job;
    public int PhysicalDefense;
    public int MagicalDefense;
    public int PhysicalAttack;
    public int MagicalAttack;
    public Dictionary<BaseResistances, int> Resistances;
    public Dictionary<BaseStats, int> Stats;
    public Dictionary<Slot, Equipment> Equipment;

    public Soldier(int number, string name, int level, Job job)
    {
        Number = number;
        Name = name;
        Level = level;
        Job = job;
        CalculateBaseStats();
        for (int i = 0; i < Level; i++)
        {
            LevelUpStats();
        }
        EquipDefault();
        CalculateEquipmentStatBonuses();
        CalculateDefenses();
        InstantiateResistances();
        CalculateResistances();
    }

    public void AddExperience(int amount)
    {
        Experience += amount;
        if (Experience >= 100) LevelUp();
    }

    public void ChangeEquipment(Equipment equip)
    {
        RemoveEquipmentBonuses();
        Equipment[equip.Slot] = equip;
        CalculateEquipmentStatBonuses();
        CalculateDefenses();
        CalculateResistances();
    }

    private void LevelUp()
    {
        Experience = 0;
        Level++;
        LevelUpStats();
        CalculateDefenses();
        // will need to be careful of the Job resistance bonus increasing the resistances every level
        // instantiating the resistances each time should fix??
        InstantiateResistances();
        CalculateResistances();
    }

    private void LevelUpStats()
    {
        foreach (StatBonus sb in Job.StatProgressions)
        {
            int val = sb.Amount + UnityEngine.Random.Range(MAX_STAT_VARIANCE * -1, MAX_STAT_VARIANCE + 1);
            if (val < 1) val = 1;
            Stats[sb.Stat] += val; 
        }
    }

    private void InstantiateResistances()
    {
        foreach (BaseResistances res in Enum.GetValues(typeof(BaseResistances)))
        {
            Resistances.Add(res, 0);
        }
    }

    private void CalculateEquipmentStatBonuses()
    {
        foreach (Equipment e in Equipment.Values)
        {
            foreach (StatBonus sb in e.StatBonuses)
            {
                Stats[sb.Stat] += sb.Amount;
            }
        }
    }

    private void RemoveEquipmentBonuses()
    {
        foreach (Equipment e in Equipment.Values)
        {
            foreach (StatBonus sb in e.StatBonuses)
            {
                Stats[sb.Stat] -= sb.Amount;
            }
            foreach (ResistanceBonus rb in e.ResistanceBonuses)
            {
                Resistances[rb.Resistance] -= rb.Amount;
            }
        }
    }

    private void CalculateDefenses()
    {
        PhysicalDefense = Mathf.RoundToInt(((Stats[BaseStats.Vitality] + PRIMARY_STAT_MOD) * (Stats[BaseStats.Strength] + SECONDARY_STAT_MOD) * (Job.PhysicalDefense / 100)) / STAT_DIVISOR);
        MagicalDefense = Mathf.RoundToInt(((Stats[BaseStats.Mentality] + PRIMARY_STAT_MOD) * (Stats[BaseStats.Intelligence] + SECONDARY_STAT_MOD) * (Job.MagicalDefense / 100)) / STAT_DIVISOR);
    }

    private void CalculateAttacks()
    {
        PhysicalAttack = Mathf.RoundToInt(((Stats[BaseStats.Strength] + PRIMARY_STAT_MOD) * (Stats[BaseStats.Dexterity] + SECONDARY_STAT_MOD) * (Job.PhysicalAttack / 100)) / STAT_DIVISOR);
        MagicalAttack = Mathf.RoundToInt(((Stats[BaseStats.Intelligence] + PRIMARY_STAT_MOD) * (Stats[BaseStats.Mentality] + SECONDARY_STAT_MOD) * (Job.MagicalAttack / 100)) / STAT_DIVISOR);
    }

    private void CalculateResistances()
    {
        foreach (BaseResistances resistance in Enum.GetValues(typeof(BaseResistances)))
        {
            Resistances[resistance] = GetEquipmenResistance(resistance);
            foreach (ResistanceBonus rb in Job.ResistanceBonuses)
            {
                if (rb.Resistance == resistance) Resistances[resistance] += rb.Amount;
                break;
            }
        }
    }

    private void CalculateBaseStats()
    {
        foreach(BaseStats stat in Enum.GetValues(typeof(BaseStats)))
        {
            Stats.Add(stat, UnityEngine.Random.Range(BASE_STAT_LOWER, BASE_STAT_UPPER + 1));
        }
    }

    private void EquipDefault() 
    {
        foreach(SlotEquipment se in Job.DefaultEquipment)
        {
            Equipment.Add(se.Slot, se.Equipment);
        }
    }

    private int GetEquipmenResistance(BaseResistances res) 
    {
        int val = 0;
        foreach (Equipment e in Equipment.Values)
        {
            foreach (ResistanceBonus rb in e.ResistanceBonuses)
            {
                if (rb.Resistance == res) val += rb.Amount;
                break;
            }
        }
        return val;
    }
}
