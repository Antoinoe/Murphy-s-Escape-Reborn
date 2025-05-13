using TMPro;
using UnityEngine;

[RequireComponent(typeof(Stopwatch))]
public class StopwatchManager : MonoBehaviour
{
    [field : SerializeField] public Stopwatch Stopwatch { get; private set; }
    [SerializeField] private TextMeshProUGUI stopwatchUi;
    [SerializeField] private TextMeshProUGUI finalTimeUi;

    private string finalTime;

    private void Start()
    {
        GameManager.Instance.OnGameStarts.AddListener(OnGameStarts);
        GameManager.Instance.OnGamePause.AddListener(OnGamePause);
        GameManager.Instance.OnPlayerFinishesGame.AddListener(OnGameEnds);
    }

    private void FixedUpdate()
    {
        if(Stopwatch != null && !GameManager.Instance.IsGamePaused)
            stopwatchUi.text = $"TIME : {Stopwatch.GetTime()}";
    }

    private void OnGameEnds()
    {
        if (Stopwatch == null)
            return;

        finalTime = Stopwatch.GetTime();
        finalTimeUi.text = $"FINAL TIME : {finalTime}";
        Stopwatch.ResetStopWatch();
    }

    private void OnGamePause(bool isPaused)
    {
        if (Stopwatch == null)
            return;

        if (isPaused)
            Stopwatch.PauseStopwatch();
        else
            Stopwatch.ResumeStopwatch();
    }

    private void OnGameStarts()
    {
        finalTime = "";
        Stopwatch.StartStopwatch();
    }
}
