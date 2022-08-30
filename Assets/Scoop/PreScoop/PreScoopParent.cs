using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreScoopParent : MonoBehaviour
{
    public static PreScoopParent instance;
    float preScoopRespawnTime = 5f;

    int[] planePlacedSituation = new int[6];
    int[] placedTile;

    PlayerMovement[] listOfPlayers;

    //---------------------------------------------
    #region Public Functions
    //---------------------------------------------

    public void PrescoopEaten(PreScoop scoop)
    {
        StartCoroutine(RespawnPrescoop(scoop));
    }

    //---------------------------------------------

    public void PlaceAllPreScoop()
    {
        EraseAllPlacementInfo();
        foreach (PreScoop preScoop in GetComponentsInChildren<PreScoop>())
        {
            PlacePreScoop(preScoop);
        }
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Private Functions
    //---------------------------------------------

    private void EraseAllPlacementInfo()
    {
        for (int i = 0; i < placedTile.Length; i++)
        {
            placedTile[i] = 0;
        }
        for (int i = 0; i < 6; i++)
        {
            planePlacedSituation[i] = 0;
        }
    }

    //---------------------------------------------

    IEnumerator RespawnPrescoop(PreScoop scoop)
    {
        int beforeTile = scoop.GetCurrentTileNumber();
        scoop.gameObject.SetActive(false);
        planePlacedSituation[Planet.PlaneNumber(beforeTile)]--;
        yield return new WaitForSeconds(preScoopRespawnTime);
        PlacePreScoop(scoop);
    }

    //---------------------------------------------

    void PlacePreScoop(PreScoop preScoop)
    {
        int tile = GetRandomTile();
        placedTile[preScoop.transform.GetSiblingIndex()] = tile;
        preScoop.Place(tile);
        preScoop.gameObject.SetActive(true);
    }

    //---------------------------------------------

    int GetRandomTile()
    {
        int plane;
        do
        {
            plane = Random.Range(0, 6);
        } while (planePlacedSituation[plane] >= 2);
        planePlacedSituation[plane]++;
        int min = plane == 0 ? 0 : Planet.planeNumbers[plane - 1];
        int max = Planet.planeNumbers[plane];
        int tile;
        do
        {
            tile = Random.Range(min, max);
        } while (IsTileOccupied(tile));
        return tile;
    }

    //---------------------------------------------

    bool IsTileOccupied(int num)
    {
        foreach (int tile in placedTile)
        {
            if (num == tile)
                return true;
        }
        foreach (PlayerMovement player in listOfPlayers)
        {
            if (player.IsOnTile(num))
                return true;
        }
        return false;
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
        placedTile = new int[transform.childCount];
        listOfPlayers = FindObjectsOfType<PlayerMovement>();
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
