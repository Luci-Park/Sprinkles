using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCountDown : ImageSoundTimer
{

    protected override void TimeOver()
    {
        base.TimeOver();
        TurnAllOff();
        LevelManager.instance.LeaveGame();
    }

    //---------------------------------------------
    #region Public Functions
    //---------------------------------------------

    public void StopCountDown()
    {
        StopTimer();
        TurnAllOff();
        GameManager.instance.PrepAndStart();
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
    #endregion
    //---------------------------------------------
}
