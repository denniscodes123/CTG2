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
    public int customRespawnTimer = -1;
    public bool awaitingRespawn = false;


    // Set Custom Respawn Times
    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        var modPlayer = Player.GetModPlayer<ClassSystem>();

        if (GameInfo.matchStage == 2)
        {
            awaitingRespawn = true;
            Player.ghost = true;
            Player.dead = true;
        }

        // How much time has passed since match started
        int timeElapsed = GameInfo.matchTime / 60 - 30;
        int extraSeconds = Math.Max(0, timeElapsed / 120); // +1s for every 2 minutes

        Player.respawnTimer = 0;
        switch (modPlayer.playerClass)
        {
            case GameClass.Archer: // Archer
                customRespawnTimer = (3 + extraSeconds) * 60;
                break;

            case GameClass.Ninja:
                customRespawnTimer = (1 + extraSeconds) * 60;
                break;

            case GameClass.Beast:
                customRespawnTimer = (2 + extraSeconds) * 60;
                break;

            default:
                customRespawnTimer = (3 + extraSeconds) * 60;
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
            if (awaitingRespawn) //was lowkey angry while coding this will clean up later
    {
        customRespawnTimer--;

        if (customRespawnTimer <= 0)
        {
            awaitingRespawn = false;

            Player.ghost = false;
            Player.dead = true;

            Player.statLife = Player.statLifeMax2;
            Player.HealEffect(Player.statLifeMax2);

            
            
        }
    }



    }

    // When a player disconnects, this hook can clean up their data.
    public override void PlayerDisconnect()
    {

    }
}
