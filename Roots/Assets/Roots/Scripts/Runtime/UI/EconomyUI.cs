using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI _debtText;
    [SerializeField] TMPro.TextMeshProUGUI _balanceText;
    [SerializeField] TMPro.TextMeshProUGUI _paymentText;

    public void UpdateBalance(int debt, int balance, int payment)
    {
        _debtText.text = '-' + debt.ToString();
        _balanceText.text = balance.ToString();
        _paymentText.text = '-' + payment.ToString() + " Monthly";
    }
}
