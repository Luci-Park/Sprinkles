using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;

public class ScoopInput : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static bool isBeingDragged;

    [SerializeField] RectTransform left;
    [SerializeField] RectTransform right;
    [SerializeField] RectTransform up;
    [SerializeField] RectTransform down;
    [SerializeField] GameObject leftFeedback;
    [SerializeField] GameObject rightFeedback;
    [SerializeField] GameObject upFeedback;
    [SerializeField] GameObject downFeedback;
    [SerializeField] RectTransform joystickTransform;

    public Direction input { get { return m_input; } }

    Direction m_input = Direction.none;
    Vector2 localDefaultPos;
    Vector2 worldSpaceDefaultPos;
    float xymaxDiff = 0.0001f;

    //float nonSelectOpacity = 0.25f;
    //float selectedOpacity = 0.75f;

    Player player;


    //---------------------------------------------
    #region Setup
    //---------------------------------------------
    
    void Setup()
    {
        upFeedback.SetActive(false);
        downFeedback.SetActive(false);
        leftFeedback.SetActive(false);
        rightFeedback.SetActive(false);
    }

    //---------------------------------------------
    
    public void SetPlayer()
    {
        if (Player.localPlayerInstance != null)
        {
            player = Player.localPlayerInstance.GetComponent<Player>();
        }
        else
        {
            Debug.Log("no Player local instance!");
        }
    }

    //---------------------------------------------

    public void InputReset()
    {
        m_input = Direction.none;
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior
    //---------------------------------------------
    private void Awake()
    {
        localDefaultPos = joystickTransform.localPosition;
        worldSpaceDefaultPos = joystickTransform.position;
    }

    //---------------------------------------------

    private void OnEnable()
    {
        Setup();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Get Input
    //---------------------------------------------

    public void OnDrag(PointerEventData eventData)
    {
        if (player.photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        OnTouch(eventData.position);
    }

    //---------------------------------------------

    public void OnPointerDown(PointerEventData eventData)
    {
        if (player.photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        isBeingDragged = true;
        OnTouch(eventData.position);
    }

    //---------------------------------------------

    public void OnPointerUp(PointerEventData eventData)
    {
        if (player.photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        isBeingDragged = false;
        CheckDirection();
        joystickTransform.localPosition = localDefaultPos;
        TurnOnFeedback();
    }

    //---------------------------------------------

    void OnTouch(Vector2 touchedPos)
    {
        if (player.photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        Vector2 resultVect = new(touchedPos.x - worldSpaceDefaultPos.x, touchedPos.y - worldSpaceDefaultPos.y);
        if (Mathf.Abs(resultVect.x) - Mathf.Abs(resultVect.y) > xymaxDiff)
        {
            resultVect.y = 0f;
        }
        else if (Mathf.Abs(resultVect.y) - Mathf.Abs(resultVect.x) > xymaxDiff)
        {
            resultVect.x = 0f;
        }
        else
        {
            resultVect = Vector2.ClampMagnitude(resultVect, xymaxDiff);
        }

        joystickTransform.localPosition = resultVect;
    }

    //---------------------------------------------

    void CheckDirection()
    {
        if (player.photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        m_input = Direction.none;
        if (joystickTransform.localPosition.x == 0)
        {
            if (joystickTransform.localPosition.y > 0)
            {
                if (joystickTransform.localPosition.y >= up.localPosition.y)
                {
                    m_input = Direction.up;
                }
            }
            else
            {
                if (joystickTransform.localPosition.y <= down.localPosition.y)
                {
                    m_input = Direction.down;
                }
            }
        }
        else
        {
            if (joystickTransform.localPosition.x > 0)
            {
                if (joystickTransform.localPosition.x >= right.localPosition.x)
                {
                    m_input = Direction.right;
                }
            }
            else
            {
                if (joystickTransform.localPosition.x <= left.localPosition.x)
                {
                    m_input = Direction.left;
                }
            }
        }
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region UI
    //---------------------------------------------
    void TurnOnFeedback()
    {
        switch (input)
        {
            case Direction.up:
                upFeedback.SetActive(true);
                break;
            case Direction.down:
                downFeedback.SetActive(true);
                break;
            case Direction.left:
                leftFeedback.SetActive(true);
                break;
            case Direction.right:
                rightFeedback.SetActive(true);
                break;
        }
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------
}
