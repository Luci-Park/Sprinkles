using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicTimer : MonoBehaviour
{
    [SerializeField] protected int startTime;
    [SerializeField] protected float interval;
    protected int timer;
    
    
    
    //---------------------------------------------
    #region no override
    //---------------------------------------------
    protected abstract void TimeOver();
    //---------------------------------------------
    protected abstract void Tick();
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region no override
    //---------------------------------------------
    
    protected IEnumerator CountDown()
    {
        timer = startTime;
        while (timer > 0)
        {
            Tick();
            timer -= 1;
            yield return new WaitForSeconds(interval);
        }
        TimeOver();
    }
    
    //---------------------------------------------
    
    protected void StartTimer()
    {
        StartCoroutine(CountDown());
    }

    //---------------------------------------------

    protected void StopTimer()
    {
        StopCoroutine(CountDown());
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------
}
