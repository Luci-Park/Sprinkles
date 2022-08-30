using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Planet : MonoBehaviour
{
    public static readonly int[] planeNumbers = { 64, 128, 192, 256, 320, 384 };
    public static Planet instance;
   
    public float gravity = -12;
    public float rotSpeed = 10f;

    private int[] tilesTeamsCount = new int[(int)Team.none];

    PhotonView photonView;


    //---------------------------------------------
    #region Gravity Functions
    //---------------------------------------------

    public void Attract(Transform targetTransform, bool isStatic = false)
    {
        Vector3 gravityUp = CheckDirection(targetTransform);
        Vector3 localUp = targetTransform.up;
        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * targetTransform.rotation;
        if (!isStatic)
        {
            targetTransform.rotation = Quaternion.RotateTowards(targetTransform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
        else
        {
            targetTransform.rotation = targetRotation;
        }
    }

    //---------------------------------------------

    Vector3 CheckDirection(Transform targetTransform)
    {
        RaycastHit hit;
        if (Physics.Raycast(targetTransform.position, (transform.position - targetTransform.position).normalized, out hit))
        {
            Debug.DrawRay(targetTransform.position, hit.normal, Color.yellow);
            return hit.normal;
        }

        return Vector3.zero;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Tile Functions
    //---------------------------------------------

    public Tile GetTile(int tileNumber)
    {
        return transform.GetChild(tileNumber).GetComponent<Tile>();
    }

    //---------------------------------------------

    public static int PlaneNumber(int tileNumber)
    {
        if (tileNumber < planeNumbers[0])
        {
            return 0;
        }
        else if (tileNumber >= planeNumbers[0] && tileNumber < planeNumbers[1])
        {
            return 1;
        }
        else if (tileNumber >= planeNumbers[1] && tileNumber < planeNumbers[2])
        {
            return 2;
        }
        else if (tileNumber >= planeNumbers[2] && tileNumber < planeNumbers[3])
        {
            return 3;
        }
        else if (tileNumber >= planeNumbers[3] && tileNumber < planeNumbers[4])
        {
            return 4;
        }
        return 5;
    }

    //---------------------------------------------

    public void CountTiles()
    {
        Array.Clear(tilesTeamsCount, 0, tilesTeamsCount.Length);
        foreach (Tile tile in GetComponentsInChildren<Tile>())
        {
            if (tile.GetTileTeam() == Team.none) continue;
            tilesTeamsCount[(int)tile.GetTileTeam()]++;
        }
        GameResultChecker.instance.StartCoroutine(GameResultChecker.instance.CheckResults(tilesTeamsCount));
    }

    //---------------------------------------------

    public void ResetTiles()
    {
        foreach (Tile tile in GetComponentsInChildren<Tile>())
        {
            tile.ChangeColor(Team.none);
        }
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region punrpcToChangeTiles
    //---------------------------------------------

    [PunRPC]
    public void RPC_SendChangeTileColorMesg(int childIndex, Team team)
    {
        photonView.RPC("RPC_ReceiveTileChangeColorMesg", RpcTarget.OthersBuffered, childIndex, team);
    }

    //---------------------------------------------

    [PunRPC]
    void RPC_ReceiveTileChangeColorMesg(int childIndex, Team team)
    {
        transform.GetChild(childIndex).GetComponent<Tile>().ChangeColor(team);
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
        photonView = GetComponent<PhotonView>();
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
        DontDestroyOnLoad(gameObject);
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



}
