using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTimer : MonoBehaviour
{
    [SerializeField] GameObject beforeThreshold;
    [SerializeField] GameObject afterThreshold;
    [SerializeField] Text timerText;
    [SerializeField] GameManager gameManager;
    [SerializeField] float gameTime;
    [SerializeField] float thresholdTime;

    GameObject currentUI;
    float endTime = 0f;
    float timer;

    bool timerOn = false;
    bool timeLessThanThresh = false;


    //---------------------------------------------
    #region Public Functions
    //---------------------------------------------

    public void StartTimer()
    {
        timerOn = true;
        timeLessThanThresh = false;
    }

    //---------------------------------------------

    public void StopTimer()
    {
        timerOn = false;
    }

    //---------------------------------------------

    public void ResetTimer()
    {
        timer = gameTime;
        TimeMoreThanThresh();
        SetText();
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
        currentUI.SetActive(true);
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
        beforeThreshold.SetActive(true);
        afterThreshold.SetActive(false);
        currentUI = beforeThreshold;
    }

    //---------------------------------------------

    void TimeLessThanThresh()
    {
        timeLessThanThresh = true;
        beforeThreshold.SetActive(false);
        afterThreshold.SetActive(true);
        currentUI = afterThreshold;
        BGM.instance.StartCoroutine(BGM.instance.PlayUrgentBGM());
    }

    //---------------------------------------------

    bool TimeIsOver()
    {
        return timer <= endTime;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior
    //---------------------------------------------

    private void Awake()
    {
        ResetTimer();
    }

    //---------------------------------------------
    
    void Update()
    {
        if (timerOn)
        {
            timer -= Time.deltaTime;
            SetText();
            if(timer <= thresholdTime&&!timeLessThanThresh)
            {
                TimeLessThanThresh();
            }
            if (TimeIsOver())
            {
                gameManager.GameDone();
            }
        }   
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------


   

}
