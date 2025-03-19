using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework; 

namespace CTG2.Content
{
    public class SpawnPoints : ModPlayer
    {

        private static readonly Vector2 RedTeamSpawn = new Vector2(109, 216);
        private static readonly Vector2 BlueTeamSpawn = new Vector2(13, 216);

        public static void TeleportToTeamSpawn(Player player)
        {
            if (player.team == 1) // Red Team
            {
                player.Teleport(RedTeamSpawn * 16, 1); 
            }
            else if (player.team == 3) // Blue Team
            {
                player.Teleport(BlueTeamSpawn * 16, 1);
            }
        }


        public static void TeleportAllPlayersToSpawn()
        {
            foreach (Player player in Main.player)
            {
                if (player.active)
                {
                    TeleportToTeamSpawn(player);
                }
            }
        }

        public override void OnRespawn()
        {
 
            if (Player.team == 1) // Red Team
            {
                Player.SpawnX = (int)RedTeamSpawn.X;
                Player.SpawnY = (int)RedTeamSpawn.Y;
            }
            else if (Player.team == 3) // Blue Team
            {
                Player.SpawnX = (int)BlueTeamSpawn.X;
                Player.SpawnY = (int)BlueTeamSpawn.Y;
            }
        }
    }
}
