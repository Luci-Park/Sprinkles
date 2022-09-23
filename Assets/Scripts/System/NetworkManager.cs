using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField] private byte maxPlayersPerRoom = 2;

    string gameVersion = "1";

    bool isConnecting;

    delegate void ConnectingFunction();

    ConnectingFunction connection;
    bool isSoloTest = false;
    //---------------------------------------------
    #region Public Functions
    //---------------------------------------------
   
    public void RandomRoomConnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connection = new ConnectingFunction(RandomRoomConnect);
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    
    //---------------------------------------------

    public void CreateRoomWithCode()
    {
        if (PhotonNetwork.IsConnected)
        {
            string code = Matchmaker.CreateCode();
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = false;
            roomOptions.MaxPlayers = maxPlayersPerRoom;
            if (PhotonNetwork.CreateRoom(code, roomOptions))
            {
                MainPageUIManager.instance.OnCodeCreationConfirmed(code);
            }
        }
        else
        {
            connection = new ConnectingFunction(CreateRoomWithCode);
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    //---------------------------------------------
    public void JoinRoomWithCode()
    {
        if (PhotonNetwork.IsConnected)
        {
            string code = MainPageUIManager.instance.GetCode();
            if (string.IsNullOrEmpty(code))
            {
                MainPageUIManager.instance.WrongCode();
            }
            else if (PhotonNetwork.JoinRoom(code))
            {
                MainPageUIManager.instance.TurnOnPage(PageTypes.Loading);
            }
        }
        else
        {
            connection = new ConnectingFunction(JoinRoomWithCode);
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    //---------------------------------------------

    public void LeaveNetworkRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Private Functions
    //---------------------------------------------
   
    void CheckIfGameStarts()
    {
        if (isSoloTest == true || PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom)
        {

            PhotonNetwork.CurrentRoom.IsOpen = false;
            LevelManager.instance.StartGame();
        }
    }

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


    //---------------------------------------------
    #region Photon Override Functions
    //---------------------------------------------
    public override void OnConnectedToMaster()
    {
        // we don't want to do anything if we are not attempting to join a room.
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if (isConnecting)
        {
            if (connection != null)
            {
                connection();
            }
            isConnecting = false;
        }
    }

    //---------------------------------------------

    public override void OnDisconnected(DisconnectCause cause)
    {

    }

    //---------------------------------------------

    public override void OnLeftRoom()
    {
        LevelManager.instance.LeaveRoom();
    }
    
    //---------------------------------------------
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }
    
    //---------------------------------------------
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoomWithCode();
    }
    
    //---------------------------------------------
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        MainPageUIManager.instance.WrongCode();
    }
    
    //---------------------------------------------
    
    public override void OnJoinedRoom()
    {
        CheckIfGameStarts();
    }
    
    //---------------------------------------------
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
    {
        CheckIfGameStarts();
    } 
    
    //---------------------------------------------

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        LevelManager.instance.LeaveGame();
    }
    
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior
    //---------------------------------------------
    private void Awake()
    {
        Singleton();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Test Functions, to be erased
    //---------------------------------------------

    public void SoloTest()
    {
        isSoloTest = true;
        RandomRoomConnect();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------
}
