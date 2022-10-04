using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningCountDown : ImageSoundTimer, IGameObserver
{
    //---------------------------------------------
    #region override
    //---------------------------------------------
    protected override void TimeOver()
    {
        base.TimeOver();
        GameManager.instance.StartGame();
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior
    //---------------------------------------------
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    //---------------------------------------------
    
    private void Start()
    {
        GameManager.instance.AddObserver(this);
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------

    //---------------------------------------------
    #region Observer Functions
    //---------------------------------------------
    public void NotifyPreparation()
    {
        StartCountDown();
    }
    //---------------------------------------------
    public void NotifyGameStart()
    {
    }
    //---------------------------------------------
    public void NotifyGameOver()
    {
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------
}
