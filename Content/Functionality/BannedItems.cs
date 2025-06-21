using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CTG2.Content.Functionality
{
    public class BannedItems : ModSystem
    {
        //Lowkey forgot theres no wood so can't craft anyways but can be helpful for later        
        private static readonly HashSet<int> BannedItemIDs = new()
        {
            ItemID.WoodenHammer,
            ItemID.DirtBomb,
            
            
        };

        public override void PostUpdatePlayers()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (!player.active)
                    continue;

                foreach (Item item in player.inventory)
                {
                    if (item != null && !item.IsAir && BannedItemIDs.Contains(item.type))
                    {
                        
                        player.AddBuff(BuffID.Webbed, 2); 

                        // Once every 3 seconds 
                        if (player.whoAmI == Main.myPlayer && Main.GameUpdateCount % 180 == 0)
                        {
                            Main.NewText($"{item.Name} is banned. Remove it immediately.", Color.Red);
                        }

                        break; 
                    }
                }
            }
        }
    }
}
