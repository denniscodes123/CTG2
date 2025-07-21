using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ClassesNamespace;
using CTG2.Content.ServerSide;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics.Tracing;


namespace CTG2.Content.ClientSide;

public class PlayerManager : ModPlayer
{
    public enum PlayerState
    {
        None, ClassSelection, Active, Spectator
    }
    public bool ShowClassUI = false;
    public bool ShowGameUI = false;

    public static int previousMatchStage = 0;
    public int customRespawnTimer = -1;
    public bool awaitingRespawn = false;
    public ClassConfig currentClass = new ClassConfig();
    public UpgradeConfig currentUpgrade = new UpgradeConfig();

    public PlayerState playerState = PlayerState.None; // UPDATE THIS EVERY STATE TRANSITION 
    public double classSelectionTimer = -1;
    public bool isGameStartClassSelection = false; // Track if this is game start vs mid-game class selection
    public int team = 0; // TODO: THIS NEEDS TO BE UPDATED IN TEAMSET 
    GemDrawLayer gemLayer = null;

    public static PlayerManager GetPlayerManager(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < Main.player.Length && Main.player[playerIndex] != null)
        {
            return Main.player[playerIndex].GetModPlayer<PlayerManager>();
        }
        return null;
    }

    public void SetTeam(int newTeam)
    {
        this.team = newTeam;
        Player.team = newTeam;
    }
    //change player state
    public void changePlayerState(PlayerState playerState)
    {
        this.playerState = playerState;

        // Handle UI changes when state changes
        if (playerState == PlayerState.Active)
        {
            ShowClassUI = false;
            ShowGameUI = true;
            //Main.NewText("PlayerManager: Set ShowClassUI to false due to Active state", Microsoft.Xna.Framework.Color.Purple);
        }
        else if (playerState == PlayerState.ClassSelection)
        {
            ShowClassUI = true;
            ShowGameUI = true;
            //Main.NewText("PlayerManager: Set ShowClassUI to true due to ClassSelection state", Microsoft.Xna.Framework.Color.Purple);
        }
        else if (playerState == PlayerState.None || playerState == PlayerState.Spectator)
        {
            ShowClassUI = false;
            ShowGameUI = true;
            // Reset class selection timer when entering None state
            classSelectionTimer = -1;
            isGameStartClassSelection = false;
        }

        Main.NewText($"PlayerManager: Changed state to {playerState}", Microsoft.Xna.Framework.Color.Purple);
    }
    public static void setPlayerClassSelectionTime(int playerIndex, double val)
    {
        PlayerManager player = PlayerManager.GetPlayerManager(playerIndex);
        player.classSelectionTimer = val;
        Main.NewText($"PlayerManager: Server set class selection timer to {val} for player {playerIndex}", Microsoft.Xna.Framework.Color.LightBlue);
    }
    // =========================== OVERRIDE METHODS ====================================
    // Set Custom Respawn Times
    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        var modPlayer = Player.GetModPlayer<ClassSystem>();

        if (pvp)
        {
            Player killer = Main.player[damageSource.SourcePlayerIndex];
            var killerManager = killer.GetModPlayer<PlayerManager>();

            if (killerManager.currentClass.Name == "Gladiator")
            {
                killer.HealEffect(20, true);
            }
        }

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
        int spectatorSpawnY = 11000 / 32;
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
        EnforceTeamLock(); // lock team
        // wecan probably delete (state transitions handle all of this)
        if (this.playerState == PlayerState.ClassSelection)
        {
            ShowClassUI = true;
            if (!Player.hostile) //this is client side only btw should be fine but might need to be synced later
            {
                Player.hostile = true;
            }
        }
        else if (this.playerState == PlayerState.Active)
        {
            ShowClassUI = false;
            if (!Player.hostile)
            {
                Player.hostile = true;
            }
        }
        else
        {
            // For None or Spectator states, keep UI hidden
            ShowClassUI = false;
        }

        if (classSelectionTimer > 0 && !isGameStartClassSelection)
        { //custom 
            classSelectionTimer--;
        }
        else if (classSelectionTimer == 0 && !isGameStartClassSelection) // class selection time expired
        {
            // END CLASS SELECTION 
            var mod = ModContent.GetInstance<CTG2>();
            ModPacket statusPacket = mod.GetPacket();
            statusPacket.Write((byte)MessageType.ExitClassSelection);
            statusPacket.Write(Player.whoAmI);  // Use Player.whoAmI instead of player
            statusPacket.Send();

            Main.NewText($"PlayerManager: Sent ExitClassSelection packet for player {Player.whoAmI}", Microsoft.Xna.Framework.Color.Purple);

            // Reset timer so this doesn't fire again
            classSelectionTimer = -1;
        }
        else
        {
            // DO NOTHING - either timer is not active, or this is a game start class selection handled by server
            if (classSelectionTimer > 0 && isGameStartClassSelection)
            {

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

    //Below is code to make ghost not get hit 
    public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable) //Try to intercept damage before its done
    {
        if (Player.ghost)
        {
            return true;
        }

        return false;
    }


    public override void OnHurt(Player.HurtInfo info) //Fallback in case immuneto doesnt work
    {
        if (Player.ghost)
        {
            info.Damage = 0;
            info.Knockback = 0f;
            info.HitDirection = 0;
            info.DamageSource = default;
        }
    }

    // When a player disconnects, this hook can clean up their data.
    public override void PlayerDisconnect()
    {

    }

    // Packet handlers for client-side state updates
    public void HandleEnterClassSelection(bool gameStarted)
    {
        changePlayerState(PlayerState.ClassSelection);
        isGameStartClassSelection = gameStarted;

        if (!gameStarted)
        {
            classSelectionTimer = 1800;
            Main.NewText($"PlayerManager: Started client-controlled class selection timer (1800)", Microsoft.Xna.Framework.Color.Green);
        }
        else
        {
            classSelectionTimer = 1800;
            Main.NewText($"PlayerManager: Started server-controlled class selection timer (1800)", Microsoft.Xna.Framework.Color.Orange);
        }
    }

    public void HandleExitClassSelection()
    {
        changePlayerState(PlayerState.Active);
        classSelectionTimer = -1;
        isGameStartClassSelection = false;
        Main.NewText($"PlayerManager: Exited class selection", Microsoft.Xna.Framework.Color.Green);
    }
    public void LockTeam()
    {
        var gameManager = ModContent.GetInstance<GameManager>();

        if (gameManager != null && gameManager.IsGameActive)
        {
            int blueTeamId = 3;
            int redTeamId = 1;
            int oldteam = Player.team;

            if (Player.team != blueTeamId && Player.team != redTeamId)
            {

                Player.team = 0;
                this.team = 0;
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, Player.whoAmI, 0);
                }
            }
            else if ((GameInfo.matchStage == 2 || GameInfo.matchStage == 1) && !Player.ghost)
            {
                int currentTeam = Player.team;
                Player.team = currentTeam;
                this.team = currentTeam;

                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, Player.whoAmI, currentTeam);
                }
            }
        }
    }

        public override void Initialize()
        {
            // We only need to create the layer for the client, not the server.
            if (!Main.dedServ)
            {
                gemLayer = new GemDrawLayer();
            }
        }

        public override void ModifyDrawLayerOrdering(IDictionary<PlayerDrawLayer, PlayerDrawLayer.Position> positions)
        {
            if (gemLayer == null)
            {
                return;
            }
            if (gemLayer != null)
            {
                positions[gemLayer] = gemLayer.GetDefaultPosition();
            }
        }

    private void EnforceTeamLock()
    {
        LockTeam();
    }
}
