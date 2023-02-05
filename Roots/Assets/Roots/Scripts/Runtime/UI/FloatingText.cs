using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    public void SetText(string text)
    {
        _text.text = text;
    }
    
    
    
}
