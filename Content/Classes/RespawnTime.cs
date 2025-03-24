using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using System;

namespace CTG2.Content.Classes
{
    public class RespawnTime : ModPlayer
    {
public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
{
    var modPlayer = Player.GetModPlayer<ClassSystem>();

    // How much time has passed since match started
    int timeElapsed = (int)(Main.GameUpdateCount / 60) - GameUI.matchTimer + 15;
    int extraSeconds = Math.Max(0, timeElapsed / 120); // +1s for every 2 minutes

    switch (modPlayer.playerClass)
    {
        case 1: // Archer
            Player.respawnTimer = (3 + extraSeconds) * 60;
            break;

        case 2: 
            Player.respawnTimer = (1 + extraSeconds) * 60;
            break;

        case 3: 
            Player.respawnTimer = (2 + extraSeconds) * 60;
            break;

        default:
            Player.respawnTimer = (3 + extraSeconds) * 60; 
            break;
    }
}
    }
}