using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    private string levelName;
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
        doRenderFloor(tileMap1);

        var tileMap2 = ftDLevel.TileMaps[1];
        doRenderElement(tileMap2);
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
        levelName = string.Format("level_01_00{0}", Random.Range(1, 4));
        FtLevelData.GetLevelData(levelName, ref data);
        ftDLevel = JsonUtility.FromJson<FtDLevel>(data);
        Debug.LogFormat("load level {0}", levelName);
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

    delegate void elementCallback(Vector3Int pos, GameObject s);

    // 熏染地板
    void doRenderFloor(FtDTileMap tileMap)
    {
        var floors = FloorObj.GetComponent<Floors>();
        floors.Init(tileMap.Bound);

        foreach (var tile in tileMap.Tiles)
        {
            if (!tilePrefebDict.ContainsKey(tile.Name))
            {
                Debug.LogErrorFormat("tile prefeb miss {0}", tile.Name);
                continue;
            }
            var tilePrefab = tilePrefebDict[tile.Name];
            var pos = new Vector3(tile.IPos.x, tile.IPos.y);
            Instantiate(tilePrefab, pos, Quaternion.identity, FloorObj.transform);
        }
    }

    // 渲染地图元素
    void doRenderElement(FtDTileMap tileMap)
    {
        var floors = FloorObj.GetComponent<Floors>();
        foreach (var tile in tileMap.Tiles)
        {
            if (!tilePrefebDict.ContainsKey(tile.Name))
            {
                Debug.LogErrorFormat("tile prefeb miss {0}", tile.Name);
                continue;
            }
            var tilePrefab = tilePrefebDict[tile.Name];
            var pos = new Vector3(tile.IPos.x, tile.IPos.y);
            var obj = Instantiate(tilePrefab, pos, Quaternion.identity, ElementObj.transform);
            var ele = obj.GetComponent<Element>();
            ele.SetPos(tile.IPos.x, tile.IPos.y);
            floors.SetElementType(tile.IPos.x, tile.IPos.y, ele.ElementType());
        }
    }


}
