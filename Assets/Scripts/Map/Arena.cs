using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour, IGameObserver
{
    public float endRotateSpeed = 0.3f;
    public static bool endRotate = false;
    public static Arena instance;

    //---------------------------------------------
    #region Rotate On Game End
    //---------------------------------------------

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (endRotate)
        {
            transform.RotateAround(transform.up, endRotateSpeed * Time.deltaTime);
        }
    }

    //---------------------------------------------

    public void ResetPostion()
    {
        endRotate = false;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
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
    private void Start()
    {
        GameManager.instance.AddObserver(this);
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region ETC.
    //---------------------------------------------

    void Singleton()
    {
        instance = this;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Observer Functions
    //---------------------------------------------
    public void NotifyPreparation()
    {
        ResetPostion();
    }
    //---------------------------------------------
    public void NotifyGameStart()
    {
    }
    //---------------------------------------------
    public void NotifyGameOver()
    {
        endRotate = true;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------

}
