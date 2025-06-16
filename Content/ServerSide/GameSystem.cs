using Terraria;
using Terraria.ModLoader;

namespace CTG2.Content.ServerSide;

public class GameSystem : ModSystem
{
    public override void PostUpdateWorld()
    {
        if (!Main.dedServ) return;
        if (!CTG2.GameManager.IsGameActive) return;
        
        CTG2.GameManager.UpdateGame();
        
        base.PostUpdateWorld();
    }
}