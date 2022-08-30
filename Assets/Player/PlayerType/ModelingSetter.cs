using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelingSetter : MonoBehaviour
{
    [Tooltip("strawberry, mint")]
    [SerializeField] GameObject[] playerModeling;
    [SerializeField] GameObject[] scoopModeling;

    public static ModelingSetter instance;

    //---------------------------------------------
    #region public functions
    //---------------------------------------------

    public GameObject GetPlayerModeling(Team team)
    {
        return playerModeling[(int)team];
    }

    //---------------------------------------------

    public GameObject GetScoopModeling(Team team)
    {
        return scoopModeling[(int)team];
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
