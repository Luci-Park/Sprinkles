using Photon.Pun;
using UnityEngine;

public class Scoop : MonoBehaviourPun
{
    Team myTeam;
    Tile currentTile;
    Tile destinationTile;
    float moveSpeed = 10f;
    float invincibleTime = 0.5f;
    float timer = 0;
    bool invincible = true;

    // Update is called once per frame
    void Update()
    {
        //if (!canMove) return;
        timer += Time.deltaTime;
        if (timer > invincibleTime) invincible = false;
        Move();
        if (OnDest())
        {
            GetNextTile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            if(player.photonView.IsMine && invincible)
            {
                return;
            }
            if (!player.photonView.IsMine)
            {
                player.SuccessfulHit();
                
            }
            player.GetPlayerScoop().OnHit(myTeam);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if(player != null && player.photonView.IsMine)
        {
            invincible = false;
        }
    }


    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationTile.GetTilePoint(), moveSpeed * Time.deltaTime);
    }
    public Scoop SetParent(Transform parent)
    {
        transform.SetParent(parent);
        return this;
    }
    public Scoop SetTeam(Team team)
    {
        myTeam = team;
        return this;
    }

    public Scoop SetStartTileAndDir(Tile beforeTile, Tile currentTile, Direction dir)
    {
        Planet.instance.Attract(transform);
        this.currentTile = currentTile;
        destinationTile = currentTile.GetNextDest(beforeTile, dir);
        return this;
    }

    void GetNextTile()
    {
        Tile temp = destinationTile.GetNextDest(currentTile, Direction.up);
        currentTile = destinationTile;
        destinationTile = temp;
    }
    bool OnDest()
    {
        if (Vector3.Distance(destinationTile.GetTilePoint(), transform.position) <= Tile.closeAdjacency)
        {
            return true;
        }
        return false;
    }
}
