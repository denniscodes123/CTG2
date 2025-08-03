using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace CTG2.Content.Configs
{
    public class CTG2Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Enable clash royale OT music")]
        [DefaultValue(false)]
        public bool ClashRoyaleOTMusic;

        [Label("Enable projectile team coloring")]
        [DefaultValue(true)]
        public bool EnableProjectileTeamColoring;


    }
}
