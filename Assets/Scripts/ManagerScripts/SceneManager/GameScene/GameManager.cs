using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public enum playerStatus
{
    isPlaying = 0,
    GameOver = 1,
    Paused = 2
}

public class GameManager : MonoSingleton<GameManager>
{
    #region INITIALIZE COMPONENTS
    private PlayerScript player;

    private int currentMapID = 1;

    private bool isRevived;

    private UnityAction<playerStatus> StatusChangedCallback;
    #endregion

    #region FUNCTIONS
    public void PlayMap(int mapID)
    {
        this.currentMapID = mapID;
    }

    private void InvokeOnPlayerStateChangedCallback(playerStatus playerState)
    {
        this.StatusChangedCallback?.Invoke(playerState);
    }

    public void OnPlayerSwiped(TouchDirection direction)
    {
        switch (direction)
        {
            case TouchDirection.Up:
                this.player.RegisterNextMove(Movement.Up); break;
            case TouchDirection.Left:
                this.player.RegisterNextMove(Movement.Left); break;
            case TouchDirection.Down:
                this.player.RegisterNextMove(Movement.Down); break;
            case TouchDirection.Right:
                this.player.RegisterNextMove(Movement.Right); break;
        }
    }

    public void OnPlayerDead()
    {
        GameInputManager.Instance.UnAssignOnPlayerSwipedCallback(this.OnPlayerSwiped);
        GameInputManager.Instance.SetGameActive(false);

        this.InvokeOnPlayerStateChangedCallback(playerStatus.GameOver);
    }

    public void OnPlayerPaused()
    {
        GameInputManager.Instance.UnAssignOnPlayerSwipedCallback(this.OnPlayerSwiped);
        GameInputManager.Instance.SetGameActive(false);

        this.InvokeOnPlayerStateChangedCallback(playerStatus.Paused);
    }

    public void OnPlayerResume()
    {
        GameInputManager.Instance.SetGameActive(true);
        GameInputManager.Instance.AssignOnPlayerSwipedCallback(this.OnPlayerSwiped);

        this.InvokeOnPlayerStateChangedCallback(playerStatus.isPlaying);
    }

    public void PseudoInputProcess()
    {
        if (Input.GetKeyDown(KeyCode.W))
            this.player.RegisterNextMove(Movement.Up);

        if (Input.GetKeyDown(KeyCode.A))
            this.player.RegisterNextMove(Movement.Left);

        if (Input.GetKeyDown(KeyCode.S))
            this.player.RegisterNextMove(Movement.Down);

        if (Input.GetKeyDown(KeyCode.D))
            this.player.RegisterNextMove(Movement.Right);
    }

    public int GetMapID()
    {
        return currentMapID;
    }

    public PlayerScript GetPlayer()
    {
        return this.player;
    }

    #endregion

    public void StartGame(PlayerScript inputPlayer)
    {
        this.player = inputPlayer;
        this.player.StartGame();

        GameUIManager.Instance.StartGame();
        GameInputManager.Instance.StartGame();
        GameInputManager.Instance.AssignOnPlayerSwipedCallback(this.OnPlayerSwiped);
    }

    private void Update()
    {
        this.PseudoInputProcess();
    }
}
