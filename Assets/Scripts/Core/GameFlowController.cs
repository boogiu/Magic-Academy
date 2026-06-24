using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    [SerializeField] private AcademyClock academyClock;

    private float currentTimeScale = 1f;
    private bool isEncounterActive = false;

    public void SetTimeSpeed(float scale)
    {
        if (isEncounterActive) return;

        currentTimeScale = scale;
        academyClock.SetTimeScale(scale);
    }

    public void SetPause(bool pause)
    {
        academyClock.SetPause(pause);
        if (!pause)
            academyClock.SetTimeScale(currentTimeScale);
    }

    public void OnEncounterStart()
    {
        isEncounterActive = true;
        SetPause(true);
    }

    public void OnEncounterFinish()
    {
        isEncounterActive = false;
        SetPause(false);
    }
}
