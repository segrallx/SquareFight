using UnityEditor;

[CustomEditor(typeof(LevelLoader))]
public class LevelLoaderEditor : Editor
{
	LevelLoader loader;
    bool showWeapons;

	void OnEnable()
    {
        loader = (LevelLoader)target;
    }

	public override void OnInspectorGUI()
    {
		base.OnInspectorGUI();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("CanMoveIn");
		EditorGUILayout.EndVertical();
	}
}
