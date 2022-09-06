using System.Collections;
using UnityEngine;

using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public static readonly float heightOffset = 0.5f;

    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] float pauseTime;


    Player player;
    PlayerInput movementInput;

    Tile currentTile;
    Tile destinationTile;

    readonly Vector3 up = Vector3.zero,
        down = new Vector3(0, 180, 0),
        left = new Vector3(0, -90, 0),
        right = new Vector3(0, 90, 0);

    Quaternion targetRotation = Quaternion.identity;

    bool doMove = false;

    Coroutine isPausing;

    

    //---------------------------------------------
    #region MonoBehavior Functions
    //---------------------------------------------

    private void Awake()
    {
        SetComponents();
    }

    //---------------------------------------------

    void Update()
    {
        if (!doMove) return;
        Move();
        Rotate();
        if (OnDest())
        {
            player.GetPlayerScoop().TryAttacking();
            GetNextDestination();
            movementInput.DirReset();
            currentTile.ChangeColor(player.myTeam);
        }
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Setting Functions
    //---------------------------------------------

    void SetComponents()
    {
        player = Player.localPlayerInstance.GetComponent<Player>();
        movementInput = GetComponent<PlayerInput>();
    }

    //---------------------------------------------
    
    public void SetStartingPoint(Tile startTile)
    {

        SetCurrentTile(startTile);
        transform.position = startTile.GetTilePoint();
        Planet.instance.Attract(transform, true);
        SetDestinationTile(startTile.GetNextDest(startTile, Direction.up));
        LookForward();
    }

    //---------------------------------------------
    
    public void Reset()
    {
        movementInput.DirReset();
        transform.rotation = Quaternion.identity;
    
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Movement Functions
    //---------------------------------------------

    public void StopMove()
    {
        player.GetAnimator().SetBool("IsMoving", false);
        doMove = false;
        player.GetPlayerSound().StopMoveSound();
    }

    //---------------------------------------------

    public void StartMove()
    {
        player.GetAnimator().SetBool("IsMoving", true);
        currentTile.ChangeColor(player.myTeam);
        doMove = true;
        player.GetPlayerSound().PlayMoveSound();
    }

    //---------------------------------------------

    public void PauseMoving()
    {
        isPausing = StartCoroutine(PauseForSec());
    }

    //---------------------------------------------

    public void CompleteStop()
    {
        StopMove();
        Reset();
        if (isPausing != null)
            StopCoroutine(isPausing);
    }

    //---------------------------------------------

    IEnumerator PauseForSec()
    {
        StopMove();
        yield return new WaitForSeconds(pauseTime);
        StartMove();
        isPausing = null;
    }

    //---------------------------------------------

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationTile.GetTilePoint(), moveSpeed * Time.deltaTime);
    }

    //---------------------------------------------

    bool OnDest()
    {
        if (Vector3.Distance(destinationTile.GetTilePoint(), transform.position) <= Tile.closeAdjacency)
        {
            return true;
        }
        return false;
    }

    //---------------------------------------------

    void Rotate()
    {
        if (destinationTile.GetTilePoint() - transform.position != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(destinationTile.GetTilePoint() - transform.position, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
    }

    //---------------------------------------------

    void LookForward()
    {
        Quaternion targetRotation = Quaternion.LookRotation(destinationTile.GetTilePoint() - transform.position, transform.up);
        transform.rotation = targetRotation;
    }
    
    //---------------------------------------------

    void GetNextDestination()
    {
        Direction input = movementInput.input;
        if (input == Direction.none || input == Direction.up)
        {
            input = Direction.up;
        }
        else
        {
            player.GetPlayerSound().OnTurn();
        }
        Tile temp = destinationTile.GetNextDest(currentTile, input);
        if (temp == null)
        {
            Debug.LogError(currentTile + " " + input);
            doMove = false;
        }

        currentTile = destinationTile;
        destinationTile = temp;
    }
   
    //---------------------------------------------
    #endregion
    //---------------------------------------------
    


    //---------------------------------------------
    #region Tile Get Set
    //---------------------------------------------

    public void SetCurrentTile(Tile tile)
    {
        currentTile = tile;
    }

    //---------------------------------------------

    public void SetDestinationTile(Tile tile)
    {
        destinationTile = tile;
    }

    //---------------------------------------------

    public Tile GetCurrentTile()
    {
        return currentTile;
    }

    //---------------------------------------------

    public Tile GetDestinationTile()
    {
        return destinationTile;
    }

    //---------------------------------------------

    public bool IsOnTile(int tileNum)
    {
        return tileNum == currentTile.transform.GetSiblingIndex() || tileNum == destinationTile.transform.GetSiblingIndex();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------

}
