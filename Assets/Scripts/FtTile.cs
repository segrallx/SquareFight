using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/FtTile")]
public class FtTile : Tile
{

	public enum Type {
		None,
		Floor,
		Hero,
		Orc,
		Rock,
		Key,
	};

	public Type mType;

}
