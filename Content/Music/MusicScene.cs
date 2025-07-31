using Terraria;
using Terraria.ModLoader;

namespace CTG2.Content.ClientSide;

public class CTGMusicScene : ModSceneEffect
{
    public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

    // This method will now print a message to chat once per second.
    public override bool IsSceneEffectActive(Player player)
    {
        // Use a timer (the game's update count) to avoid spamming the chat.
        if (Main.GameUpdateCount % 60 == 0)
        {
            Main.NewText("DEBUG: CTGMusicScene is Active!");
        }
        return true;
    }

    // This property will now print the Music ID it finds.
    public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/clashroyaleOT");
    // public override int Music
    // {
    //     get
    //     {
    //         int musicId = MusicLoader.GetMusicSlot(mod, "Assets/Music/clashroyaleOT");

    //         if (Main.GameUpdateCount % 60 == 0)
    //         {
    //             Main.NewText($"DEBUG: Trying to play Music ID: {musicId}");
    //         }
    //         return musicId;
    //     }
    // }
}