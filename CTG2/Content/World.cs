using Terraria;
using Terraria.ModLoader;

namespace CTG2.Content
{
    public class World : ModSystem
    {
        public override void PreUpdateWorld()
        {
            // Set time to always be daytime (Noon)
            Main.dayTime = true;
            Main.time = 27000; // 12:00 PM (Midday)
            Main.eclipse = false; // No eclipses
            Main.bloodMoon = false; // No blood moons
        }
    }

    public class NoEnemySpawns : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            // Set spawn rate to a ridiculously high number (effectively stops spawning)
            spawnRate = int.MaxValue;
            maxSpawns = 0;
        }
    }
}
