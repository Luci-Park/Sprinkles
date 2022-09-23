using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningCountDown : ImageSoundTimer
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
    #endregion
    //---------------------------------------------
}
