using Photon.Pun;
using UnityEngine;

public class Scoop : MonoBehaviourPun
{
    Team myTeam;
    Tile currentTile;
    Tile destinationTile;
    float moveSpeed = 10f;
    float invincibleTimer = 0;
    float maxTimer = 1f;


    // Update is called once per frame
    void Update()
    {
        //if (!canMove) return;
        
        Move();
        invincibleTimer += Time.deltaTime;        
        if (OnDest())
        {
            GetNextTile();
        }
    }

    private void FixedUpdate()
    {
        Planet.instance.Attract(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            if(player.photonView.IsMine && invincibleTimer < maxTimer)
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


    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationTile.GetMidPoint(), moveSpeed * Time.deltaTime);
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
        if (Vector3.Distance(destinationTile.GetMidPoint(), transform.position) <= Tile.closeAdjacency)
        {
            return true;
        }
        return false;
    }
}
