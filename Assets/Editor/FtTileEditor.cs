using UnityEditor;



[CustomEditor(typeof(FtTile))]
public class FtTileEditor :Editor
{
	FtTile tile;
    bool showWeapons;

	void OnEnable()
    {
        tile = (FtTile)target;
    }

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
	}

	public override void OnInspectorGUI()
    {
		base.OnInspectorGUI();
		// EditorGUILayout.BeginVertical();
		// EditorGUILayout.LabelField("CanMoveIn");
		// EditorGUILayout.EndVertical();
	}
}
