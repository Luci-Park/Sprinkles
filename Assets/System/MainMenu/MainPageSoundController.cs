using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPageSoundController : MonoBehaviour
{
    public AudioClip buttonClick;
    public AudioClip BGM;
    AudioSource audioSource;

    //---------------------------------------------
    #region Public Functions
    //---------------------------------------------

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClick, 1);
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Private Functions
    //---------------------------------------------

    void PlayBGM()
    {
        audioSource.clip = BGM;
        audioSource.Play();
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

    private void Start()
    {
        PlayBGM();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------
}
