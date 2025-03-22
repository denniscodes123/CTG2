using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CTG2.Content
{
    public class SpawnPoints : ModPlayer
    {

        private static readonly Point RedTeamSpawn = new Point(1500, 270);  
        private static readonly Point BlueTeamSpawn = new Point(13, 216);

        public static void TeleportToTeamSpawn(Player player)
        {
            if (player.team == 1) // Red Team
            {
                player.Teleport(new Vector2(RedTeamSpawn.X * 16, RedTeamSpawn.Y * 16), 1);
            }
            else if (player.team == 3) // Blue Team
            {
                player.Teleport(new Vector2(BlueTeamSpawn.X * 16, BlueTeamSpawn.Y * 16), 1);
            }
        }

        public static void TeleportAllPlayersToSpawn()
        {
            foreach (Player player in Main.player)
            {
                if (player.active)
                {
                    TeleportToTeamSpawn(player);
                    player.AddBuff(BuffID.Webbed, 30); 
                }
            }
        }

        public override void OnRespawn()
        {
            if (Player.team == 1) // Red Team
            {
                Player.SpawnX = RedTeamSpawn.X;
                Player.SpawnY = RedTeamSpawn.Y;
            }
            else if (Player.team == 3) // Blue Team
            {
                Player.SpawnX = BlueTeamSpawn.X;
                Player.SpawnY = BlueTeamSpawn.Y;
            }
        }
    
    }
}
