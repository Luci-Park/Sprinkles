using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultChecker : MonoBehaviour
{
    public static GameResultChecker instance;

    [SerializeField] Image [] gauge;
    [SerializeField] GameObject win;
    [SerializeField] GameObject lose;
    [SerializeField] EndCountDown endCountDown;

    float gaugeFillTime = 0.3f;

    //---------------------------------------------
    #region Result Check
    //---------------------------------------------
    public IEnumerator CheckResults(int[] teamscore)
    {
        yield return new WaitForSeconds(CameraWalk.cameraWalkTime);

        int denominator = teamscore[(int)Team.strawberry] + teamscore[(int)Team.mint];
        bool won = teamscore[(int)Team.strawberry] > teamscore[(int)Team.mint];
        if (Player.localPlayerInstance.GetComponent<Player>().myTeam != Team.strawberry)
        {
            won = !won;
        }
        StartCoroutine(ChangeGauge(0, denominator, teamscore[(int)Team.strawberry]));
        StartCoroutine(ChangeGauge(1, denominator, teamscore[(int)Team.mint]));

        
        if (won)
        {
            BGM.instance.PlayWin();
            win.SetActive(true);
            lose.SetActive(false);
        }
        else
        {
            BGM.instance.PlayLose();
            win.SetActive(false);
            lose.SetActive(true);
        }

        yield return new WaitForSeconds(3f);
        endCountDown.StartCountDown();
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region UI functions
    //---------------------------------------------

    public void ChangeGuage(int team, int denominator, int numberOfTiles)
    {
        gauge[team].fillAmount = numberOfTiles / denominator;
    }

    //---------------------------------------------

    IEnumerator ChangeGauge(int team, float denominator, int numberOfTiles)
    {
        gauge[team].fillAmount = 0f;
        float fillPercent = numberOfTiles / denominator;
        float step = fillPercent / gaugeFillTime * Time.deltaTime;

        BGM.instance.PlayScoreCount();
        while (gauge[team].fillAmount < fillPercent)
        {
            gauge[team].fillAmount += step;
            yield return null;
        }
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



    //---------------------------------------------
    #region ETC.
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

}
