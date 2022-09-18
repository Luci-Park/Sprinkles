using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tile : MonoBehaviour
{
    public static float closeAdjacency = 0.01f;

    Team myTeam = Team.none;

    Renderer rend;
    float colorChangeSmooth = 0.001f;
    float colorChangeDur = 0.01f;

    public List<Tile> adjacentTiles = new List<Tile>();

    Vector3[] adjacentTilePositions = new Vector3[9];
    float tileAdjacentDistance = 0.03f;

    int tileIndex;

    //---------------------------------------------
    #region Color Functions
    //---------------------------------------------

    public void ChangeColor(Team newTeam)
    {
        if (newTeam == myTeam) return;
        Planet.instance.RPC_SendChangeTileColorMesg(tileIndex, newTeam);
        StartCoroutine(LerpColor(TeamController.GetTeamColor(myTeam), TeamController.GetTeamColor(newTeam)));
        myTeam = newTeam;
    }

    //---------------------------------------------

    public Team GetTileTeam()
    {
        return myTeam;
    }

    //---------------------------------------------

    public void ChangeAdjacentTiles(Team team)
    {
        foreach(Tile tile in adjacentTiles)
        {
            tile.ChangeColor(team);
        }
    }

    //---------------------------------------------

    IEnumerator LerpColor(Color pastTeam, Color newTeam)
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = colorChangeSmooth / colorChangeDur; //The amount of change to apply.
        while (progress < 1)
        {
            rend.material.color = Color.Lerp( pastTeam, newTeam, progress);
            progress += increment;
            yield return new WaitForSeconds(colorChangeSmooth);
        }
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Tile Info Get Functions
    //---------------------------------------------

    public Vector3 GetTilePoint()
    {
        if(adjacentTilePositions[(int)Direction8.middle] == null)
        {
            Debug.Log("no middle");
        }
        return adjacentTilePositions[(int)Direction8.middle];
    }
    //---------------------------------------------
    
    public Tile GetNextDest(Tile beforeTile, Direction8 moveDirection)
    {

        Direction8 currentDirection = Direction8.down;
        if (beforeTile != this)
        {
            for (Direction8 dir = Direction8.up; dir <= Direction8.right; ++dir)
            {
                if(adjacentTiles[(int)dir] == beforeTile)
                {
                    currentDirection = dir;
                    break;
                }
            }
        }

        Direction8 nextDirection = DirectionFunctions.GetRelativeDirection(currentDirection, moveDirection);
        return adjacentTiles[(int)nextDirection];
    }
    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Tile Info Set functions
    //---------------------------------------------

    public void SetTileInfo()
    {
        rend = GetComponent<Renderer>();
        SetPoints();
        GetListOfAdjacentTiles();
    }

    //---------------------------------------------
    void GetListOfAdjacentTiles()
    {
        for (Direction8 dir = Direction8.middle; dir < Direction8.none; ++dir)
        {
            GetAdjecentTilesAtPoint(dir);
        }
    }

    //---------------------------------------------

    Tile GetTileInDirection(Direction8 direction)
    {
        Tile result = null;
        Collider[] hitColliders = Physics.OverlapSphere(adjacentTilePositions[(int)direction], tileAdjacentDistance);
        foreach(Collider coll in hitColliders)
        {
            if(coll != GetComponent<Collider>() && coll.GetComponent<Tile>() != null)
            {
                result = coll.GetComponent<Tile>();
                break;
            }
        }
        return result;
    }

    //---------------------------------------------

    void GetAdjecentTilesAtPoint(Direction8 direction)
    {
        if(direction == Direction8.middle)
        {
            adjacentTiles.Add(this);
            return;
        }
        Collider[] hitColliders = Physics.OverlapSphere(adjacentTilePositions[(int)direction], tileAdjacentDistance);
        foreach(Collider coll in hitColliders)
        {
            Tile newTile = coll.GetComponent<Tile>();
            if(newTile != null && !adjacentTiles.Contains(newTile))
            {
                adjacentTiles.Add(newTile);
            }
        }
    }

    //---------------------------------------------

    void SetPoints()
    {
        Vector3[] corners = GetComponent<MeshFilter>().sharedMesh.vertices;
        adjacentTilePositions[(int)Direction8.middle] = CalculateOrbit();
        adjacentTilePositions[(int)Direction8.upLeft] = transform.TransformPoint(corners[1]);
        adjacentTilePositions[(int)Direction8.upRight] = transform.TransformPoint(corners[2]);
        adjacentTilePositions[(int)Direction8.downRight] = transform.TransformPoint(corners[3]);
        adjacentTilePositions[(int)Direction8.downLeft] = transform.TransformPoint(corners[0]);
        adjacentTilePositions[(int)Direction8.up] = GetMidPoint(adjacentTilePositions[(int)Direction8.upLeft], adjacentTilePositions[(int)Direction8.upRight]);
        adjacentTilePositions[(int)Direction8.down] = GetMidPoint(adjacentTilePositions[(int)Direction8.downLeft], adjacentTilePositions[(int)Direction8.downRight]);
        adjacentTilePositions[(int)Direction8.left] = GetMidPoint(adjacentTilePositions[(int)Direction8.upLeft], adjacentTilePositions[(int)Direction8.downLeft]);
        adjacentTilePositions[(int)Direction8.right] = GetMidPoint(adjacentTilePositions[(int)Direction8.upRight], adjacentTilePositions[(int)Direction8.downRight]);

    }

    //---------------------------------------------

    Vector3 CalculateOrbit()
    {
        Vector3 mid = rend.bounds.center;
        Vector3 dir = (Vector3.zero - mid).normalized;
        RaycastHit hit;
        Physics.Raycast(mid, dir, out hit, 1f);

        return hit.point - dir * PlayerMovement.heightOffset;
    }

    //---------------------------------------------

    Vector3 GetMidPoint(Vector3 v1, Vector3 v2)
    {
        return (v1 + v2) / 2;
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region Tile Info Debug Functions
    //---------------------------------------------

    public void PrintTileInfo()
    {
        GetListOfAdjacentTiles();
        Debug.Log("adjacent tiles to " + gameObject.name);
        foreach (Tile tile in adjacentTiles)
        {
            Debug.Log(tile.name);
        }
    }

    //---------------------------------------------

    public void ShowTileInDirection(int i)
    {
        Tile tile = GetTileInDirection((Direction8)i);
        Debug.Log((Direction8)i);
        if (tile == null)
        {
        }
        else
        {
            Debug.Log(tile.name);
        }
    }

    //---------------------------------------------

    /*
    private void OnDrawGizmos()
    {
        rend = GetComponent<Renderer>();
        Gizmos.color = Color.blue;
        Vector3 mid = rend.bounds.center;
        Vector3 dir = (Vector3.zero - mid).normalized;
        Gizmos.DrawSphere(mid, 0.03f);
        RaycastHit hit;
        if(Physics.Raycast(mid, dir, out hit, 1f))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.03f);
        }
        else
        {
            Debug.Log(gameObject.name);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(hit.point - dir * Player.heightOffset, 0.03f);

    }*/

    //---------------------------------------------

    public void DrawPoint()
        {

            Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.white, Color.cyan, Color.magenta, Color.black, Color.grey };
            SetPoints();
            int i = 0;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction8.middle], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction8.upLeft], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction8.upRight], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction8.downRight], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction8.downLeft], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction8.up], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction8.down], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction8.left], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction8.right], 0.03f);
        }

    //---------------------------------------------
    #endregion
    //---------------------------------------------



    //---------------------------------------------
    #region MonoBehavior
    //---------------------------------------------

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        SetTileInfo();
        tileIndex = transform.GetSiblingIndex();
    }

    //---------------------------------------------
    #endregion
    //---------------------------------------------

}
