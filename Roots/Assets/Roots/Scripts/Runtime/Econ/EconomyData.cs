using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyData
{
    private int _balance;
    private int _debt;
    private int _minimumPayment;

    public int Balance => _balance;
    public int Debt => _debt;
    public int MinimumPayment => _minimumPayment;

    public EconomyData(EconomyDataSettings settings)
    {
        _balance = settings.StartingBalance;
        _debt = settings.StartingDebt;
        _minimumPayment = settings.MinimumDebtPayment;
    }

    /// <summary>
    /// Charge monethly payment and check if the player has lost
    /// </summary>
    /// <returns></returns>
    public bool MonthTick()
    {
        _balance -= _minimumPayment;
        _debt -= _minimumPayment;

        if (_balance < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool Charge(int payment)
    {
        if (_balance > payment)
        {
            _balance -= payment;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddMoney(int payment)
    {
        _balance += payment;
    }
}
