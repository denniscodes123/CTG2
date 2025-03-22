using Terraria;
using Terraria.ModLoader;
using System;

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
        }
    

    public class NoEnemySpawns : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            // High spawn rate stops spawning
            spawnRate = int.MaxValue;
            maxSpawns = 0;
        }
    }
}
}

