using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameFlowController gameFlowController;
    [SerializeField] private AcademyClock academyClock;

    [Header("Date UI")]
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI seasonText;


    [Header("Speed Buttons")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button speed1Button;
    [SerializeField] private Button speed2Button;

    private void Start()
    {
        pauseButton?.onClick.AddListener(() =>  gameFlowController.SetPause(true));
        speed1Button?.onClick.AddListener(() => gameFlowController.SetTimeSpeed(1f));
        speed2Button?.onClick.AddListener(() => gameFlowController.SetTimeSpeed(2f));
       
        academyClock.OnDayPhaseChanged.AddListener(UpdateDateUI);
        academyClock.OnSeasonChanged.AddListener(UpdateSeasonUI);

        UpdateDateUI(academyClock.currentDayPhase);
        UpdateSeasonUI(academyClock.currentSeason);
    }

    private void UpdateDateUI(DayPhase phase)
    {
        if (dateText == null) return;
        dateText.text = $"{academyClock.year}³â {academyClock.month}¿ù {academyClock.day}ÀÏ "
            + phase switch
            {
                DayPhase.Morning => "¾ÆÄ§",
                DayPhase.Day => "³·",
                DayPhase.Evening => "Àú³á",
                DayPhase.Night => "¹ã",
                _ => ""
            };
    }

    private void UpdateSeasonUI(Season season)
    {
        if (seasonText == null) return;
        seasonText.text = season switch
        {
            Season.Spring => "º½",
            Season.Summer => "¿©¸§",
            Season.Autumn => "°¡À»",
            Season.Winter => "°Ü¿ï",
            _ => ""
        };
    }
}
