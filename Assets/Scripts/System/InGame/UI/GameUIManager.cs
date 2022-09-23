using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject scoopJoyStickUI;
    [SerializeField] InGameTimer gameTimer;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] PopUpAlarm popup;
    [SerializeField] Animator splashAnimation;
    public Canvas mainCanvas;
    public static GameUIManager instance;


    //---------------------------------------------
    #region Scoop Related UI Functions
    //--------------------------------------------- 

    public void GotScoop()
    {
        scoopJoyStickUI.SetActive(true);
    }

    //---------------------------------------------

    public void UsedScoop()
    {
        scoopJoyStickUI.SetActive(false);
    }

    //---------------------------------------------

    public void GotHit(bool isMine, Team team)
    {
        splashAnimation.SetTrigger("Splash");
        SplashSetter.splasher.SetSplash(team);
        popup.GotHit(isMine);
    }

    //---------------------------------------------

    public void HitSomeone()
    {
        popup.HitSomeone();
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region GameOverUI Functions
    //---------------------------------------------

    public void ShowGameOverScreen()
    {
        gameTimer.HideTimer();
        scoopJoyStickUI.SetActive(false);
        StartCoroutine(TurnOnGameOverPanel());
        //카메라 어떤 한 포인트로 움직이기

    }

    //---------------------------------------------

    IEnumerator TurnOnGameOverPanel()
    {
        yield return new WaitForSeconds(CameraWalk.cameraWalkTime);
        gameOverScreen.SetActive(true);
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Systemtic Functions
    //---------------------------------------------

    public void ResetUI()
    {
        scoopJoyStickUI.GetComponentInChildren<ScoopJoystickUISet>().SetUI();
        gameOverScreen.SetActive(false);
        gameTimer.ShowTimer();
        scoopJoyStickUI.SetActive(false);
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region private functions
    //---------------------------------------------

    void Singleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior functions
    //---------------------------------------------
    private void Awake()
    {
        Singleton();
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------
}