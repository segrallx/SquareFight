using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FtLevelEditor : EditorWindow
{
    public int toolbarOption = 0;
    public string[] toolbarTexts = { "关 卡", "属 性" };
    private int levelSelectIdx = -1;

    // 当前操作的关卡数据
    //private FtDLevel ftDLevel;
    // 瓦片对象
    public Tile[] tileObjs;

    #region ShowTileMapEditor

    [MenuItem("Tools/LevelEditor %t")]
    public static void ShowTileMapEditor()
    {
        if (EditorSceneManager.GetActiveScene().name == "LevelEditor")
        {
            FtLevelEditor window = (FtLevelEditor)EditorWindow.GetWindow(typeof(FtLevelEditor));
            window.titleContent = new GUIContent("LevelEditor");
            window.Show();
        }
        else
        {
            if (EditorUtility.DisplayDialog("提示",
"打开编辑器需要跳转到LevelEditor场景，点击确定将会自动保存，点击取消则继续留在当前场景", "确定", "取消"))
            {
                EditorSceneManager.SaveOpenScenes();
                EditorSceneManager.OpenScene("Assets/Scenes/LevelEditor.unity");

                FtLevelEditor window = (FtLevelEditor)EditorWindow.GetWindow(typeof(FtLevelEditor));
                window.titleContent = new GUIContent("LevelEditor");
                window.Show();
            }
        }
    }

    #endregion

    #region OnGUI

    void OnGUI()
    {
        //toolbarOption = GUILayout.Toolbar(toolbarOption, toolbarTexts, GUILayout.Height(21));
        guiLevelPopUp();

        // switch (toolbarOption)
        // {
        //     case 0:
        //         // Title("Editor");
        //         // TileMapContent();
        //         break;
        //     case 1:
        //         // Title("Setting");
        //         // SettingContent();
        //         break;
        //     case 2:
        //         // Title("About");
        //         // AboutContent();
        //         break;
        // }
    }

	void refreshLevelList()
	{
	}

    void guiLevelPopUp()
    {
        GUILayout.Space(10);
        GUILayout.BeginVertical();
        string[] levelListName;
        int[] levelListIdx;

        FtLevelData.GetLevelSelectInfo(out levelListName, out levelListIdx);
        var newLevelSelectIdx = EditorGUILayout.IntPopup("Level Selector",
                                                         levelSelectIdx,
                                                         levelListName,
                                                         levelListIdx,
                                                         GUILayout.Width(300)
                                                  );

        if (newLevelSelectIdx != levelSelectIdx)
        {
            Debug.LogFormat("select index {0}", newLevelSelectIdx);
            saveLevelByData();
            levelSelectIdx = newLevelSelectIdx;
            loadTileFromAssert();
            loadLevelByData();
        }

		GUILayout.EndVertical();

		GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save", GUILayout.Width(80)))
        {
            saveLevelByData();
        }

		if (GUILayout.Button("Refresh", GUILayout.Width(80)))
        {
			FtLevelData.SetDirty();
			loadTileFromAssert();
			loadLevelByData();
        }

		GUILayout.EndHorizontal();
    }

    // void clearCurrentLevel()
    // {
    //     Debug.LogFormat("clear current level");
    // }

    void loadTileFromAssert()
    {
        string[] list = {
            "FtNone.asset",
            "FtFloor.asset",
            "FtHero.asset",
            "FtOrcArcher.asset",
            "FtOrcSilly.asset",
            "FtRock.asset",
        };

        tileObjs = new Tile[list.Length];
        for (int i = 0; i < list.Length; i++)
        {
            tileObjs[i] = UnityEditor.AssetDatabase.
                LoadAssetAtPath<Tile>("Assets/LevelTiled/" + list[i]);
        }
    }

    // 测试读取tilemap信息.
    void gridShowTest()
    {

    }

    // 从文件中加载关卡数据.
    void loadLevelByData()
    {
        //gridShowTest();
        if (levelSelectIdx < 0)
        {
            return;
        }

        string data = "";
        if (!FtLevelData.GetLevelData(levelSelectIdx, ref data))
        {
            Debug.LogErrorFormat("load level {0} data error ", levelSelectIdx);
            return;
        }

        FtDLevel ftDLevel = JsonUtility.FromJson<FtDLevel>(data);
        var grid = GameObject.Find("Grid");
        if (grid != null)
        {
            DestroyImmediate(grid);
        }

        var gridObj = new GameObject("Grid");
        gridObj.transform.position = new Vector3(-0.5f, -0.5f, 0);
        gridObj.AddComponent<Grid>();

        //for (int i = 0; i < ftDLevel.TileMaps.Count; i++)
        foreach (var tileMap in ftDLevel.TileMaps)
        {
            //var tileMap = ftDLevel.TileMaps[i];
            GameObject tilemap = new GameObject(tileMap.Name);
            tilemap.transform.SetParent(gridObj.transform);
            tilemap.transform.localPosition = Vector3.zero;
            Tilemap map = tilemap.AddComponent<Tilemap>();
            TilemapRenderer render = tilemap.AddComponent<TilemapRenderer>();
			render.sortingOrder = tileMap.SortingOrder;
            // render.sortOrder = (TilemapRenderer.SortOrder)tilemapData[i].SortOrderIndex;
            // render.sortingOrder = tilemapData[i].OrderInLayer;
            // render.sortingLayerName = SortingLayer.layers[tilemapData[i].SortingLayerIndex].name;
            //Map.SetTileMap(map, tilemapData[i]);
            //for (int j = 0; j < tileMap.Tiles.Length; j++)
            foreach (var tile in tileMap.Tiles)
            {
                //var tile = tileMap.Tiles[j];
                map.SetTile(tile.IPos, tileObjs[tile.Type]);
            }
        }

        Debug.LogFormat("loadLevelByData ok");
    }

    // 将关卡数据序列化保存到文件中.
    void saveLevelByData()
    {
        if (levelSelectIdx < 0)
        {
            return;
        }

        var grid = GameObject.Find("Grid");
        FtDLevel ftDLevel = new FtDLevel();

        for (var tileMapIdx = 0; tileMapIdx < grid.transform.childCount; tileMapIdx++)
        {
            var child = grid.transform.GetChild(tileMapIdx);
            var tilemap = child.GetComponent<Tilemap>();
			var tilemapRender = child.GetComponent<TilemapRenderer>();
            var tileDMap = new FtDTileMap();
            var area = tilemap.cellBounds;
            var tileArray = tilemap.GetTilesBlock(area);

			tileDMap.Name = tilemap.name;
			tileDMap.Bound = new FtDBound(area.xMin, area.xMax, area.yMin, area.yMax);

            for (int i = area.xMin; i < area.xMax; i++)
            {
                for (int j = area.yMin; j < area.yMax; j++)
                {
                    var pos = new Vector3Int(i, j, 0);
                    var ftTile = tilemap.GetTile<FtTile>(pos);
                    if (ftTile == null)
                    {
                        continue;
                    }

                    var tileD = new FtDTile();
					tileD.Name = ftTile.name;
                    tileD.IPos = pos;
                    tileD.Type = (int)ftTile.mType;
                    tileDMap.Tiles.Add(tileD);
					tileDMap.SortingOrder = tilemapRender.sortingOrder;
                }
            }

            ftDLevel.TileMaps.Add(tileDMap);
        }

        string data = JsonUtility.ToJson(ftDLevel);
        Debug.LogFormat("set level data {0}", levelSelectIdx);
		//Debug.LogFormat("{0}", data);
        FtLevelData.SetLevelData(levelSelectIdx, data);
        Debug.LogFormat("saveLevelByData ok {0}", levelSelectIdx);

    }

    #endregion


}
