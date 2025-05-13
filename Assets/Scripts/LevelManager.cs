using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [field: SerializeField] public int LevelIterationCounter { get; private set; }

    [field: SerializeField] public Transform SpawnPointRoot { get; private set; }

    [SerializeField] private LevelObjects[] levelObjects = new LevelObjects[4];
    [SerializeField] private GameObject mainMenuRoot;
    [SerializeField] private GameObject winMenuRoot;
    [SerializeField] private GameObject creditMenuRoot;
    [SerializeField] private Transform levelObjectsRoot;
    [SerializeField] private Transform endPointRoot;

    private void Start()
    {
        GameManager.Instance.OnApplicationStarts.AddListener(() => OnApplicationStarts());
        GameManager.Instance.OnGameStarts.AddListener(() => OnGameStarts());
        GameManager.Instance.OnPlayerFinishesGame.AddListener(() => OnPlayerFinishesGame());
        GameManager.Instance.OnPlayerReachesEndPortal.AddListener(() => OnPlayerTriggerEndPortal());
    }

    private void OnPlayerTriggerEndPortal()
    {
        LevelIterationCounter++;
        ChangeLevel(LevelIterationCounter);
    }

    private void OnApplicationStarts()
    {
        DisplayMainMenu();
        HideLevels();
        LevelIterationCounter = 0;
    }

    public void ChangeLevel(int level)
    {
        EnableMainMenu(false);
        HideLevels();
        DisplayLevel(level);
        GameManager.Instance.OnLevelDisplayed?.Invoke();
    }

    private void DisplayLevel(int level)
    {
        if (level <= 0)
            return;

        if (level > levelObjects.Length)
        {
            GameManager.Instance.OnPlayerFinishesGame?.Invoke();
            return;
        }

        levelObjects.Where(x => x.Level == level).FirstOrDefault().ObjectsToDisplay.ForEach(y => y.SetActive(true));
    }

    /// <summary>
    /// Start Level 1 by default
    /// </summary>
    public void StartGame()
    {
        EnableMainMenu(false);
        DisplayVictoryMenu(false);
        LevelIterationCounter++;
        ChangeLevel(LevelIterationCounter);
    }

    public void DisplayMainMenu()
    {
        DisplayVictoryMenu(false);
        HideLevels();
        EnableMainMenu(true);
    }

    private void EnableMainMenu(bool displayMenu)
        => mainMenuRoot.SetActive(displayMenu);

    private void DisplayVictoryMenu(bool displayMenu)
        => winMenuRoot.SetActive(displayMenu);

    public void HideLevels()
    {
        foreach (var obj in levelObjects) 
        {
            foreach(var o in obj.ObjectsToDisplay) { o.SetActive(false); }
        }
    }

    private void OnGameStarts()
        => StartGame();

    public void QuitGame()
        => GameManager.Instance.QuitGame();

    public void OnPlayerFinishesGame()
    {
        LevelIterationCounter = 0;
        HideLevels();
        EnableMainMenu(false);
        DisplayVictoryMenu(true);
    }
}
