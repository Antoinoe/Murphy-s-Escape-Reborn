using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGamePaused { get; private set; }

    [HideInInspector] public UnityEvent OnApplicationStarts;
    [HideInInspector] public UnityEvent<bool> OnGamePause;
    [HideInInspector] public UnityEvent OnPlayerDie;
    /// <summary>
    /// When the user clicks on the start button
    /// </summary>
    [HideInInspector] public UnityEvent OnGameStarts;
    [HideInInspector] public UnityEvent OnPlayerReachesEndPortal;
    [HideInInspector] public UnityEvent OnPlayerFinishesGame;
    [HideInInspector] public UnityEvent OnLevelDisplayed;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Initialize();
    }

    private void Start()
    {
        OnApplicationStarts?.Invoke();
        
    }

    private void Initialize()
    {
        //levelManager.DisplayMainMenu();
    }

    public void StartGame()
    {
        OnGameStarts?.Invoke();
        Debug.Log("Game starts!");
    }

    public void QuitGame()
        => Application.Quit();

    public void SetPause(bool pause)
    {
        Debug.Log($"Set Pause {pause}");
        IsGamePaused = pause;
        OnGamePause?.Invoke(IsGamePaused);
    }

    public Transform GetSpawnPoint()
        => levelManager.SpawnPointRoot;
}
