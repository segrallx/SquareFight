using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;


public class FtLevelEditor : EditorWindow
{
    public int toolbarOption = 0;
    public string[] toolbarTexts = { "关 卡", "属 性" };
    private int levelSelectIdx = 0;

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
                                                         GUILayout.Width(450)
                                                  );
        if (newLevelSelectIdx != levelSelectIdx)
        {
            levelSelectIdx = newLevelSelectIdx;
            loadTileFromAssert();
            clearCurrentLevel();
            loadLevelByData();
        }


        GUILayout.EndVertical();
    }

    void clearCurrentLevel()
    {
        Debug.LogFormat("clear current level");
    }

    void loadTileFromAssert()
    {
        string[] list = {
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

	// 从文件中加载关卡数据.
    void loadLevelByData()
    {
        string data = "";
        if (!FtLevelData.GetLevelData(levelSelectIdx, ref data))
        {
            Debug.LogErrorFormat("load level {0} data error ", levelSelectIdx);
            return;
        }

		FtDLevel ftDLevel = JsonUtility.FromJson<FtDLevel>(data);

        Debug.LogFormat("loadLevelByData");
        var grid = GameObject.Find("Grid");
        if (grid != null)
        {
            DestroyImmediate(grid);
        }

        var gridObj = new GameObject("Grid");
        gridObj.transform.position = new Vector3(-0.5f, -0.5f, 0);
        gridObj.AddComponent<Grid>();

        for (int i = 0; i < ftDLevel.TileMaps.Length; i++)
        {
            var tileMap = ftDLevel.TileMaps[i];
            GameObject tilemap = new GameObject(tileMap.Name);
            tilemap.transform.SetParent(gridObj.transform);
            tilemap.transform.localPosition = Vector3.zero;
            Tilemap map = tilemap.AddComponent<Tilemap>();
            TilemapRenderer render = tilemap.AddComponent<TilemapRenderer>();
            // render.sortOrder = (TilemapRenderer.SortOrder)tilemapData[i].SortOrderIndex;
            // render.sortingOrder = tilemapData[i].OrderInLayer;
            // render.sortingLayerName = SortingLayer.layers[tilemapData[i].SortingLayerIndex].name;
            //Map.SetTileMap(map, tilemapData[i]);
            for (int j = 0; j < tileMap.Tiles.Length; j++)
            {
                var tile = tileMap.Tiles[j];
                map.SetTile(tile.IPos, tileObjs[tile.Type]);
            }
        }
    }


	// 将关卡数据序列化保存到文件中.
	void saveLevelByData()
    {
		FtDLevel ftDLevel = new FtDLevel();
		var grid = GameObject.Find("Grid");
		//grid.transform

		string data = JsonUtility.ToJson(ftDLevel);
		FtLevelData.SetLevelData(levelSelectIdx, data);
	}

    #endregion


}
