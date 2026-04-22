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
        [DefaultValue(false)]
        public bool EnableProjectileTeamColoring;


        [Label("Selected Track: 0=None, 1=Clash, 2=Mystery")]
        public int SelectedMusicIndex { get; set; } = 0;

        [Label("Enable vanilla double-tap dash")]
        [Tooltip("When enabled, the vanilla double-tap dash works as normal. When disabled, only the Dash keybind triggers dashes.")]
        [DefaultValue(true)]
        public bool IsVanillaDashEnabled;

    }
}
