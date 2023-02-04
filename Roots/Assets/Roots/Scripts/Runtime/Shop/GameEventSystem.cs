using UnityEngine;

public static class GameEventSystem
{
    private const int k_merchantDay = 1;
    private const int k_merchantHour = 9;
    
    private const int k_debtDay = 1;
    private const int k_debtHour = 8;
    
    public static bool MerchantArrive()
    {
        if (TimeSystem.Date.Day == k_merchantDay && TimeSystem.GetTimeDate().Hours == k_merchantHour)
        {
            return true;
        }
        
        return false;
    }
    
    public static bool DebtCollected()
    {
        if (TimeSystem.Date.Day == k_debtDay && TimeSystem.GetTimeDate().Hours == k_debtHour)
        {
            return true;
        }
        
        return false;
    }
}
