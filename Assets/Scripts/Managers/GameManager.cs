using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    InGame,
    Pause,
    Win,
    Lose
}

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    #region [ MONOBEHAVIOR ]
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }
    #endregion
    #endregion

    public GameState CurrentGameState;

    // Seconds between the water starting and EndGame().
    public float TimeSurvived { get; private set; }

    private float _startTime;

    void Start()
    {
        CurrentGameState = GameState.MainMenu;
        GridManager.Instance.CreateGrid();
        WaterManager.Instance.Init();
        WaterManager.Instance.StartWater();
        _startTime = Time.time;
    }

    public void EndGame()
    {
        if (CurrentGameState == GameState.Lose)
            return;

        CurrentGameState = GameState.Lose;
        TimeSurvived = Time.time - _startTime;
        WaterManager.Instance.StopWater();

        MainMenuUI mainMenu = FindObjectOfType<MainMenuUI>();
        if (mainMenu == null)
        {
            Debug.LogWarning("GameManager.EndGame: no MainMenuUI in the scene, cannot show the game over screen.");
            return;
        }
        mainMenu.GameOverScreen.ActivatePanel();
    }

}
