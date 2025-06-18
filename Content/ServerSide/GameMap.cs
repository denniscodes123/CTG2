using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Terraria;
using Terraria.ModLoader;

namespace CTG2.Content.ServerSide;

public enum MapTypes
{
    Kraken,
    Cranes,
    Stalactite,
    Triangles,
    Keep,
    Classic,
    SteppingStones,
    Goblin
}

public class MapData
{
    public int TileType { get; set; }
    public int WallType  { get; set; }
    public int TileColor  { get; set; }
    public int WallColor  { get; set; }
}
public class GameMap
{
    public List<MapData> GetMap(MapTypes map)
    {
        string fileName = map.ToString().ToLower();
        var mod = ModContent.GetInstance<CTG2>();
        
        using (var stream = mod.GetFileStream($"Content/MapSaves/{fileName}.json"))
        using (var fileReader = new StreamReader(stream))
        {
            var jsonData = fileReader.ReadToEnd();
            try
            {
                var mapData = JsonSerializer.Deserialize<List<MapData>>(jsonData);
                return mapData;
            }
            catch
            {
                Main.NewText("Failed to load or parse inventory file.", Microsoft.Xna.Framework.Color.Red);
                return null;
            }
        }
    }
    public void LoadMap(MapTypes mapPick)
    {
        var mapData = GetMap(mapPick);
        int startX = 600;
        int startY = 100;
        
        int mapWidth = 500;
        int mapHeight = 100;
        int idx = 0;
        for (int y = startY; y < startY + mapHeight; y++)
        {
            for (int x = startX; x < startX + mapWidth; x++)
            {   
                Tile tile = Framing.GetTileSafely(x, y);

                tile.TileType = (ushort)mapData[idx].TileType;
                tile.WallType = (ushort)mapData[idx].WallType;
                tile.TileColor = (byte)mapData[idx].TileColor;
                tile.WallColor = (byte)mapData[idx].WallColor;
                idx++;
            }
        }
    }
}