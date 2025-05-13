using UnityEngine;
public class Stopwatch : MonoBehaviour
{
    public float CurrentTimeSeconds => Mathf.Round(currentTime* 100) / 100.0f;
    private float currentTime;

    public StopwatchState State { get; private set; }

    private void FixedUpdate()
    {
        if (State == StopwatchState.Running)
        {
            currentTime += Time.deltaTime;
        }
    }

    public void StartStopwatch()
    {
        State = StopwatchState.Running;
        currentTime = 0;
    }

    public void PauseStopwatch()
    {
        State = StopwatchState.Paused;
    }

    public void ResumeStopwatch()
    {
        State = StopwatchState.Running;
    }

    public void ResetStopWatch()
    {
        State = StopwatchState.Stopped;
        currentTime = 0;
    }

    public string GetTime()
    {
        int min = (int)(currentTime / 60);
        int sec = (int)(currentTime % 60);

        string strMin = min >= 10 ? $"{min}" : $"0{min}";
        string strSec = sec >= 10 ? $"{sec}" : $"0{sec}";

        return $"{strMin}:{strSec}";
    }
}
