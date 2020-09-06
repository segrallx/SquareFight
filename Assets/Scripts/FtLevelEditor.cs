using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;


public class FtLevelEditor : EditorWindow
{
    public int toolbarOption = 0;
    public string[] toolbarTexts = { "关 卡", "属 性" };

    private int levelSelectIdx = 0;

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
            clearCurrentLevel();
            loadLevelByData();
        }


        GUILayout.EndVertical();
    }

    void clearCurrentLevel()
    {
        Debug.LogFormat("clear current level");
    }

    void loadLevelByData()
    {
        Debug.LogFormat("loadLevelByData");
    }

    #endregion


}
