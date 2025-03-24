using Terraria;

public class TileSnapshot
{
    public ushort? TileType; 
    public ushort? WallType; 
    public byte TileColor;   // for painted blocks
    public byte WallColor;   // for painted walls

    public TileSnapshot(Tile tile)
    {
        if (tile.HasTile)
            TileType = tile.TileType;

        if (tile.WallType > 0)
            WallType = tile.WallType;

        TileColor = tile.TileColor;
        WallColor = tile.WallColor;
    }

    public void ApplyTo(Tile tile)
    {
        if (TileType.HasValue)
        {
            tile.TileType = TileType.Value;
            tile.HasTile = true;
        }
        else
        {
            tile.HasTile = false;
        }

        tile.WallType = WallType ?? 0;
        tile.TileColor = TileColor;
        tile.WallColor = WallColor;
    }
}
