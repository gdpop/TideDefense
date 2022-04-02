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

    public static GameManager Instance {
        get {
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

    public GameState ActualGameState;

    void Start()
    {
        ActualGameState = GameState.MainMenu;
        GridManager.Instance.CreateGrid();
    }

}
