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

    private Tile upTile, leftTile, rightTile, downTile;
    List<Tile> adjacentTiles = new List<Tile>();

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

    public Vector3 GetMidPoint()
    {
        if(adjacentTilePositions[(int)Direction.middle] == null)
        {
            Debug.Log("no middle");
        }
        return adjacentTilePositions[(int)Direction.middle];
    }
    //---------------------------------------------
    
    public Tile GetNextDest(Tile beforeTile, Direction moveDirection)
    {
        if(upTile == beforeTile)
        {
            switch (moveDirection)
            {
                case Direction.up:
                    return downTile;
                case Direction.down:
                    return upTile;
                case Direction.left:
                    return rightTile;
                case Direction.right:
                    return leftTile;
            }
        }
        else if(leftTile == beforeTile)
        {

            switch (moveDirection)
            {
                case Direction.up:
                    return rightTile;
                case Direction.down:
                    return leftTile;
                case Direction.left:
                    return upTile;
                case Direction.right:
                    return downTile;
            }
        }
        else if(downTile == beforeTile || this == beforeTile)
        {

            switch (moveDirection)
            {
                case Direction.up:
                    return upTile;
                case Direction.down:
                    return downTile;
                case Direction.left:
                    return leftTile;
                case Direction.right:
                    return rightTile;
            }
        }
        else if( rightTile == beforeTile)
        {
            switch (moveDirection)
            {
                case Direction.up:
                    return leftTile;
                case Direction.down:
                    return rightTile;
                case Direction.left:
                    return downTile;
                case Direction.right:
                    return upTile;
            }
        }
        return null;
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
        Set4DirTiles();
        GetListOfAdjacentTiles();
    }

    //---------------------------------------------

    public void SetTileInfo(TileInfo info)
    {
        if(info.tileNum != transform.GetSiblingIndex())
        {
            Debug.LogError("wrong info");
        }
        Transform parent = Planet.instance.transform;
        upTile = parent.GetChild(info.upTile).GetComponent<Tile>();
        downTile = parent.GetChild(info.downTile).GetComponent<Tile>();
        leftTile = parent.GetChild(info.leftTile).GetComponent<Tile>();
        rightTile = parent.GetChild(info.rightTile).GetComponent<Tile>();

        foreach(int tile in info.adjacentTiles)
        {
            adjacentTiles.Add(parent.GetChild(tile).GetComponent<Tile>());
        }
    }

    //---------------------------------------------

    public TileInfo GetTileInfo()
    {
        SetTileInfo();
        TileInfo info = new TileInfo();
        info.tileNum = transform.GetSiblingIndex();
        info.upTile = upTile.transform.GetSiblingIndex();
        info.downTile = downTile.transform.GetSiblingIndex();
        info.leftTile = leftTile.transform.GetSiblingIndex();
        info.rightTile = rightTile.transform.GetSiblingIndex();

        info.adjacentTiles = new int[adjacentTiles.Count];
        for(int i = 0; i< adjacentTiles.Count; i++)
        {
            info.adjacentTiles[i] = adjacentTiles[i].transform.GetSiblingIndex();
        }
        return info;
    }

    //---------------------------------------------
    void GetListOfAdjacentTiles()
    {
        GetAdjecentTilesAtPoint(Direction.downLeft);
        GetAdjecentTilesAtPoint(Direction.downRight);
        GetAdjecentTilesAtPoint(Direction.upLeft);
        GetAdjecentTilesAtPoint(Direction.upRight);
    }

    //---------------------------------------------
    void Set4DirTiles()
    {
        upTile = GetTileInDirection(Direction.up);
        downTile = GetTileInDirection(Direction.down);
        leftTile = GetTileInDirection(Direction.left);
        rightTile = GetTileInDirection(Direction.right);
    }

    //---------------------------------------------

    Tile GetTileInDirection(Direction direction)
    {
        Tile result = null;
        Collider[] hitColliders = Physics.OverlapSphere(adjacentTilePositions[(int)direction], tileAdjacentDistance);
        foreach(Collider coll in hitColliders)
        {
            if(coll.GetComponent<Tile>() != null && coll != GetComponent<Collider>())
            {
                result = coll.GetComponent<Tile>();
                break;
            }
        }
        return result;
    }

    //---------------------------------------------

    void GetAdjecentTilesAtPoint(Direction direction)
    {
        Collider[] hitColliders = Physics.OverlapSphere(adjacentTilePositions[(int)direction], tileAdjacentDistance);
        foreach(Collider coll in hitColliders)
        {
            if (coll == GetComponent<Collider>()) continue;
            Tile newTile = coll.GetComponent<Tile>();
            if(newTile != null)
            {
                if (!adjacentTiles.Contains(newTile))
                {
                    adjacentTiles.Add(newTile);
                }
            }
        }
    }

    //---------------------------------------------

    void SetPoints()
    {
        Vector3[] corners = GetComponent<MeshFilter>().sharedMesh.vertices;
        adjacentTilePositions[(int)Direction.middle] = CalculateOrbit();
        adjacentTilePositions[(int)Direction.upLeft] = transform.TransformPoint(corners[1]);
        adjacentTilePositions[(int)Direction.upRight] = transform.TransformPoint(corners[2]);
        adjacentTilePositions[(int)Direction.downRight] = transform.TransformPoint(corners[3]);
        adjacentTilePositions[(int)Direction.downLeft] = transform.TransformPoint(corners[0]);
        adjacentTilePositions[(int)Direction.up] = GetMidPoint(adjacentTilePositions[(int)Direction.upLeft], adjacentTilePositions[(int)Direction.upRight]);
        adjacentTilePositions[(int)Direction.down] = GetMidPoint(adjacentTilePositions[(int)Direction.downLeft], adjacentTilePositions[(int)Direction.downRight]);
        adjacentTilePositions[(int)Direction.left] = GetMidPoint(adjacentTilePositions[(int)Direction.upLeft], adjacentTilePositions[(int)Direction.downLeft]);
        adjacentTilePositions[(int)Direction.right] = GetMidPoint(adjacentTilePositions[(int)Direction.upRight], adjacentTilePositions[(int)Direction.downRight]);

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
        Set4DirTiles();
        GetListOfAdjacentTiles();
        Debug.Log("adjacent tiles to " + gameObject.name);
        foreach (Tile tile in adjacentTiles)
        {
            Debug.Log(tile.name);
        }

        Debug.Log("up: " + upTile.name);
        Debug.Log("left: " + leftTile.name);
        Debug.Log("right: " + rightTile.name);
        Debug.Log("down: " + downTile.name);
    }

    //---------------------------------------------

    public void ShowTileInDirection(int i)
    {
        Tile tile = GetTileInDirection((Direction)i);
        Debug.Log((Direction)i);
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
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction.middle], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction.upLeft], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction.upRight], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction.downRight], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction.downLeft], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction.up], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction.down], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction.left], 0.03f);
            i++;
            Gizmos.color = colors[i];
            Gizmos.DrawSphere(adjacentTilePositions[(int)Direction.right], 0.03f);
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
