using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCountDown : MonoBehaviour
{
    [SerializeField] GameObject[] countDown;
    [SerializeField] AudioClip[] countSound;

    AudioSource audioSource;

    Coroutine countDownRoutine;


    //---------------------------------------------
    #region Public Functions
    //---------------------------------------------

    public IEnumerator CountDown()
    {
        yield return new WaitForSeconds(5f);
        countDownRoutine = StartCoroutine(CountDown());
        for (int i = 0; i < countDown.Length; i++)
        {
            TurnOnOne(i);
            if (countSound[i] != null)
            {
                audioSource.PlayOneShot(countSound[i]);
            }
            yield return new WaitForSeconds(1f);
        }
        LevelManager.instance.LeaveGame();
    }

    //---------------------------------------------

    public void StopTimer()
    {
        if(countDownRoutine != null)
            StopCoroutine(countDownRoutine);
        TurnAllOff();

    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Private Functions
    //---------------------------------------------

    void TurnOnOne(int index)
    {
        for (int i = 0; i < countDown.Length; i++)
        {
            if (i == index)
            {
                countDown[i].SetActive(true);
            }
            else
            {
                countDown[i].SetActive(false);
            }
        }
    }

    //---------------------------------------------

    void TurnAllOff()
    {
        for(int i = 0; i<countDown.Length; i++)
        {
            countDown[i].SetActive(false);
        }
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
