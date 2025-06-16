using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content
{
   public static class WorldProperties 
{
    public static TileSnapshot[,] savedRegion;
    public static int savedX;
    public static int savedY;
    
    public static TileSnapshot[,] SaveRegion(int x1, int y1, int x2, int y2)
    {
        int width = x2 - x1 + 1;
        int height = y2 - y1 + 1;
        TileSnapshot[,] region = new TileSnapshot[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = Main.tile[x1 + x, y1 + y];
                region[x, y] = new TileSnapshot(tile);
            }
        }

        return region;
    }

        public static TileSnapshot[,] loadMap(String name)
        {
     string saveDirectory = Path.Combine(Main.SavePath, "Mods", "CTG2", "MapSaves");
    string filePath = Path.Combine(saveDirectory, $"{name}.json");

    if (!File.Exists(filePath))
    {
        Main.NewText($"Failure to locate world: {name}.json");
        return null;
    }
    
    try
    {
        string json = File.ReadAllText(filePath);
        TileSnapshot[,] loadedTiles = JsonConvert.DeserializeObject<TileSnapshot[,]>(json);
        return loadedTiles;
    }
    catch (Exception e)
    {
        ModContent.GetInstance<CTG2>().Logger.Error($"Failed to load or deserialize map '{name}.json'. Error: {e.Message}");
        Main.NewText($"Error loading world: {name}.json. Check logs for details.");
        return null;
    }
        
    }

    public static void RestoreRegion(int x1, int y1, TileSnapshot[,] region)
        {
            int width = region.GetLength(0);
            int height = region.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = Main.tile[x1 + x, y1 + y];
                    region[x, y].ApplyTo(tile);
                    WorldGen.SquareTileFrame(x1 + x, y1 + y);
                    WorldGen.SquareWallFrame(x1 + x, y1 + y);
                }
            }

            int chunkSize = 64;

            for (int x = 0; x < width; x += chunkSize)
            {
                for (int y = 0; y < height; y += chunkSize)
                {
                    int sendX = x1 + x + chunkSize / 2;
                    int sendY = y1 + y + chunkSize / 2;

                    NetMessage.SendTileSquare(-1, sendX, sendY, chunkSize);
                }
            }

        }
}

public class TimeControlSystem : ModSystem
{
        public override void PreUpdateWorld()
        {
            Main.dayTime = true;
            Main.time = 27000;
            Main.eclipse = false;
            Main.bloodMoon = false;
            Main.raining = false;
            Main.maxRaining = 0f;
            Main.cloudAlpha = 0f;
            Main.moonPhase = 0; 
            Main.windSpeedCurrent = 0f;
            Main.windSpeedTarget = 0f;
        }
    

    public class NoEnemySpawns : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {

            spawnRate = int.MaxValue;
            maxSpawns = 0;
        }
    }
}
}

