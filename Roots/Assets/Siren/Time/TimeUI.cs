using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Siren;

public class TimeUI : MonoBehaviour
{
    public Image m_clockFace;
    public TextMeshProUGUI m_dateText;
    public TimeSettings m_timeSettings;
    public TextMeshProUGUI m_timeText;

    [SerializeField] private FlowUIGroup _flowUIGroup;

    string previousMonth = string.Empty;

    public void Start()
    {
        SetDateText();
    }

    public void Update()
    {
        SetDateText();
    }


    public void SetDateText()
    {
        var time = TimeSystem.GameTime01;
        var hours = Mathf.FloorToInt(time * 24);
        var minutes = Mathf.RoundToInt((time * 24 - hours) * 60 * 100) / 100;


        m_timeText.text = TimeSystem.DayPhase + Environment.NewLine + hours + ":" + minutes.ToString("d2") +
                          (time < 0.5 ? "AM" : "PM");

        var dayText = TimeSystem.Date.Day.ToString("d2");
        var monthText = TimeSystem.Date.Month.ToString("d2");
        var yearText = TimeSystem.Date.Year.ToString("d2");
        m_dateText.text = $"{dayText}/{monthText}/{yearText}";

        if (previousMonth == string.Empty)
        {
            previousMonth = monthText;
        }

        //horrible, disgusting, ugly month check
        if (previousMonth != monthText)
        {
            _flowUIGroup.SendMessage(new NewMonthMessage());
            previousMonth = monthText;
        }

            //float phaseTime = m_timeSettings.m_phaseOffset / 24;
            if (time != 0)
        {
            m_clockFace.transform.rotation = Quaternion.Euler(Vector3.LerpUnclamped(new Vector3(0, 0, 0), new Vector3(0, 0, -360), time));
        }

    }
}