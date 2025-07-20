using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Chat;



namespace CTG2.Content.Functionality
{
    public class NoTombs : ModSystem
    {
        public override void Load()
        {
            Terraria.On_Player.DropTombstone += (orig, self, coins, deathText, hitDir) => { };

            Terraria.On_Player.KillMe += (orig, self, damageSource, dmg, hitDirection, pvp) => {
                // Get killer info
            int killerIndex = damageSource.SourcePlayerIndex;

                // Get victim team and name
            string victimName = self.team == 1 ? $"[c/FF0000:{self.name}]" : 
                                self.team == 3 ? $"[c/0000FF:{self.name}]" :
                                self.name;

                // Check if killer is a player and if so get name and team
            string killerName = "???";
            if (killerIndex >= 0)
            {
                Player killer = Main.player[killerIndex];
                killerName = killer.team == 1 ? $"[c/FF0000:{killer.name}]" : //Red team color (color is a little off rn)
                            killer.team == 3 ? $"[c/0000FF:{killer.name}]" :  //Blue team color (off as well)
                            killer.name;
            }

            // Try to get damage source wep ID if null (most cases for some reason) just use held item like vanilla
            int? itemID = damageSource.SourceItem?.netID;
            if (itemID == null && killerIndex >= 0)
                itemID = Main.player[killerIndex].HeldItem?.netID;

            // Msg to send
            string msg = $"{killerName} ([i:{itemID ?? 0}]) {victimName}";

            // Run on server only and catch null statements caused by desync
                if (Main.netMode == NetmodeID.Server && itemID != 0 && itemID.HasValue)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(msg), Color.White);
                }




            // This makes sure death is still processed normally 
            orig(self, damageSource, dmg, hitDirection, pvp);
};

        }


        
    }

    
}
