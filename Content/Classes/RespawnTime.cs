using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using System;
using ClassesNamespace;
using CTG2.Content.ClientSide;


namespace CTG2.Content.Classes
{
    public class RespawnTime : ModPlayer
    {
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            var modPlayer = Player.GetModPlayer<ClassSystem>();

            // How much time has passed since match started
            int timeElapsed = GameInfo.matchTime/60 - 30;
            int extraSeconds = Math.Max(0, timeElapsed / 120); // +1s for every 2 minutes

            switch (modPlayer.playerClass)
            {
                case GameClass.Archer: // Archer
                    Player.respawnTimer = (3 + extraSeconds) * 60;
                    break;

                case GameClass.Ninja: 
                    Player.respawnTimer = (1 + extraSeconds) * 60;
                    break;

                case GameClass.Beast: 
                    Player.respawnTimer = (2 + extraSeconds) * 60;
                    break;

                default:
                    Player.respawnTimer = (3 + extraSeconds) * 60; 
                    break;
            }
        }
    }
}
