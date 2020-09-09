using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FtBrush))]
	public class FtBrushEditor : UnityEditor.Tilemaps.GridBrushEditor
	{
		//private FtBrushEditor prefabBrush { get { return target as FtBrushEditor; } }

		public override void PaintPreview(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			base.PaintPreview(grid, brushTarget, position);
		}

		public override void OnPaintInspectorGUI()
		{
			//EditorGUILayout.LabelField(XMConst.CopyRight);
		}

		public override void OnPaintSceneGUI(GridLayout grid, GameObject brushTarget,
											 BoundsInt position, GridBrushBase.Tool tool, bool executing)
		{
			base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);
			Handles.Label(grid.CellToWorld(new Vector3Int(position.x, position.y, position.z)),
						  new Vector3Int(position.x, position.y, position.z).ToString());
		}

	}
