using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct VegetableData
{
    public int GrowingTime;
    public int2 CropValue;
    public int2 SeedValue;
    public int MaxHealth;
    public int HarvestNumber;
    public int InitialStock;
    public int2 seedDropRange;
    public Sprite Icon;
    public VegetableObject Prefab;
    public VegetableAbility[] VegetableAbilities;
}
