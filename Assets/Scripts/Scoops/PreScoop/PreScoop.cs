using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreScoop : MonoBehaviour
{
    int tileNumber;
    PreScoopParent parent;
    //---------------------------------------------
    #region PlaceFunctions
    //---------------------------------------------
    public void Place(int tileNum)
    {
        tileNumber = tileNum;
        Vector3 pos = Planet.instance.GetTile(tileNum).GetTilePoint();
        transform.position = pos;
        Planet.instance.Attract(transform, true);
    }

    public int GetCurrentTileNumber()
    {
        return tileNumber;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Eaten Functions
    //---------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null && player.photonView.IsMine)
        {
            player.GetPlayerScoop().GotScoop();
            parent.PrescoopEaten(this);
        }
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior
    //---------------------------------------------
    private void Start()
    {
        parent = GetComponentInParent<PreScoopParent>();
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------
}
