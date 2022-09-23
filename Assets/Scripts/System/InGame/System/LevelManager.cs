using Photon.Pun;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourPunCallbacks
{
    readonly string mainMenuScene = "Main menu";
    readonly string playScene = "PlayScene";

    public static LevelManager instance;

    //---------------------------------------------
    #region public functions
    //---------------------------------------------
    public void StartGame()
    {
        LoadNetworkScene(playScene);
    }

    //---------------------------------------------

    public void LeaveGame()
    {
        NetworkManager.instance.LeaveNetworkRoom();
        LoadLocalScene(mainMenuScene);
    }
    //---------------------------------------------
    public void LeaveRoom()
    {
        if (SceneManager.GetActiveScene().name != mainMenuScene)
            LoadLocalScene(mainMenuScene);
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region private functions
    //---------------------------------------------
    void LoadNetworkScene(string sceneName)
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(sceneName);
    }

    //---------------------------------------------

    void LoadLocalScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //---------------------------------------------

    void Singleton()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior functions
    //---------------------------------------------
    private void Awake()
    {
        Singleton();
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------
}
