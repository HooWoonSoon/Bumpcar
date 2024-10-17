using System;
using System.Collections.Generic;

public class CharacterStats
{
    public float BaseValue;

    public float Value { 
        get {
            if (isDirty)
            {
                _value = CalculateFinalValue();
                isDirty = false;
            } 
            return _value;
        }
        }

    private bool isDirty = true;
    private float _value;

    private readonly List<PlayerDataModifier> playerDataModifiers;

    public CharacterStats(float baseValue)
    {
        BaseValue = baseValue;
        playerDataModifiers = new List<PlayerDataModifier>();
    }

    public void AddModifier(PlayerDataModifier modifier)
    {
        isDirty = true;
        playerDataModifiers.Add(modifier); 
        playerDataModifiers.Sort(CompareModifierOrder);
    }

    private int CompareModifierOrder(PlayerDataModifier a, PlayerDataModifier b)
    {
        if (a.Order < b.Order)
        {
            return -1;
        }
        else if (a.Order > b.Order)
            return 1;
        return 0;
    }

    public bool RemoveModifier(PlayerDataModifier modifier)
    {
        isDirty = true;
        return playerDataModifiers.Remove(modifier);
    }

    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;

        for (int i = 0; i < playerDataModifiers.Count; i++)
        {
            PlayerDataModifier modifier = playerDataModifiers[i];

            if (modifier.Type == StataModType.Flat)
            {
                finalValue += playerDataModifiers[i].Value;
            }
            else if (modifier.Type == StataModType.Percent)
            {
                finalValue *= 1 + modifier.Value;
            }
        }

        return (float)Math.Round(finalValue, 4);
    }
}

