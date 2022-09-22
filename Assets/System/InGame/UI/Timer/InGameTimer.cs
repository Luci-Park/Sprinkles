using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTimer : BasicTimer
{
    [SerializeField] GameObject beforeThresholdImage;
    [SerializeField] GameObject afterThresholdImage;
    [SerializeField] Text timerText;
    [SerializeField] float thresholdTime;

    GameObject currentUI;


    //---------------------------------------------
    #region Public Functions
    //---------------------------------------------

    public void StartGameCountDown()
    {
        TimeMoreThanThresh();
        StartTimer();
    }

    //---------------------------------------------

    public void ResetTimer()
    {
        TimeMoreThanThresh();
        timer = startTime;
        SetText();
        ShowTimer();
    }

    //---------------------------------------------

    public void HideTimer()
    {
        Color color = Color.white;
        color.a = 0f;
        timerText.color = color;
        currentUI.SetActive(false);

    }

    //---------------------------------------------

    public void ShowTimer()
    {
        Color color = Color.white;
        color.a = 1f;
        if(currentUI!= null) currentUI.SetActive(true);
        timerText.color = color;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Private Functions
    //---------------------------------------------

    void SetText()
    {
        timerText.text = timer.ToString("F0");
    }

    //---------------------------------------------

    void TimeMoreThanThresh()
    {
        beforeThresholdImage.SetActive(true);
        afterThresholdImage.SetActive(false);
        currentUI = beforeThresholdImage;
    }

    //---------------------------------------------

    void TimeLessThanThresh()
    {
        beforeThresholdImage.SetActive(false);
        afterThresholdImage.SetActive(true);
        currentUI = afterThresholdImage;
        BGM.instance.StartCoroutine(BGM.instance.PlayUrgentBGM());
    }

    //---------------------------------------------

    bool TimeIsOver()
    {
        return timer <= 0;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Override Functions
    //---------------------------------------------    
    protected override void TimeOver()
    {
        GameManager.instance.GameDone();
    }

    protected override void Tick()
    {
        SetText();
        if(timer == thresholdTime)
        {
            TimeLessThanThresh();
        }
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------


   

}
