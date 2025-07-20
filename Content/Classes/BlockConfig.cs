using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CTG2.Content.ClientSide;
namespace CTG2.Content.Classes;

public class BlockRewardSystem : ModSystem
{
    private double blockTimer = 0;
    public static bool canBeDropped;
    public override void PostUpdatePlayers()
    {
        blockTimer += 1.0 / 60.0;
        ;

        if (blockTimer < 45.0)
            return;

        blockTimer = 0;

        if (GameInfo.matchStage != 2)
            return;

        canBeDropped = true;
        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player player = Main.player[i];
            if (!player.active || player.dead || player.team == 0)
                continue;


        if (Main.netMode != NetmodeID.Server && i == Main.myPlayer)
        {
            player.QuickSpawnItem(null, ItemID.DirtBlock, 50);
        }
            canBeDropped = false;
        }
    }
    
}
