using UnityEngine;
using UnityEngine.Events;

public enum Season { Spring, Summer, Autumn, Winter }

public class AcademyClock : MonoBehaviour
{
    [Header("Time Setting")]
    [SerializeField] private float secondsPerDay = 3f;

    public int day { get; private set; } = 1;
    public int month { get; private set; } = 1;
    public int year { get; private set; } = 1;
    public Season currentSeason { get; private set; } = Season.Spring;

    public UnityEvent OnDayPassed;
    public UnityEvent<int> OnMonthPassed;
    public UnityEvent<Season> OnSeasonChanged;
    public UnityEvent<int> OnYearChanged;

    private float _timer = 0f;
    private bool _isPaused = false;

    private readonly int maxMonth = 12;
    private static readonly Season[] seasonPerMonth =
    {
        Season.Winter, Season.Winter, Season.Spring, Season.Spring,
        Season.Summer, Season.Summer, Season.Summer, Season.Summer,
        Season.Autumn, Season.Autumn, Season.Winter, Season.Winter
    };

    private static readonly int[] daysPerMonth =
    {
        30, 30, 30, 30,
        30, 30, 30, 30,
        30, 30, 30, 30
    };

    public void SetPause(bool pause) => _isPaused = pause;
    public void SetTimeScale(float scale) => Time.timeScale = scale;

    private void Update()
    {
        if (_isPaused) return;
        _timer += Time.deltaTime;
        if (_timer > secondsPerDay)
        {
            _timer -= secondsPerDay;
            AdvanceDay();
        }

        Debug.Log($"현재 날짜 : {year}년 {month}월 {day}일");
        Debug.Log($"계절 : {currentSeason}");

    }

    private void AdvanceDay()
    {
        day++;
        OnDayPassed?.Invoke();
        if (day > daysPerMonth[month-1])//0-Base
            AdvanceMonth();
    }

    private void AdvanceMonth()
    {
        day = 1;
        month++;
        OnMonthPassed?.Invoke(month);

        if (month > maxMonth)
        {
            AdvanceYear();
        }

        Season newSeason = seasonPerMonth[month - 1]; //0-Base
        if (newSeason != currentSeason)
            AdvanceSeason(newSeason);
    }

    private void AdvanceSeason(Season newSeason)
    {
        currentSeason = newSeason;               
        OnSeasonChanged?.Invoke(currentSeason);  
    }

    private void AdvanceYear()
    {
        month = 1;
        year++;
        OnYearChanged?.Invoke(year);
    }
}