using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    public float minSwipeLength = 200f;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    public Direction input { get { return m_input; } }
    private Direction m_input = Direction.none;

    //---------------------------------------------
    #region MonoBehavior
    //---------------------------------------------
    void Update()
    {
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (ScoopInput.isBeingDragged) return;
        Direction dir = DetectSwipe();
        if (dir == Direction.none)
        {
            dir = DetectButtonPress();
        }
        if (dir != Direction.none)
        {
            m_input = dir;
        }
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Get Input
    //---------------------------------------------
    
    public void DirReset()
    {
        m_input = Direction.none;
    }

    //---------------------------------------------

    public Direction DetectButtonPress()
    {
        Direction dir;
        if (Input.GetKeyDown(KeyCode.W))
        {
            dir = Direction.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            dir = Direction.down;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            dir = Direction.right;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            dir = Direction.left;
        }
        else {
            dir = Direction.none;
        }
        return dir;
    }

    //---------------------------------------------

    public Direction DetectSwipe()
    {
        Direction dir = Direction.none;
        if (Input.touches.Length > 0) {
             Touch t = Input.GetTouch(0);
 
             if (t.phase == TouchPhase.Began) {
                 firstPressPos = new Vector2(t.position.x, t.position.y);
             }
 
             if (t.phase == TouchPhase.Ended) {
                secondPressPos = new Vector2(t.position.x, t.position.y);
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                // Make sure it was a legit swipe, not a tap
                if (currentSwipe.magnitude < minSwipeLength) {
                    dir = Direction.none;
                    Debug.Log("just a tap");
                    return dir;
                }
           
                currentSwipe.Normalize();

                // Swipe up
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
                    dir = Direction.up;
                // Swipe down
                } else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
                    dir = Direction.down;
                // Swipe left
                } else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
                    dir = Direction.left;
                // Swipe right
                } else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
                    dir = Direction.right;
                }
            }
        }
        return dir;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------
}
