using UnityEngine.EventSystems;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    public float minSwipeLength = 200f;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    public Direction8 input { get { return m_input; } }
    private Direction8 m_input = Direction8.none;
    bool onUI;

    //---------------------------------------------
    #region MonoBehavior
    //---------------------------------------------
    void Update()
    {
        if(ScoopInput.isBeingDragged || photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        Direction8 dir = DetectSwipe();
        if (dir == Direction8.none)
        {
            dir = DetectButtonPress();
        }
        if (dir != Direction8.none)
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
        m_input = Direction8.none;
    }

    //---------------------------------------------

    public Direction8 DetectButtonPress()
    {
        Direction8 dir;
        if (Input.GetKeyDown(KeyCode.W))
        {
            dir = Direction8.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            dir = Direction8.down;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            dir = Direction8.right;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            dir = Direction8.left;
        }
        else {
            dir = Direction8.none;
        }
        return dir;
    }

    //---------------------------------------------

    public Direction8 DetectSwipe()
    {
        Direction8 dir = Direction8.none;
        if (Input.touches.Length > 0) {
             Touch t = Input.GetTouch(0);
 
             if (t.phase == TouchPhase.Began) {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    onUI = true;
                }
                else
                {
                    onUI = false;
                }
                    firstPressPos = new Vector2(t.position.x, t.position.y);
             }

             if (t.phase == TouchPhase.Ended && !onUI) {
                secondPressPos = new Vector2(t.position.x, t.position.y);
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                // Make sure it was a legit swipe, not a tap
                if (currentSwipe.magnitude < minSwipeLength) {
                    dir = Direction8.none;
                    return dir;
                }
           
                currentSwipe.Normalize();

                // Swipe up
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
                    dir = Direction8.up;
                // Swipe down
                } else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
                    dir = Direction8.down;
                // Swipe left
                } else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
                    dir = Direction8.left;
                // Swipe right
                } else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
                    dir = Direction8.right;
                }
            }
        }
        return dir;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------
}
