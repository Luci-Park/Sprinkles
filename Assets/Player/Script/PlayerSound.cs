using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] AudioClip scoopThrownSound;
    [SerializeField] AudioClip gotHitSound;
    [SerializeField] AudioClip movingSound;
    [SerializeField] AudioClip gotScoopSound;
    [SerializeField] AudioClip turnSound;
    [SerializeField] AudioClip hitSuccessSound;
    AudioSource audioSource;

    public void PlayMoveSound()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.clip = movingSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void OnTurn()
    {
        audioSource.PlayOneShot(turnSound,1);
    }

    public void StopMoveSound()
    {
        audioSource.Stop();
    }

    public void PlayGotHitSound()
    {
        audioSource.PlayOneShot(gotHitSound, 1);
    }
    public void PlayGotScoopSound()
    {
        audioSource.PlayOneShot(gotScoopSound, 1);
    }
    public void PlayScoopThrownSound()
    {
        audioSource.PlayOneShot(scoopThrownSound, 1);
    }
    public void PlayOnHitSuccessSound()
    {
        //audioSource.PlayOneShot(hitSuccessSound, 1);
    }
    //---------------------------------------------
    #region MonoBehavior Functions
    //---------------------------------------------
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------
}
