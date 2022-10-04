using System.Collections;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;

//public enum PlayerReadyState { NotReady, ReadyToStart, ReadyToEnd};

public class Player : MonoBehaviourPun, IGameObserver
{
    public static GameObject localPlayerInstance;

    public Team myTeam = Team.mint;

    //static string statusKey = "Status";

    [SerializeField] Transform modelingPosition;
    //[SerializeField] GameObject shadowModel;
    [SerializeField] Transform gameCamera;

    PlayerMovement movement;
    PlayerScoopAction scoopAction;
    PlayerSound sound;

    Animator animator;
    GameObject modeling;
    //PlayerReadyState readyState = PlayerReadyState.NotReady;

    //---------------------------------------------
    #region Modeling Set Functions
    //---------------------------------------------

    void SetModeling()
    {
        SetPlayerModeling();
        SetScoopModeling();
    }

    //---------------------------------------------

    void SetPlayerModeling()
    {
        if (modeling != null)
        {
            Destroy(modeling);
        }
        modeling = Instantiate(ModelingSetter.instance.GetPlayerModeling(myTeam), modelingPosition);
    }

    //---------------------------------------------

    void SetScoopModeling()
    {
        scoopAction.SetScoopPrefab(ModelingSetter.instance.GetScoopModeling(myTeam));
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Component Get Functions
    //---------------------------------------------

    public Animator GetAnimator()
    {
        return animator;
    }

    //---------------------------------------------

    public PlayerMovement GetMovement()
    {
        return movement;
    }

    //---------------------------------------------

    public PlayerScoopAction GetPlayerScoop()
    {
        return scoopAction;
    }

    //---------------------------------------------

    public PlayerSound GetPlayerSound()
    {
        return sound;
    }

    //---------------------------------------------

    public Transform GiveCameraPos()
    {
        return gameCamera;
    }

    //---------------------------------------------

    public bool IsTeam(Team team)
    {
        return myTeam == team;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Setting Up Player For a New Game
    //---------------------------------------------

    public void Reset()
    {
        scoopAction.Reset();
        movement.Reset();
    }

    //---------------------------------------------

    public void StartGame()
    {
        movement.StartMove();
    }

    //---------------------------------------------

    public void GameOver()
    {
        movement.CompleteStop();
        scoopAction.Reset();
        //SetReadyState(PlayerReadyState.ReadyToEnd);
    }

    //---------------------------------------------

    public void GamePrep(ScoopInput scoop, Transform scoopParent)
    {
        scoopAction.SetScoopInput(scoop).SetScoopParent(scoopParent);
        scoop.SetPlayer();
    }

    //---------------------------------------------

    public void SetStartingPoint(Tile startTile)
    {
        movement.SetStartingPoint(startTile);
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------
        


    //---------------------------------------------
    #region ModelSet
    //---------------------------------------------

    void SetMyModel()
    {
        localPlayerInstance = gameObject;
        gameCamera.gameObject.SetActive(true);
        gameObject.name += "mine";

    }

    //---------------------------------------------

    void SetOtherModel()
    {
        movement.enabled = false;
        gameCamera.gameObject.SetActive(false);
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region NetworkTeamSet
    //---------------------------------------------
    [PunRPC]
    void RPC_GetTeam()
    {
        myTeam = GameManager.instance.GetPlayerTeam();
        photonView.RPC("RPC_UpdateTeam", RpcTarget.OthersBuffered, myTeam);
        SetModeling();
    }

    //---------------------------------------------

    [PunRPC]
    void RPC_UpdateTeam(Team newTeam)
    {
        myTeam = newTeam;
        SetModeling();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Interaction Functions
    //---------------------------------------------

    public void SuccessfulHit()
    {
        GameUIManager.instance.HitSomeone();
        sound.PlayOnHitSuccessSound();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior Functions
    //---------------------------------------------
    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        scoopAction = GetComponent<PlayerScoopAction>();
        sound = GetComponent<PlayerSound>();
        animator = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            SetMyModel();
        }
        else
        {
            SetOtherModel();
        }
        GameManager.instance.AddObserver(this);
    }

    //---------------------------------------------

    private void Start()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }
        //SetReadyState(PlayerReadyState.ReadyToStart);
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------

    /*
    void SetReadyState(PlayerReadyState ready)
    {
        readyState = ready;
        Hashtable playerState = new Hashtable() { { GameManager.PLAYER_STATE, (int)readyState } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerState);
    }
    */

    //---------------------------------------------
    #region Observer Functions
    //---------------------------------------------
    public void NotifyPreparation()
    {
        int plane = GameManager.instance.GetRandomPlayerTile();
        SetStartingPoint(Planet.instance.GetTile(plane));
    }
    //---------------------------------------------
    public void NotifyGameStart()
    {
        StartGame();
    }
    //---------------------------------------------
    public void NotifyGameOver()
    {
        GameOver();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------

}
