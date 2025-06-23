using System;
using System.IO;
using System.Text.Json;
using ClassesNamespace;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CTG2.Content.ClientSide;

public class PlayerManager : ModPlayer
{   
    public static bool ShowClassUI = false;
    public static bool ShowGameUI = false;
    
    // Set Custom Respawn Times
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
    
    // Set Custom Spawn Points
    public override void OnRespawn()
    {   
        int blueBaseX = 12346 / 16;
        int blueBaseY = 10940 / 16;
        int redBaseX = 20385 / 16;
        int redBaseY = 10940 / 16;
        switch (GameInfo.matchStage)
        { 
            case 0:
                break;
            case 1:
                if (Player.team == 3)
                {
                    Player.SpawnX = blueBaseX;
                    Player.SpawnY = blueBaseY;
                }
                else
                {
                    Player.SpawnX = redBaseX;
                    Player.SpawnY = redBaseY;
                }
                break;
            case 2:
                if (Player.team == 3)
                {
                    Player.SpawnX = blueBaseX;
                    Player.SpawnY = blueBaseY;
                }
                else
                {
                    Player.SpawnX = redBaseX;
                    Player.SpawnY = redBaseY;
                }
                break;
        }
    }

    public override void PreUpdate()
    {
        if (GameInfo.matchStage == 1)
        {
            if (GameInfo.matchTime == 1795)
            {   
                var player = Main.LocalPlayer.GetModPlayer<ClassSystem>();
                
                if (player.playerClass == GameClass.None)
                {
                    Main.NewText($"You did not select a class! Assigning a random class.");
                    var classes = Enum.GetValues(typeof(GameClass));
                    player.playerClass = (GameClass)CTG2.randomGenerator.Next(1, classes.Length);
                    player.ResetEffects();
                }
            }
        }

    }

    // When a player disconnects, this hook can clean up their data.
    public override void PlayerDisconnect()
    {

    }
}