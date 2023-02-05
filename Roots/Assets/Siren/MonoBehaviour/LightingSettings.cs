using UnityEngine;

[CreateAssetMenu(fileName = "LightingSettings", menuName = "Settings/LightingSettings")]
public class LightingSettings : ScriptableObject
{
    public Gradient m_ambientColour;
    public Gradient m_directionalColour;
    public Gradient m_fogColour;
}