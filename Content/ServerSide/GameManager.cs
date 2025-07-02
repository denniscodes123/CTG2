using System;
using Terraria;
using Terraria.ModLoader;
using CTG2.Content;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using System.Collections;
using System.Collections.Generic;
using Terraria.ID;

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

        BlueGem = new Gem(new Vector2(13332, 11504));
        RedGem = new Gem(new Vector2(19316, 11504));

        BlueTeam = new GameTeam(new Vector2(12346, 10940), new Vector2(12346, 10940), 3);
        RedTeam = new GameTeam(new Vector2(20385, 10940), new Vector2(20385, 10940), 1);
        
        Map = new GameMap();

        IsGameActive = false;
        MatchTime = 0;
    }

    public void StartGame()
    {
        IsGameActive = true;

        MatchTime = 0;
        BlueGem.Reset();
        RedGem.Reset();

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

        // Map Set

        //TODO: Create MapLoad() function (maybe using a GameMap class?)
        bool isMapPicked = mapQueue.TryDequeue(out MapTypes result);
        if (isMapPicked)
        {
            Map.LoadMap(result);
        }
        else
        {
            Map.LoadMap((MapTypes)CTG2.randomGenerator.Next(0, 7));
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

        if (MatchTime == 1800)
        {
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
            
            // Set spectator status
            playerSpectatorStatus[playerIndex] = true;
            
            // Apply spectator mode
            player.ghost = true;
            player.respawnTimer = 9999;
            player.team = 0; // Remove from teams
            
            // Teleport to spectator area
            player.Teleport(spectatorSpawnPoint);
            
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
            
            Console.WriteLine($"Player {player.name} entered spectator mode");
        }
        else
        {
            // Exit spectator mode
            if (!spectatorOriginalTeams.TryGetValue(playerIndex, out int originalTeam) || originalTeam == 0)
            {
                // Player has no team to return to
                Console.WriteLine($"Player {player.name} cannot exit spectator mode - no team assigned");
                return; 
            }

            if (60 * 15 * 60 - MatchTime > 45 * 60)
            {
                // Player is cooked 
                Console.WriteLine($"Player {player.name} cannot exit spectator mode - game is about to end soon");
                return; 
            }
        
            playerSpectatorStatus[playerIndex] = false;
            
            player.team = originalTeam; // this may need to be changed 

            var mod = ModContent.GetInstance<CTG2>();
            NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, playerIndex, originalTeam);

            player.ghost = false;
            player.respawnTimer = 0;

            if (IsGameActive)
            {
 
                Vector2 classLocation = originalTeam == 3 ? 
                    new Vector2(12346, 10940) : // Blue team class location
                    new Vector2(20385, 10940);  // Red team class location
                
                player.Teleport(classLocation);
                
                ModPacket teleportPacket = mod.GetPacket();
                teleportPacket.Write((byte)MessageType.ServerTeleport);
                teleportPacket.Write(playerIndex);
                teleportPacket.Write((int)classLocation.X);
                teleportPacket.Write((int)classLocation.Y);
                teleportPacket.Send(toClient: playerIndex);
                
                Console.WriteLine($"Player {player.name} joined class selection");
            
          
            }
            

            ModPacket statusPacket = mod.GetPacket();
            statusPacket.Write((byte)MessageType.ServerSpectatorUpdate);
            statusPacket.Write(playerIndex);
            statusPacket.Write(false); // not spectator
            statusPacket.Send(toClient: playerIndex);
            
            Console.WriteLine($"Player {player.name} exited spectator mode and rejoined team {originalTeam}");
        }
    }

    public override void PostUpdateWorld()
    {
        if (!Main.dedServ) return;
        if (!IsGameActive) return;

        UpdateGame();

        base.PostUpdateWorld();
    }
    


}