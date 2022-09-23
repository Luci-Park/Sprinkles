using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
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
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region ETC.
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
