using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace CTG2.Content.Classes
{
    public class RespawnTime : ModPlayer
    {
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            var modPlayer = Player.GetModPlayer<ClassSystem>(); 
            
            if (modPlayer.playerClass == 1) //Archer Class
            {
                Player.respawnTimer = 60; // 1-second respawn time for Archers
            }
            else
            {
                Player.respawnTimer = 180; // Default 3-second respawn time for other classes
            }
        }

        public override void OnRespawn()
        {
            var modPlayer = Player.GetModPlayer<ClassSystem>();

            if (modPlayer.playerClass == 1) 
            {
                Player.AddBuff(BuffID.Ironskin, 600); 
                Player.AddBuff(BuffID.Webbed, 180);
            }
        }
    }
}
