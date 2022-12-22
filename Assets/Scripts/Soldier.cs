using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Elements {
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

public class Soldier
{
    const int BASE_STAT_LOWER = 3;
    const int BASE_STAT_UPPER = 18;
    const int MAX_STAT_VARIANCE = 3;
    const int PRIMARY_STAT_MOD = 100;
    const int SECONDARY_STAT_MOD = 25;
    const int DEF_DIVISOR = 376;
    const int ATK_DIVISOR = 280;

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
    public Dictionary<Elements, int> Resistances = new Dictionary<Elements, int>();
    public Dictionary<BaseStats, int> Stats = new Dictionary<BaseStats, int>();
    public Dictionary<Slot, Equipment> Equipment = new Dictionary<Slot, Equipment>();

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
        CalculateAttacks();
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
        CalculateAttacks();
        CalculateResistances();
    }

    private void LevelUp()
    {
        Experience = 0;
        Level++;
        RemoveEquipmentBonuses();
        LevelUpStats();
        CalculateEquipmentStatBonuses();
        CalculateDefenses();
        CalculateAttacks();
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
        PhysicalDefense = Mathf.RoundToInt(((Stats[BaseStats.Vitality] + PRIMARY_STAT_MOD) * (Stats[BaseStats.Strength] + SECONDARY_STAT_MOD) * (Job.PhysicalDefense / 100)) / DEF_DIVISOR);
        MagicalDefense = Mathf.RoundToInt(((Stats[BaseStats.Mentality] + PRIMARY_STAT_MOD) * (Stats[BaseStats.Intelligence] + SECONDARY_STAT_MOD) * (Job.MagicalDefense / 100)) / DEF_DIVISOR);
    }

    private void CalculateAttacks()
    {
        PhysicalAttack = Mathf.RoundToInt(((Stats[BaseStats.Strength] + PRIMARY_STAT_MOD) * (Stats[BaseStats.Dexterity] + SECONDARY_STAT_MOD) * (Job.PhysicalAttack / 100)) / ATK_DIVISOR);
        MagicalAttack = Mathf.RoundToInt(((Stats[BaseStats.Intelligence] + PRIMARY_STAT_MOD) * (Stats[BaseStats.Mentality] + SECONDARY_STAT_MOD) * (Job.MagicalAttack / 100)) / ATK_DIVISOR);
    }

    private void CalculateResistances()
    {
        Resistances.Clear();
        foreach (Elements res in Enum.GetValues(typeof(Elements)))
        {
            Resistances.Add(res, 0);
        }
        foreach (Elements resistance in Enum.GetValues(typeof(Elements)))
        {
            Resistances[resistance] = GetEquipmenResistance(resistance);
            foreach (ResistanceBonus rb in Job.ResistanceBonuses)
            {
                if (rb.Resistance == resistance) Resistances[resistance] += rb.Amount;
                break;
            }
        }
    }

    public int CalculatePhysicalDamage(int enemyAtk, Elements atkElement)
    {
        int damage = Mathf.RoundToInt(((enemyAtk * 100) / (PhysicalDefense + 100)) * (1 - (Resistances[atkElement] / 100)));
        if (damage <= 0) return 1;
        return damage;
    }

    public int CalculateMagicalDamage(int enemyAtk, Elements atkElement)
    {
        int damage = Mathf.RoundToInt(((enemyAtk * 100) / (MagicalDefense + 100)) * (1 - (Resistances[atkElement] / 100)));
        if (damage <= 0) return 1;
        return damage;
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

    private int GetEquipmenResistance(Elements res) 
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
