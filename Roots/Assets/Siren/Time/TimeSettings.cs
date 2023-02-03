using UnityEngine;

[CreateAssetMenu(fileName = "TimeData", menuName = "Data/TimeData")]
public class TimeSettings : ScriptableObject
{
    public float m_dayLength = 5;
    public Month[] m_months;

    public int m_phaseOffset;

    public Date m_startDate;
}