using UnityEngine;

[CreateAssetMenu(fileName = "EconomySettings", menuName = "Data/EconomySettings")]
public class EconomyDataSettings : ScriptableObject
{
    public int StartingBalance = 1000;
    public int StartingDebt = 100000;
    public int MinimumDebtPayment = 250;
}
