using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class TileInfoSaver : MonoBehaviour
{
    public static string filename;
    public static string filePath;

    private void Awake()
    {
        filename = transform.name + ".json" ;
        filePath = Path.Combine(Application.persistentDataPath + "/MapInfoSaves/", filename);
    }
    private void Start()
    {
        if (File.Exists(filePath))
        {
            GetFile();
        }
        else
        {
            SaveFile();
        }
    }

    public void SaveFile()
    {
        Directory.CreateDirectory(filePath);
        ListOfTiles newInfo = new ListOfTiles();
        string json = JsonUtility.ToJson(newInfo);
        newInfo.tileInfo = new TileInfo[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            newInfo.tileInfo[i] = transform.GetChild(i).GetComponent<Tile>().GetTileInfo();
        }
        File.WriteAllText(filePath, json);
    }

    public void GetFile()
    {
        string json = File.ReadAllText(filePath);
        ListOfTiles mapInfo = JsonUtility.FromJson<ListOfTiles>(json);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Tile>().SetTileInfo(mapInfo.tileInfo[i]);
        }
    }

}


[System.Serializable]
public class TileInfo
{
    public int tileNum;
    public int upTile;
    public int downTile;
    public int leftTile;
    public int rightTile;
    public int [] adjacentTiles;
}

[System.Serializable]
public class ListOfTiles
{
    public TileInfo[] tileInfo;
}