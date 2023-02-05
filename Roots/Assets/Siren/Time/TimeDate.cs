using System;

[Serializable]
public struct TimeDate
{
    public int Minutes;
    public int Hours;
    public int Day;
    public int Month;
    public int Year;

    public TimeDate(int minutes, int hours, int day, int month, int year)
    {
        Minutes = minutes;
        Hours = hours;
        Day = day;
        Month = month;
        Year = year;
    }
}