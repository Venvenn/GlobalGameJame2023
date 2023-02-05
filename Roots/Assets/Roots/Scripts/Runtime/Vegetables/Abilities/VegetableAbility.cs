using System;
using Unity.Mathematics;

[Serializable]
public struct VegetableAbility
{
    public AbilityType AbilityType;
    public int2 GridDistance;
    public int Amount;
    public float Frequency;
    public VegetableDataObject Target;
}
