using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningCountDown : MonoBehaviour
{
    [SerializeField] GameObject[] countDown;
    [SerializeField] AudioClip[] countSound;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartCountDown()
    {
        StartCoroutine(CountDown());
    }

    public IEnumerator CountDown()
    {
        for(int i = 0; i<countDown.Length; i++)
        {
            TurnOnOne(i);
            if(countSound[i] != null)
            {
                audioSource.PlayOneShot(countSound[i]);
            }
            yield return new WaitForSeconds(0.5f);
        }

        countDown[3].SetActive(false);
    }

    void TurnOnOne(int index)
    {
        for(int i = 0; i<4; i++)
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

}
