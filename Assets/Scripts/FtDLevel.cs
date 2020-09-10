using System.Collections.Generic;
using UnityEngine;

public class FtDConst
{
    // 瓦片类型
    public const int FtTypeFloor = 1;
    public const int FtTypeHero = 2;
    public const int FtTypeOrcArcher = 3;
    public const int FtTypeOrcSilly = 4;
    public const int FtTypeOrcRock = 5;
}

[System.Serializable]
public class FtDTile
{
    public int Type;
    public Vector3Int IPos;
}

[System.Serializable]
public class FtDTileMap
{
    public string Name;
    public List<FtDTile> Tiles;
    public FtDTileMap()
    {
        Tiles = new List<FtDTile>();
    }
}

[System.Serializable]
public class FtDLevel
{
    public List<FtDTileMap> TileMaps;

    public FtDLevel()
    {
        TileMaps = new List<FtDTileMap>();
    }
}