using System;
using UnityEngine;

public static class TimeSystem
{
    private static float s_gameTime;
    private static TimeSettings s_timeSettings;
    private static int s_leapYearBonus;
    private static float s_phaseOffsetValue;
    public static float GameTime01 => s_gameTime / s_timeSettings.m_dayLength;

    public static DayPhase DayPhase => (DayPhase) Mathf.FloorToInt(GetPhaseAdjustedTime01() * Enum.GetValues(typeof(DayPhase)).Length);
    public static Date Date { get; private set; }
    
    public static void Init(TimeSettings timeSettings)
    {
        s_timeSettings = timeSettings;
        Date = new Date(s_timeSettings._startDate.Day, s_timeSettings._startDate.Month, s_timeSettings._startDate.Year);
        s_leapYearBonus = Date.Year % 4 == 0 ? 1 : 0;
        s_phaseOffsetValue = s_timeSettings.m_phaseOffset / 24f;
        s_gameTime = s_phaseOffsetValue * s_timeSettings.m_dayLength;
    }

    public static TimeDate GetTimeDate()
    {
        var time = GameTime01;
        var hours = Mathf.FloorToInt(time * 24);
        var minutes = Mathf.RoundToInt((time * 24 - hours) * 60 * 100) / 100;
        return new TimeDate(minutes, hours, Date.Day, Date.Month, Date.Year);
    }

    public static TimeSpan GetTimeSpan(TimeDate date1, TimeDate date2)
    {
        DateTime dateTime1 = new DateTime(date1.Year, date1.Month, date1.Day);
        dateTime1 = dateTime1.Add(new TimeSpan(date1.Hours, date1.Minutes, 0));
        DateTime dateTime2 = new DateTime(date2.Year, date2.Month, date2.Day);
        dateTime2 = dateTime2.Add(new TimeSpan(date2.Hours, date2.Minutes, 0));
        return dateTime2 - dateTime1;
    }
    
    public static void UpdateTime()
    {
        s_gameTime += Time.deltaTime;
        if (s_gameTime >= s_timeSettings.m_dayLength)
        {
            s_gameTime = 0;

            //update day
            Date.Day++;

            //update month
            if (Date.Day > s_timeSettings.m_months[Date.Month - 1].Days + (Date.Month == 2 ? s_leapYearBonus : 0))
            {
                Date.Day = 1;
                Date.Month++;
            }

            //update year
            if (Date.Month > s_timeSettings.m_months.Length)
            {
                Date.Month = 1;
                Date.Year++;

                s_leapYearBonus = Date.Year % 4 == 0 ? 1 : 0;
            }
        }
    }

    public static float GetPhaseAdjustedTime01()
    {
        var time = s_gameTime / s_timeSettings.m_dayLength - s_phaseOffsetValue;
        if (time < 0) time = 1 + time;
        return time;
    }
}