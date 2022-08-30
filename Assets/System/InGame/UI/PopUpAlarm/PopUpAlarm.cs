using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpAlarm : MonoBehaviour
{

    Animator animator;
    
    //---------------------------------------------
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //---------------------------------------------

    public void HitSomeone()
    {
        animator.SetTrigger("attackSuccess");
    }

    //---------------------------------------------

    public void GotHit(bool isMine)
    {
        if (isMine)
        {
            animator.SetTrigger("attackedMyself");
        }
        else
        {
            animator.SetTrigger("attackByOther");

        }
    }
    //---------------------------------------------
}
