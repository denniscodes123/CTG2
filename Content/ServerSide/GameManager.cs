﻿using System;
using Terraria;
using Terraria.ModLoader;
using CTG2.Content;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using System.Collections;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Chat;
using Terraria.Localization;

namespace CTG2.Content.ServerSide;

public class GameManager : ModSystem
{
    // True when 
    public bool IsGameActive { get; private set; }
    public bool HasRoundStarted { get; private set; }

    public int MatchTime { get; private set; }

    public GameTeam BlueTeam { get; private set; }
    public GameTeam RedTeam { get; private set; }

    public Gem BlueGem { get; private set; }
    public Gem RedGem { get; private set; }

    public Queue<MapTypes> mapQueue = new();
    
    public GameMap Map { get; private set; }

    // Spectator tracking
    private Dictionary<int, bool> playerSpectatorStatus = new Dictionary<int, bool>();
    private Dictionary<int, int> spectatorOriginalTeams = new Dictionary<int, int>();
    private Vector2 spectatorSpawnPoint = new Vector2((13332 + 19316) / 2, 11000); // Center area for spectators

    public override void OnWorldLoad()
    {
        // TODO: Re-Paste the Arena on world load (in case it gets destroyed by an admin).

        BlueGem = new Gem(new Vector2(CTG2.config.BlueGem[0], CTG2.config.BlueGem[1]));
        RedGem = new Gem(new Vector2(CTG2.config.RedGem[0], CTG2.config.RedGem[1]));

        BlueTeam = new GameTeam(new Vector2(CTG2.config.BlueSelect[0], CTG2.config.BlueSelect[1]), new Vector2(CTG2.config.BlueBase[0], CTG2.config.BlueBase[1]), 3);
        RedTeam = new GameTeam(new Vector2(CTG2.config.RedSelect[0], CTG2.config.RedSelect[1]), new Vector2(CTG2.config.RedBase[0], CTG2.config.RedBase[1]), 1);
        
        // map paste takes Tile coords. spawn point takes pixel coords.
        Map = new GameMap(CTG2.config.MapPaste[0], CTG2.config.MapPaste[1]);

        IsGameActive = false;
        MatchTime = 0;
    }

    public void StartGame()
    {
        IsGameActive = true;
        MatchTime = 0;
        BlueGem.Reset();
        RedGem.Reset();
        bool isMapPicked = mapQueue.TryDequeue(out MapTypes result);
        if (isMapPicked)
        {
            Map.LoadMap(result);
        }
        else
        {
            Map.LoadMap((MapTypes)CTG2.randomGenerator.Next(0, 7));
        }
        

        // TODO: send all players to class selection 
        BlueTeam.UpdateTeam();
        RedTeam.UpdateTeam();

        // Auto-spectate players not on teams (team 0)
        foreach (Player player in Main.player)
        {
            if (player.active && player.team == 0)
            {
                SetPlayerSpectator(player.whoAmI, true);
                Console.WriteLine($"Auto-spectating player {player.name} (not on a team)");
            }
        }





        BlueTeam.StartMatch();
        RedTeam.StartMatch();

    }

    // Pauses/Unpauses game
    public void PauseGame()
    {
        MatchTime += 900;
        BlueTeam.PauseTeam();
        RedTeam.PauseTeam();
    }

    public void EndGame()
    {   
        
        IsGameActive = false;
   
        foreach (var kvp in playerSpectatorStatus)
        {
            int playerIndex = kvp.Key;
            if (Main.player[playerIndex].active && kvp.Value) 
            {
                Main.player[playerIndex].ghost = false;
                Main.player[playerIndex].respawnTimer = 0;
                
              
                var mod_ = ModContent.GetInstance<CTG2>();
                ModPacket statusPacket = mod_.GetPacket();
                statusPacket.Write((byte)MessageType.ServerSpectatorUpdate);
                statusPacket.Write(playerIndex);
                statusPacket.Write(false); 
                statusPacket.Send(toClient: playerIndex);
            }
        }
        
        playerSpectatorStatus.Clear();
        spectatorOriginalTeams.Clear();
        
        Console.WriteLine("Cleared all spectator tracking on game end");
        
        var mod = ModContent.GetInstance<CTG2>();
        ModPacket packet = mod.GetPacket();
        packet.Write((byte)MessageType.ServerGameEnd);
        packet.Send();
        // TODO: Return all players to spectate area, clear inventories, etc. 
    }

    // Runs every frame while game running. Runs all gem checks, draws timer and gem status.
    public void UpdateGame()
    {
        // TODO: Check if each player has completed class selection (no == class select, yes == send to match)
        
        // force set team/pvp
        BlueTeam.EnforceTeam();
        RedTeam.EnforceTeam();
        
        if (MatchTime == 1800)
        {

            // TODO: APPLY END CLASS SELECTION METHOD FOR ALL players 
            // DO THIS BY ITERATING OVER ALL PLAYERS AND SEND RELEVANT PACKETS

            BlueTeam.SendToBase();
            RedTeam.SendToBase();
        }
        // Increase match duration by 1 tick
        MatchTime++;

        if (MatchTime >= 1800)
        {
            // Updates holding/capturing status of both gems.
            BlueGem.Update(RedGem, RedTeam.Players);
            RedGem.Update(BlueGem, BlueTeam.Players);
        }

        // Send updated GameInfo to clients every 6 ticks (every 0.1s)
        if (MatchTime % 6 == 0)
        {
            var mod = ModContent.GetInstance<CTG2>();
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MessageType.ServerGameUpdate);
            if (MatchTime >= 1800)
            {
                packet.Write((int)2);
            }
            else
            {
                packet.Write((int)1);
            }

            packet.Write((int)MatchTime);
            // Blue and red Gem X positions
            var distBetweenGems = Math.Abs(RedGem.Position.X - BlueGem.Position.X);
            if (BlueGem.IsHeld)
            {
                // send % of the way the gem is to enemy base as an integer
                var distFromGem = Main.player[BlueGem.HeldBy].position.X - BlueGem.Position.X;
                var intPercentage = (int)Math.Round(distFromGem / distBetweenGems * 100);
                intPercentage = Math.Clamp(intPercentage, 0, 100);
                packet.Write(intPercentage);
            }
            else packet.Write((int)0);

            if (RedGem.IsHeld)
            {
                // send % of the way the gem is to enemy base as an integer
                // dist from gem is negative for red
                var distFromGem = -(Main.player[RedGem.HeldBy].position.X - RedGem.Position.X);
                var intPercentage = (int)Math.Round(distFromGem / distBetweenGems * 100);
                intPercentage = Math.Clamp(intPercentage, 0, 100);
                packet.Write(intPercentage);
            }
            else packet.Write((int)0);

            // Blue and red gem holders
            if (BlueGem.IsHeld) packet.Write(Main.player[BlueGem.HeldBy].name);
            else packet.Write("At Base");
            if (RedGem.IsHeld) packet.Write(Main.player[RedGem.HeldBy].name);
            else packet.Write("At Base");
            packet.Send();
        }

        if (BlueGem.IsCaptured)
        {
            Console.WriteLine("Blue gem captured!");
            EndGame();
        }

        else if (RedGem.IsCaptured)
        {
            Console.WriteLine("Red gem captured!");
            EndGame();
        }

        // if Match time exceeds a certain point, end the match
        if (MatchTime >= 60 * 60 * 15)
        {
            EndGame();
        }
        // TODO: 
    }

    public void queueMap(MapTypes mapType) { // idk what to do here i have a Queue<MapType> here
        mapQueue.Enqueue(mapType);
    }

    public bool IsPlayerSpectator(int playerIndex)
    {
        return playerSpectatorStatus.GetValueOrDefault(playerIndex, false);
    }
    
    public void SetPlayerSpectator(int playerIndex, bool isSpectator)
    {
        if (!Main.player[playerIndex].active) return;
        
        var player = Main.player[playerIndex];
        
        if (isSpectator)
        {
            // Store original team for later purposes
            if (player.team != 0)
            {
                spectatorOriginalTeams[playerIndex] = player.team;
            }
            if (BlueGem.IsHeld && BlueGem.HeldBy == playerIndex)
            {
                BlueGem.Reset();
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{player.name} dropped the Blue Gem when entering spectator mode"), Microsoft.Xna.Framework.Color.Blue);
                Console.WriteLine($"Player {player.name} dropped Blue Gem when entering spectator mode");
            }
            
            if (RedGem.IsHeld && RedGem.HeldBy == playerIndex)
            {
                RedGem.Reset();
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{player.name} dropped the Red Gem when entering spectator mode"), Microsoft.Xna.Framework.Color.Red);
                Console.WriteLine($"Player {player.name} dropped Red Gem when entering spectator mode");
            }
            // Set spectator status
            playerSpectatorStatus[playerIndex] = true;
            // UPDATE PLAYER STATE INDEX             
            // Teleport to spectator area
            
            // Send teleport packet to client
            var mod = ModContent.GetInstance<CTG2>();
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MessageType.ServerTeleport);
            packet.Write(playerIndex);
            packet.Write((int)spectatorSpawnPoint.X);
            packet.Write((int)spectatorSpawnPoint.Y);
            packet.Send(toClient: playerIndex);
            
            // Send spectator status update
            ModPacket statusPacket = mod.GetPacket();
            statusPacket.Write((byte)MessageType.ServerSpectatorUpdate);
            statusPacket.Write(playerIndex);
            statusPacket.Write(true); // is spectator
            statusPacket.Send(toClient: playerIndex);
            
            // DEBUG: Global broadcast
            Console.WriteLine($"Player {player.name} entered spectator mode");
        }
        else
        {
            // DEBUG: Global broadcast start
            
            // Check if player has original team
            if (!spectatorOriginalTeams.TryGetValue(playerIndex, out int originalTeam) || originalTeam == 0 || player.ghost != true)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[DEBUG] {player.name} CANNOT EXIT SPECTATOR"), Microsoft.Xna.Framework.Color.LimeGreen);
                Console.WriteLine($"Player {player.name} cannot exit spectator mode - no team assigned (originalTeam: {originalTeam})");
                return;
            }
            if (60 * 15 * 60 - MatchTime < 45 * 60)
            {
                // Player is cooked 
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[DEBUG] {player.name} TOO LATE GG"), Microsoft.Xna.Framework.Color.LimeGreen);
                
                Console.WriteLine($"Player {player.name} cannot exit spectator mode - game is about to end soon");
                return; 
            }

            // UPDATE PLAYER STATE HERE 
            
            playerSpectatorStatus[playerIndex] = false;
            player.team = originalTeam;

            var mod = ModContent.GetInstance<CTG2>();
            NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, playerIndex, originalTeam);

            
            // THIS CODE IS CORRECT ONCE METHODS ARE FILLED IN 
            // ModPacket statusPacket = mod.GetPacket();
            // statusPacket.Write((byte)MessageType.EnterClassSelection);
            // statusPacket.Write(playerIndex);
            // statusPacket.Write(originalTeam);
            // statusPacket.Write(false); // game has already started 
            // statusPacket.Send(toClient: playerIndex);

            // ModPacket statusPacket = mod.GetPacket();
            // statusPacket.Write((byte)MessageType.ServerSpectatorUpdate);
            // statusPacket.Write(playerIndex);
            // statusPacket.Write(false); // not spectator
            // statusPacket.Send(toClient: playerIndex);


            // CODE BELOW ALL NEEDS TO BE DELETED ONCE CLASS IS FINISHED
            Vector2 teleportLocation;
            if (MatchTime < 1800) // Class selection phase (first 30 seconds)
            {

                // Teleport to class selection area
                teleportLocation = originalTeam == 3 ?
                    new Vector2(12346, 10940) : // Blue team class location
                    new Vector2(20385, 10940);  // Red team class location
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[DEBUG] {player.name} in class selection"), Microsoft.Xna.Framework.Color.LimeGreen);
                Console.WriteLine($"Player {player.name} joined class selection with {(1800 - MatchTime) / 60} seconds remaining");
            }
            else // Active game phase
            {
                // Teleport to team base
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[DEBUG] {player.name} should be in game?"), Microsoft.Xna.Framework.Color.LimeGreen);
                teleportLocation = originalTeam == 3 ?
                    new Vector2(12346, 10940) : // Blue team base (same as class location)
                    new Vector2(20385, 10940);  // Red team base (same as class location)


                Console.WriteLine($"Player {player.name} joined active game");
            }

            
            // Always teleport the player
            player.Teleport(teleportLocation);

            ModPacket teleportPacket = mod.GetPacket();
            teleportPacket.Write((byte)MessageType.ServerTeleport);
            teleportPacket.Write(playerIndex);
            teleportPacket.Write((int)teleportLocation.X);
            teleportPacket.Write((int)teleportLocation.Y);
            teleportPacket.Send(toClient: playerIndex);

            // Send spectator status update
            ModPacket statusPacket = mod.GetPacket();
            statusPacket.Write((byte)MessageType.ServerSpectatorUpdate);
            statusPacket.Write(playerIndex);
            statusPacket.Write(false); // not spectator
            statusPacket.Send(toClient: playerIndex);

            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[DEBUG] {player.name} SUCCESSFULLY exited spectator mode!"), Microsoft.Xna.Framework.Color.LimeGreen);
            //NetMessage.BroadcastChatMessage(NetworkText.FromLiteral($"[DEBUG] {player.name} SUCCESSFULLY exited spectator mode!"), Microsoft.Xna.Framework.Color.LimeGreen);
            Console.WriteLine($"Player {player.name} exited spectator mode and rejoined team {originalTeam}");
        }
    }


    public void startPlayerClassSelection(int playerIndex, int team, bool gameStarted)
    {
        // TODO (fill in this function):

        // set player state index (in PlayerManagement in ClientSide) to 1

        if (gameStarted)
        {
            // teleport player to correct location

        }
        else
        {
            // here we need to update Player class selection timer 
            // 
        }
    }
    public void endPlayerClassSelection(int playerIndex, int team)
    {
        // TODO (fill in this function)
        // set player state index to 2
        //  update plyaer class selection timer to -1
        // teleport 
        // update 
    }
    public override void PostUpdateWorld()
    {
        if (!Main.dedServ) return;
        if (!IsGameActive) return;

        UpdateGame();

        base.PostUpdateWorld();
    }
    


}