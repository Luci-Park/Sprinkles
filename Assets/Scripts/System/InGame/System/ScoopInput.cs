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
    Vector2 defaultInputPos;
    Vector2 mouseStartPos;
    float xymaxDiff = 35f;

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
        defaultInputPos = joystickTransform.anchoredPosition;
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
        mouseStartPos = eventData.position;
        OnTouch(eventData.position);
    }

    //---------------------------------------------

    public void OnPointerUp(PointerEventData eventData)
    {
        if (player.photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        CheckDirection();
        joystickTransform.anchoredPosition = defaultInputPos;
        TurnOnFeedback();
    }

    //---------------------------------------------

    void OnTouch(Vector2 touchedPos)
    {
        joystickTransform.position = touchedPos;
        Vector2 clampPosition = joystickTransform.anchoredPosition;
        float diff = Mathf.Abs(touchedPos.x - mouseStartPos.x) - Mathf.Abs(touchedPos.y - mouseStartPos.y);
        if (diff > xymaxDiff)
        {
            clampPosition.y = defaultInputPos.y;
        }
        else if (diff < xymaxDiff)
        {
            clampPosition.x = defaultInputPos.x;
        }
        joystickTransform.anchoredPosition = clampPosition;    
    }

    //---------------------------------------------

    void CheckDirection()
    {
        if (player.photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        m_input = Direction.none;
        Vector2 joystickPos = joystickTransform.anchoredPosition;
        if (Mathf.Abs(Mathf.Abs(joystickPos.x) - Mathf.Abs(joystickPos.y)) < xymaxDiff) return;

        if (joystickPos.x == defaultInputPos.x)
        {
            if (joystickPos.y > defaultInputPos.y)
            {
                    m_input = Direction.up;
            }
            else
            {
                    m_input = Direction.down;
            }
        }
        else
        {
            if (joystickPos.x > defaultInputPos.x)
            {
                m_input = Direction.right;
            }
            else
            {
                m_input = Direction.left;
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
