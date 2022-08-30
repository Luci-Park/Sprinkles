using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] AudioClip bgm_usual;
    [SerializeField] AudioClip bgm_urgent;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip lose;
    [SerializeField] AudioClip scoreBeep;
    [SerializeField] AudioClip bgm_transition;
    AudioSource audioSource;

    public static BGM instance;
    //---------------------------------------------
    #region During Game BGM
    //---------------------------------------------

    public void PlayUsualBGM()
    {
        audioSource.clip = bgm_usual;
        audioSource.loop = true;
        audioSource.Play();
    }

    //---------------------------------------------

    public IEnumerator PlayUrgentBGM()
    {
        audioSource.clip = bgm_transition;
        audioSource.Play();
        yield return new WaitForSeconds(2.0f);
        audioSource.clip = bgm_urgent;
        audioSource.loop = true;
        audioSource.Play();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region At End Game BGM
    //---------------------------------------------

    public void PlayWin()
    {
        audioSource.clip = win;
        audioSource.loop = false;
        audioSource.Play();
    }

    //---------------------------------------------

    public void PlayLose()
    {
        audioSource.clip = lose;
        audioSource.loop = false;
        audioSource.Play();
    }

    //---------------------------------------------

    public void PlayScoreCount()
    {
        audioSource.PlayOneShot(scoreBeep, 1);
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior functions
    //---------------------------------------------
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Singleton();
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region ETC.
    //---------------------------------------------

    public void StopPlaying()
    {
        audioSource.Stop();
    }
    
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
