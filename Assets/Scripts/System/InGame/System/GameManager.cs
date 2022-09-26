using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public const string PLAYER_STATE = "PlayerState";

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject scoopParent;
    [SerializeField] ScoopInput scoopInput;
    [SerializeField] BeginningCountDown beginningCountDown;
    [SerializeField] EndCountDown endCountDown;
    [SerializeField] CameraWalk endCamera;

    public static GameManager instance;

    static int[] playeroccuPiedTiles = new int[6];
    static int[] numberOfTeamMembers = new int[(int)Team.none];

    Player player;
    PlayerReadyState waitingFor = PlayerReadyState.ReadyToStart;

    List<IGameObserver> observers = new List<IGameObserver>();

    Photon.Realtime.LoadBalancingClient balancingClient = new Photon.Realtime.LoadBalancingClient();
    Photon.Realtime.Player[] players;

    //---------------------------------------------
    #region ObserverSetters
    //---------------------------------------------
    public void AddObserver(IGameObserver observer)
    {
        if (!observers.Contains(observer)) observers.Add(observer);
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Game Status Preparation
    //---------------------------------------------

    void Awake()
    {
        Singleton();
        SpawnPlayer();
    }

    //---------------------------------------------
    private void Start()
    {
        PrepareGame();
        players = PhotonNetwork.PlayerList;//Photon
    }
    //---------------------------------------------


    [PunRPC]
    void RPC_Reset()
    {
        PrepareGame();
    }

    //---------------------------------------------

    public void PrepareGame()
    {
        endCamera.gameObject.SetActive(false);
        NotifyGamePrep();
        waitingFor = PlayerReadyState.ReadyToStart;
    }

    //---------------------------------------------

    public void StartGame()
    {
        NotifyGameStart();
        BGM.instance.PlayUsualBGM();
    }
    //---------------------------------------------
    private void NotifyGamePrep()
    {
        foreach (IGameObserver observer in observers)
        {
            observer.NotifyPreparation();
        }
    }
    //---------------------------------------------
    private void NotifyGameStart()
    {
        foreach (IGameObserver observer in observers)
        {
            observer.NotifyGameStart();
        }
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------


    //---------------------------------------------
    #region PlayerPreperation
    //---------------------------------------------
    void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0).GetComponent<Player>();
            player.transform.parent = Arena.instance.transform;
            player.GamePrep(scoopInput, scoopParent.transform);
            int plane = GetRandomPlayerTile();
            player.SetStartingPoint(Planet.instance.GetTile(plane));
        }
    }
    //---------------------------------------------
    public int GetRandomPlayerTile()
    {
        int plane;
        do {
            plane = Random.Range(0, 6);
        }while (playeroccuPiedTiles[plane] != 0) ;
        playeroccuPiedTiles[plane] = 1;
        int min = plane == 0 ? 0 : Planet.planeNumbers[plane - 1];
        int max = Planet.planeNumbers[plane];
        plane = Random.Range(min, max);
        
        return plane;        
    }
    //---------------------------------------------

    public Team GetPlayerTeam()
    {
        int team;
        do
        {
            team = Random.Range(0, (int)Team.none);
        } while (numberOfTeamMembers[team] > 0);;
        numberOfTeamMembers[team]++;
        return (Team)team;
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region On Game End Functions
    //---------------------------------------------
    public void GameDone()
    {
        waitingFor = PlayerReadyState.ReadyToEnd;
        NotifyGameDone();
        DestroyAllGameObjects();
        EndCameraWalk();
    }

    //---------------------------------------------

    public void Reset()
    {
        //LevelManager.levelManager.StartGame();
        PrepareGame();
        photonView.RPC("RPC_Reset", RpcTarget.Others);
    }
    //---------------------------------------------
    private void DestroyAllGameObjects()
    {
        foreach (Scoop scoop in scoopParent.transform.GetComponentsInChildren<Scoop>())
        {
            PhotonNetwork.Destroy(scoop.gameObject);
        }
    }
    //---------------------------------------------
    private void NotifyGameDone()
    {
        foreach(IGameObserver observer in observers)
        {
            observer.NotifyGameOver();
        }
    }
    //---------------------------------------------
    private void EndCameraWalk()
    {
        endCamera.gameObject.SetActive(true);
        endCamera.StartCameraWalk(player.GiveCameraPos());
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable state)
    {
        object playerState;
        if (state.TryGetValue(PLAYER_STATE, out playerState) && (PlayerReadyState)playerState == waitingFor)
        {
            CheckPlayersReady(waitingFor);
        }
    }

    void CheckPlayersReady(PlayerReadyState state)
    {
        foreach (Photon.Realtime.Player p in players)
        {
            object playerState;
            if (p.CustomProperties.TryGetValue(PLAYER_STATE, out playerState) && (PlayerReadyState)playerState != state)
            {
                return;
            }
        }
        if (PlayerReadyState.ReadyToStart == state) {
            beginningCountDown.StartCountDown();
            //startTimer();//StartGame();
        }
        else if (PlayerReadyState.ReadyToEnd == state) {
            endCountDown.StartCountDown();
            //startEndTimer();
        }
    }

    //---------------------------------------------
    #region private functions
    //---------------------------------------------

    void Singleton()
    {
        instance = this;
    }


    private void FixedUpdate()
    {
        balancingClient.Service();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------
}