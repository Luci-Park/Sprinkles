using System.Collections;
using UnityEngine;

using Photon.Pun;
public class PlayerScoopAction : MonoBehaviourPun
{
    Player player;
    
    ScoopInput scoopInput;
    Transform scoopParent;

    GameObject scoopPrefab;
    bool hasScoop;

    //---------------------------------------------
    #region Setting Functions
    //---------------------------------------------

    public PlayerScoopAction SetScoopPrefab(GameObject prefab)
    {
        scoopPrefab = prefab;
        return this;
    }

    //---------------------------------------------

    public PlayerScoopAction SetScoopInput(ScoopInput input)
    {
        scoopInput = input;
        return this;
    }

    //---------------------------------------------

    public PlayerScoopAction SetScoopParent(Transform parent)
    {
        scoopParent = parent;
        return this;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region GetHit
    //---------------------------------------------

    [PunRPC]
    public void OnHit(Team team)
    {
        if (!photonView.IsMine)
        {
            photonView.RPC("OnHit", RpcTarget.Others, team);
            return;
        }

        player.GetMovement().GetCurrentTile().ChangeAdjacentTiles(team);
        GameUIManager.instance.GotHit(player.IsTeam(team), team);
        player.GetMovement().PauseMoving();
        player.GetAnimator().SetTrigger("OnHit");
        player.GetPlayerSound().PlayGotHitSound();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------


    //---------------------------------------------
    #region Using Scoop
    //---------------------------------------------

    public void Reset()
    {
        hasScoop = false;
        scoopInput.InputReset();
    }

    //---------------------------------------------

    public void GotScoop()
    {
        hasScoop = true;
        GameUIManager.instance.GotScoop();
        player.GetPlayerSound().PlayGotScoopSound();
    }

    //---------------------------------------------

    public void TryAttacking()
    {
        if (hasScoop && scoopInput.input != Direction8.none)
        {
            UseScoop();
        }
    }

    //---------------------------------------------

    void UseScoop()
    {
        Tile currentTile = player.GetMovement().GetCurrentTile();
        Tile destinationTile = player.GetMovement().GetDestinationTile();

        Scoop scoop = PhotonNetwork.Instantiate(scoopPrefab.name, destinationTile.GetTilePoint(), Quaternion.identity).GetComponent<Scoop>();
        scoop.SetParent(scoopParent).SetTeam(player.myTeam).SetStartTileAndDir(currentTile, destinationTile, scoopInput.input);
        //scoop.StartMove();
        scoopInput.InputReset();
        hasScoop = false;
        GameUIManager.instance.UsedScoop();
        player.GetPlayerSound().PlayScoopThrownSound();

    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior Functions
    //---------------------------------------------
    private void Start()
    {
        player = GetComponent<Player>();
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------
}
