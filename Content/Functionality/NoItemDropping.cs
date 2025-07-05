using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CTG2.Content.ClientSide;
using CTG2.Content.Classes;
namespace CTG2.Content.Functionality;

public class DropBlockSystem : ModSystem
{


public override void PreUpdateEntities()
    {
        for (int i = 0; i < Main.maxItems; i++)
        {
            Item item = Main.item[i];

if (item.velocity.Y == -2f && item.active && (GameInfo.matchStage==1 || GameInfo.matchStage==2) && BlockRewardSystem.canBeDropped==false) {
    //Dropped items y gets set to -2 (items from /item and breaking blocks don't get turned to air)
    item.TurnToAir();
    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, i);
    if (Main.myPlayer == i)
        Main.NewText("Dropping items is disabled.", Color.Red);
        }
}
        }
    }







