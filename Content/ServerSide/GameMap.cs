using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Terraria;
using Terraria.GameContent.Achievements;
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
    Goblin
}

public class MapData
{
    public int? TileType { get; set; }
    public int? WallType  { get; set; }
    public int? TileColor  { get; set; }
    public int? WallColor  { get; set; }
}
public class GameMap
{   
    public int PasteX { get; set; }
    public int PasteY { get; set; }

    public GameMap(int pasteX, int pasteY)
    {
        PasteX = pasteX;
        PasteY = pasteY;
    }
    

        public static Dictionary<MapTypes, List<List<MapData>>> PreloadedMaps = new();

    public static void PreloadAllMaps()
    {
        foreach (MapTypes mapType in Enum.GetValues(typeof(MapTypes)))
        {
            string fileName = mapType.ToString().ToLower();
            var mod = ModContent.GetInstance<CTG2>();
            using (var stream = mod.GetFileStream($"Content/MapSaves/{fileName}.json"))
            using (var fileReader = new StreamReader(stream))
            {
                var jsonData = fileReader.ReadToEnd();
                try
                {
                    var mapData = JsonSerializer.Deserialize<List<List<MapData>>>(jsonData);
                    PreloadedMaps[mapType] = mapData;
                }
                catch
                {
                    Main.NewText($"Failed to preload map {fileName}.", Microsoft.Xna.Framework.Color.Red);
                }
            }
        }
    }
    public List<List<MapData>> GetMap(MapTypes map)
    {
        if (PreloadedMaps.TryGetValue(map, out var cached))
        {
            return cached;
        }
            
        string fileName = map.ToString().ToLower();
        var mod = ModContent.GetInstance<CTG2>();

        using (var stream = mod.GetFileStream($"Content/MapSaves/{fileName}.json"))
        using (var fileReader = new StreamReader(stream))
        {
            var jsonData = fileReader.ReadToEnd();
            try
            {
                var mapData = JsonSerializer.Deserialize<List<List<MapData>>>(jsonData);
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
        /*
        needs to be accessed by method in GameManager loadMap

        */
        var mapData = GetMap(mapPick);
        int startX = PasteX;
        int startY = PasteY;
        
        int mapWidth = mapData[0].Count;
        int mapHeight = mapData.Count;
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {   
                
                int wx = x+startX;
                int wy = y+startY;
                Tile tile = Framing.GetTileSafely(wx, wy);
                var mapTile = mapData[y][x];
                
                if (mapTile.TileType.HasValue)
                {   
                    WorldGen.PlaceTile(wx, wy, (mapTile.TileType ?? 0), 
                        mute: true, forced: true, -1, style: 0);
                    var newTile = Framing.GetTileSafely(wx, wy);
                    newTile.Slope = 0;
                }
                else
                {
                    WorldGen.KillTile(wx, wy, noItem: true);
                }

                if (mapTile.WallType.HasValue)
                {   
                    WorldGen.PlaceWall(wx, wy, (mapTile.WallType ?? 0), mute: true);
                    
                }
                else
                {
                    tile.WallType = 0;
                }
                tile.TileColor = (byte)(mapTile.TileColor ?? 0);
                tile.WallColor = (byte)(mapTile.WallColor ?? 0);
                
                // Send tile update squares
                if (x % 7 == 0 || y % 7 == 0)
                {
                    NetMessage.SendTileSquare(-1, x+startX, y+startY, 15);
                }
            }
        }
    }
}