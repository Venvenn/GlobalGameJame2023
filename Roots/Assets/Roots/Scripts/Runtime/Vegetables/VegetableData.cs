using System;
using UnityEngine;

[Serializable]
public struct VegetableData
{
    public int GrowingTime;
    public int BaseValue;
    public int SeedValue;
    public int MaxHealth;
    public int HarvestNumber;
    public Sprite Icon;
    public VegetableObject Prefab;
    public VegetableAbility[] VegetableAbilities;
}
