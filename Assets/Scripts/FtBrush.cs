using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
[CustomGridBrush(false, true, false, "Ft Brush")]
public class FtBrush : UnityEditor.Tilemaps.GridBrush
{

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        base.Paint(gridLayout, brushTarget, position);
        Debug.LogFormat("Paint");
    }

    public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
    {
        base.Erase(gridLayout, brushTarget, position);
        Debug.LogFormat("Erase");
    }

}
