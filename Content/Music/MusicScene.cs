
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;
using CTG2.Content.ServerSide;
using CTG2.Content.Configs;


namespace CTG2.Content
{
    // Shows setting up two basic biomes. For a more complicated example, please request.
    public class CTG2Biome : ModBiome
    {
        public override int Music
        {
            get
            {
                var cfg = ModContent.GetInstance<CTG2Config>(); //reads from config
                if (!cfg.ClashRoyaleOTMusic)
                    return -1; // this makes no ovveride

                return MusicLoader.GetMusicSlot(Mod, "Assets/Music/clashroyaleOT");
            }
        }

        public override bool IsBiomeActive(Player player)
            => GameInfo.overtime; //make biome only active during overtime
            //todo: Make sure isovertime is synced clientside

        public override SceneEffectPriority Priority
            => SceneEffectPriority.BiomeHigh;
    }
}
