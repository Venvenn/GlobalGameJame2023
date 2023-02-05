using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI _debtText;
    [SerializeField] TMPro.TextMeshProUGUI _balanceText;

    public void UpdateBalance(int debt, int balance)
    {
        _debtText.text = '-' + debt.ToString();
        _balanceText.text = balance.ToString();
    }
}
