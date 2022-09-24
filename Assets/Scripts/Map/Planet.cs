using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Planet : MonoBehaviour, IGameObserver
{
    public static readonly int[] planeNumbers = { 64, 128, 192, 256, 320, 384 };
    public static Planet instance;
   
    public float gravity = -12;
    public float rotSpeed = 10f;

    private int[] tilesTeamsCount = new int[(int)Team.none + 1];

    PhotonView photonView;
    int numberOfTiles;

    Tile[] TileChildren;

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
        return TileChildren[tileNumber];// transform.GetChild(tileNumber).GetComponent<Tile>();
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


    public void TileChanged(Team from, Team to, int tileIndex)
    {
        TileCount(from, to);
        RPC_SendChangeTileColorMesg(tileIndex, from, to);
    }

    //---------------------------------------------
    void TileCount(Team from, Team to)
    {
        tilesTeamsCount[(int)from] -= 1;
        tilesTeamsCount[(int)to] += 1;
    }
    //---------------------------------------------
    void ResetTileCount()
    {
        for(int i = 0; i< (int)Team.none; ++i)
        {
            tilesTeamsCount[i] = 0;
        }
        tilesTeamsCount[(int)Team.none] = numberOfTiles;
    }

    //---------------------------------------------

    public void CountTiles()
    {
        GameResultChecker.instance.StartCoroutine(GameResultChecker.instance.CheckResults(tilesTeamsCount));
    }

    //---------------------------------------------

    public void ResetTiles()
    {
        foreach (Tile tile in TileChildren)
        {
            tile.ChangeColorImmediately(Team.none);
        }
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region punrpcToChangeTiles
    //---------------------------------------------

    [PunRPC]
    void RPC_SendChangeTileColorMesg(int childIndex, Team from, Team to)
    {
        photonView.RPC("RPC_ReceiveTileChangeColorMesg", RpcTarget.OthersBuffered, childIndex, (int)from,(int)to);
    }

    //---------------------------------------------

    [PunRPC]
    void RPC_ReceiveTileChangeColorMesg(int childIndex, int from, int to)
    {
        transform.GetChild(childIndex).GetComponent<Tile>().ChangeColor((Team)to);
        TileCount((Team)from, (Team)to);
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
        numberOfTiles = transform.childCount;
        TileChildren = GetComponentsInChildren<Tile>();
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
        ResetTiles();
        ResetTileCount();
    }
    //---------------------------------------------
    public void NotifyGameStart()
    {
    }
    //---------------------------------------------
    public void NotifyGameOver()
    {
        CountTiles();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



}
