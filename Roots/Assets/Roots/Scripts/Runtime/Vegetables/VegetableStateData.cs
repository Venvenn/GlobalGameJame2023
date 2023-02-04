using System;

public class VegetableStateData
{
    public float Growth;
    public float Health;
    public float Quality;
    public TimeDate PlantTime;

    public VegetableStateData(float growth, float health, float quality, TimeDate plantTime)
    {
        Growth = growth;
        Health = health;
        Quality = quality;
        PlantTime = plantTime;
    }
}
