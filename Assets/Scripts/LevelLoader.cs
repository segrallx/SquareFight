using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public string LevelName;
    public GameObject FloorObj;
    public GameObject ElementObj;

    private FtDLevel ftDLevel;
    private Dictionary<string, GameObject> tilePrefebDict;
    private string tilePrefabPath = "Prefabs";

    private static LevelLoader __instance = null;
    public static LevelLoader Instance()
    {
        return __instance;
    }

    void Awake()
    {
        loadTilePrefebs();
        loadLevelData();

        var tileMap1 = ftDLevel.TileMaps[0];
        doRender(FloorObj, tileMap1);
        var floors = FloorObj.GetComponent<Floors>();
        floors.Init(tileMap1.Bound);

        var tileMap2 = ftDLevel.TileMaps[1];
        doRender(ElementObj, tileMap2);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void loadLevelData()
    {
        string data = "";
        FtLevelData.GetLevelData(LevelName, ref data);
        ftDLevel = JsonUtility.FromJson<FtDLevel>(data);
        Debug.LogFormat("load level {0}", LevelName);
    }

    void loadTilePrefebs()
    {
        tilePrefebDict = new Dictionary<string, GameObject>();
        var tileList = Resources.LoadAll<GameObject>(tilePrefabPath);
        foreach (var tile in tileList)
        {
            //Debug.LogFormat("load resource tile {0}", tile.name);
            tilePrefebDict[tile.name] = tile;
        }
    }


    void doRender(GameObject parent, FtDTileMap tileMap)
    {
        foreach (var tile in tileMap.Tiles)
        {
            if (!tilePrefebDict.ContainsKey(tile.Name))
            {
                Debug.LogErrorFormat("tile prefeb miss {0}", tile.Name);
                continue;
            }
            var tilePrefab = tilePrefebDict[tile.Name];
            var pos = new Vector3(tile.IPos.x, tile.IPos.y);
            Instantiate(tilePrefab, pos, Quaternion.identity, parent.transform);
        }
    }


}
