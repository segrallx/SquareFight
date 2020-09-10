using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/FtTile")]
public class FtTile : Tile
{

	public enum Type {
		None,
		Floor,
		Hero,
		OrcArcher,
		OrcSilly,
		Rock,
		Key,
	};

	public Type mType;

	public override string ToString()
	{
		return "tst";
	}



}
