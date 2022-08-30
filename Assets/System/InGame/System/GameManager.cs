using Photon.Pun;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameUIManager uIManager;
    [SerializeField] GameObject scoopParent;
    [SerializeField] GameObject preScoopParent;
    [SerializeField] InGameTimer gameTimer;
    [SerializeField] BeginningCountDown countDown;
    [SerializeField] ScoopInput scoopInput;

    [SerializeField] CameraWalk endCamera;

    static int[] playeroccuPiedTiles = new int[6];
    static int[] numberOfTeamMembers = new int[(int)Team.none];
    Player player;

    public static bool isPlaying = false;
    public static GameManager instance;

    //---------------------------------------------
    #region Game Status Preparation
    //---------------------------------------------
    
    void Awake()
    {
        Singleton();
        SpawnPlayer();
        StartCoroutine(PrepAndStart());
    }

    //---------------------------------------------

    [PunRPC]
    void RPC_Reset()
    {
        StartCoroutine(PrepAndStart());
    }
    
    //---------------------------------------------

    IEnumerator PrepAndStart()
    {
        PrepareGame();
        yield return StartCoroutine(countDown.CountDown());
        StartGame();

    }

    //---------------------------------------------

    void PrepareGame()
    {
        endCamera.gameObject.SetActive(false);
        Arena.instance.ResetPostion();
        Planet.instance.ResetTiles();
        int plane = GetRandomPlayerTile();
        player.SetStartingPoint(Planet.instance.GetTile(plane));
        uIManager.ResetUI();
        PreScoopParent.instance.PlaceAllPreScoop();
        gameTimer.ResetTimer();
    }

    //---------------------------------------------

    public void StartGame()
    {
        isPlaying = true;
        player.StartGame();
        gameTimer.StartTimer();
        BGM.instance.PlayUsualBGM();
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
        }
    }
    //---------------------------------------------
    int GetRandomPlayerTile()
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
        isPlaying = false;
        uIManager.ShowGameOverScreen();
        gameTimer.StopTimer();
        player.GameOver();
        foreach(Scoop scoop in scoopParent.transform.GetComponentsInChildren<Scoop>())
        {
            PhotonNetwork.Destroy(scoop.gameObject);
        }
        Planet.instance.CountTiles();
        Arena.endRotate = true;
        endCamera.gameObject.SetActive(true);
        endCamera.StartCameraWalk(player.GiveCameraPos());
    }

    //---------------------------------------------

    public void Reset()
    {
        //LevelManager.levelManager.StartGame();
        StartCoroutine(PrepAndStart());
        photonView.RPC("RPC_Reset", RpcTarget.Others);
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region private functions
    //---------------------------------------------

    void Singleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------
}