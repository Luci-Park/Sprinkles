using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWalk : MonoBehaviour
{
    public static float cameraWalkTime = 2.5f;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform planet;
    [SerializeField] float planetDistance;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip moveClip;

    //---------------------------------------------

    IEnumerator EndGameWalk()
    {
        audioSource.PlayOneShot(moveClip);
        Vector3 moveDirection = (transform.position - planet.position).normalized;
        while(Vector3.Distance(transform.position, planet.position) < planetDistance)
        {
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
            transform.LookAt(planet);
            yield return null; 
        }

    }

    //---------------------------------------------

    public void StartCameraWalk(Transform startPos)
    {
        transform.position = startPos.position;
        transform.rotation = startPos.rotation;
        StartCoroutine(EndGameWalk());
    }

    //---------------------------------------------
}

