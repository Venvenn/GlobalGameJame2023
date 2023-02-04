using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VegetableStateData
{
    public float Growth;
    public float Health;
    public float Quality;

    public VegetableStateData(float growth, float health, float quality)
    {
        Growth = growth;
        Health = health;
        Quality = quality;
    }
}
