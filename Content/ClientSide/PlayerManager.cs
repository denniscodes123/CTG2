using System;
using System.IO;
using System.Text.Json;
using ClassesNamespace;
using CTG2.Content.ServerSide;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content.ClientSide;

public class PlayerManager : ModPlayer
{   
    public enum PlayerState
{
    None, ClassSelection, Active, Spectator
}
    public static bool ShowClassUI = false;
    public static bool ShowGameUI = false;
    public static int previousMatchStage = 0;
    public int customRespawnTimer = -1;
    public bool awaitingRespawn = false;
    public static ClassConfig currentClass = new ClassConfig();
    public static UpgradeConfig currentUpgrade = new UpgradeConfig();

    public PlayerState playerState = PlayerState.None; // UPDATE THIS EVERY STATE TRANSITION 
    public double classSelectionTimer = -1;
    public int team = 0; // TODO: THIS NEEDS TO BE UPDATED IN TEAMSET 


    //change player state
    public void changePlayerState(PlayerState playerState)
    {
        this.playerState = playerState;
    }

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

        // removed switch, using config-based respawn time.
        customRespawnTimer = (currentClass.RespawnTime + extraSeconds) * 60;
    }
    
    
    // Set Custom Spawn Points
    public override void OnRespawn()
    {

        int blueBaseX = CTG2.config.BlueBase[0] / 16;
        int blueBaseY = CTG2.config.BlueBase[1] / 16;
        int redBaseX = CTG2.config.RedBase[0] / 16;
        int redBaseY = CTG2.config.RedBase[1] / 16;
        int spectatorSpawnX = (13332 + 19316) / 32;
        int spectatorSpawnY =  11000 / 32;
        if (Player.ghost)
        {
            Player.SpawnX = spectatorSpawnX;
            Player.SpawnY = spectatorSpawnY;
            return; 
        }
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
    
    // Lock team/pvp, Enable/disable UI
    public override void PreUpdate()
    {   
        // this needs to update player class selection time
        
        if (GameInfo.matchStage == 1)
        {
            // as soon as class selection starts
            if (previousMatchStage != GameInfo.matchStage)
            {
                ShowClassUI = true;
            }

        }
        else if (GameInfo.matchStage == 2)
        {
            // as soon as match starts
            if (previousMatchStage != GameInfo.matchStage)
            {
                ShowClassUI = false;
            }
        }
        // match stage 0 (no match going on)
        else
        {
            ShowClassUI = false;
        }

        if (customRespawnTimer > 0)
        {
            customRespawnTimer--;
        }
        else if (customRespawnTimer == 0) // or class has been selected TODO 
        {
            // END CLASS SELECTION 
        }
        else
        {
            // DO NOTHING
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
