using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ImageSoundTimer : BasicTimer
{
    [SerializeField] protected GameObject[] countDown;
    [SerializeField] protected AudioClip[] countSound;

    protected AudioSource audioSource;
    protected GameObject currentSelected;
    
    
    
    //---------------------------------------------
    #region Public Functions
    //---------------------------------------------
    public void StartCountDown()
    {
        currentSelected = countDown[countDown.Length - 1];
        StartTimer();
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Private Functions
    //---------------------------------------------
    protected void TurnOnOne(int index)
    {
        currentSelected.SetActive(false);
        countDown[index].SetActive(true);
        currentSelected = countDown[index];
    }
    //---------------------------------------------
    protected void TurnAllOff()
    {
        for (int i = 0; i < countDown.Length; i++)
        {
            countDown[i].SetActive(false);
        }
    }
    //---------------------------------------------
    #endregion
    //--------------------------------------------- 
    
    
    //---------------------------------------------
    #region override
    //---------------------------------------------
    protected override void TimeOver()
    {
        currentSelected.SetActive(false);
    }
    //---------------------------------------------
    protected override void Tick()
    {
        int idx = timer - 1;
        TurnOnOne(idx);
        audioSource.PlayOneShot(countSound[idx]);
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------

}
