using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGamePaused { get; private set; }

    [HideInInspector] public UnityEvent<bool> OnGamePause;
    [HideInInspector] public UnityEvent OnPlayerDie;
    [HideInInspector] public UnityEvent OnGameStarts;
    [HideInInspector] public UnityEvent OnPlayerLevelUp;
    [HideInInspector] public UnityEvent OnPlayerFinishesGame;

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
        IsGamePaused = pause;
        OnGamePause?.Invoke(IsGamePaused);
    }

    public Transform GetSpawnPoint()
        => levelManager.SpawnPointRoot;
}
