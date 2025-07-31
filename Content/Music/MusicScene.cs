
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content
{
// Shows setting up two basic biomes. For a more complicated example, please request.
public class CTG2Biome : ModBiome
{
// Select all the scenery


// Select Music
public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/clashroyaleOT");
        /* If you need the music choice to be conditional, such as supporting the Otherworld soundtrack toggle, you can use this approach:
        public override int Music {
        get {
        if (!Main.swapMusic == Main.drunkWorld && !Main.remixWorld) {
        return MusicID.OtherworldlyEerie;
        }
        return MusicLoader.GetMusicSlot(Mod, "Assets/Music/MysteriousMystery");
        }
        }
        */



        // Populate the Bestiary Filter


        // Calculate when the biome is active.

        public override bool IsBiomeActive(Player player)
        {
            // First, we will use the exampleBlockCount from our added ModSystem for our first custom condition
            return true;
        }

// Declare biome priority. The default is BiomeLow so this is only necessary if it needs a higher priority.
public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
}
}