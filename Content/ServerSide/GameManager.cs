using System;
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
using CTG2.Content.ClientSide;
using Terraria.Enums;
using ClassesNamespace;

namespace CTG2.Content.ServerSide;

public class GameManager : ModSystem
{
    // True when 
    public bool IsGameActive { get; private set; }
    public bool HasRoundStarted { get; private set; }

    public int MatchTime { get; private set; }

    public GameTeam BlueTeam { get; private set; }
    public GameTeam RedTeam { get; private set; }

    public Dictionary<int, GameTeam> intToTeam;

    public Gem BlueGem { get; private set; }
    public Gem RedGem { get; private set; }

    public Queue<MapTypes> mapQueue = new();

    public bool pause = false;
    public bool killonce = true;
    private bool isOvertime = false;
    private int overtimeTimer = 0;
    private const int OVERTIME_DURATION = 60 * 2 * 60; // 2 minutes in ticks (60 ticks/sec)
    
    public GameMap Map { get; private set; }

    // Spectator tracking
    private Dictionary<int, bool> playerSpectatorStatus = new Dictionary<int, bool>();
    private Dictionary<int, int> spectatorOriginalTeams = new Dictionary<int, int>();
    private Vector2 spectatorSpawnPoint = new Vector2((13332 + 19316) / 2, 11000); // Center area for spectators
    
    // New game delay tracking
    private bool isWaitingForNewGame = false;
    private int newGameTimer = 0;
    private int blueGemFireworkTimer = 0;
    private int redGemFireworkTimer = 0;
    private const int FIREWORK_INTERVAL = 120;
/* might be cooked rn 
    public bool IsPubsModeActive { get; set; }

    public void EnablePubsMode()
    {
        IsPubsModeActive = true;
        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("[GAMEMODE] Pubs mode enabled."), Color.Aqua);

        ModPacket packet = Mod.GetPacket();
        packet.Write((byte)MessageType.SyncPubsMode);
        packet.Write(true);
        packet.Send();

    }
    
        public static void FillLavaInDesignatedArea()
        {
            //hard coded coords for the right.wld if you are on the wrong wld it will spawn lava in wrong spot!!!
            int xMin = 13702 /16; 
            int yMin = 11719 /16;
            int xMax = 19030 /16;
            int yMax = 11814 /16;
    
            for (int x = xMin; x <= xMax; x++)
            {
                for (int y = yMin; y <= yMax; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
    
    
                    if (!tile.HasTile)
                    {
                        tile.LiquidAmount = 255;
                        tile.LiquidType = LiquidID.Lava;
                        Liquid.AddWater(x, y); // Schedule a liquid update at this position
                        WorldGen.SquareTileFrame(x, y, true);
                    }
                }
            }
    
            Liquid.UpdateLiquid();
        }
        public static void FillHoneyForMap(MapTypes map)
        {
            // Define honey zones for specific maps
            List<Rectangle> honeyZones = new();

            switch (map)
            {
                case MapTypes.Kraken:
                    honeyZones.Add(new Rectangle(16357 / 16, 10965 / 16, (16376 - 16357 + 1) / 16, 1));
                    honeyZones.Add(new Rectangle(1022, 685, 2, 1)); 
                    break;

                case MapTypes.Stalactite:
                    honeyZones.Add(new Rectangle(16357 / 16, 10965 / 16, (16376 - 16357 + 1) / 16, 1));
                    honeyZones.Add(new Rectangle(1022, 685, 2, 1)); 
                    break;

                default:
                    return;
            }

            foreach (var zone in honeyZones)
            {
                for (int x = zone.Left; x < zone.Right; x++)
                {
                    for (int y = zone.Top; y < zone.Bottom; y++)
                    {
                        Tile tile = Framing.GetTileSafely(x, y);

                        if (!tile.HasTile)
                        {
                            tile.LiquidAmount = 255;
                            tile.LiquidType = LiquidID.Honey;
                            Liquid.AddWater(x, y);
                            WorldGen.SquareTileFrame(x, y, true);
                        }
                    }
                }
            }

            

            Liquid.UpdateLiquid();
        }
        public static void ClearHoneyForMap()
        {
            List<Rectangle> honeyZones = new();

            honeyZones.Add(new Rectangle(1022, 685, 10, 10));

            foreach (var zone in honeyZones)
            {
                for (int x = zone.Left; x < zone.Right; x++)
                {
                    for (int yPos = zone.Top; yPos < zone.Bottom; yPos++)
                    {
                        Tile tile = Framing.GetTileSafely(x, yPos);

                        if (tile.LiquidAmount > 0 && tile.LiquidType == LiquidID.Honey)
                        {
                            tile.LiquidAmount = 0;
                            tile.LiquidType = 0;
                            Liquid.AddWater(x, yPos);
                            WorldGen.SquareTileFrame(x, yPos, true);
                        }
                    }
                }
            }

            Liquid.UpdateLiquid();
        }

    
    public override void OnWorldLoad()
    {
        // TODO: Re-Paste the Arena on world load (in case it gets destroyed by an admin).
        Main.spawnTileX = 13317 / 16; //spawn coords for the world ONLY on world load (this is changed later)
        Main.spawnTileY = 10855 / 16;

        BlueGem = new Gem(new Vector2(CTG2.config.BlueGem[0], CTG2.config.BlueGem[1]), 3);
        RedGem = new Gem(new Vector2(CTG2.config.RedGem[0], CTG2.config.RedGem[1]), 1);

        BlueTeam = new GameTeam(new Vector2(CTG2.config.BlueSelect[0], CTG2.config.BlueSelect[1]), new Vector2(CTG2.config.BlueBase[0], CTG2.config.BlueBase[1]), 3);
        RedTeam = new GameTeam(new Vector2(CTG2.config.RedSelect[0], CTG2.config.RedSelect[1]), new Vector2(CTG2.config.RedBase[0], CTG2.config.RedBase[1]), 1);

        intToTeam = new Dictionary<int, GameTeam>
        {
            { 3, BlueTeam },
            { 1, RedTeam }
        };

        // map paste takes Tile coords. spawn point takes pixel coords.
        Map = new GameMap(CTG2.config.MapPaste[0], CTG2.config.MapPaste[1]);
        GameMap.PreloadAllMaps();
        IsGameActive = false;
        MatchTime = 0;
    }

    public void StartGame()
    {
        isWaitingForNewGame = false;
        IsGameActive = true;
        MatchTime = 0;
        BlueGem.Reset();
        RedGem.Reset();
        bool isMapPicked = mapQueue.TryDequeue(out MapTypes result);

                /*
        if (IsPubsModeActive)
        {
            List<Player> unassigned = new List<Player>();
            foreach (Player p in Main.player)
            {
                if (p != null && p.active)
                    unassigned.Add(p);
            }

            unassigned.Shuffle(CTG2.randomGenerator);

            for (int i = 0; i < unassigned.Count; i++)
            {
                Player plyrr = unassigned[i];
                int team = (i % 2 == 0) ? 1 : 3; // Alternate Red (1) and Blue (3)

                plyrr.team = team;
                var mod1 = ModContent.GetInstance<CTG2>();
                ModPacket SetTeamPacket = mod1.GetPacket();
                SetTeamPacket.Write((byte)MessageType.UpdatePlayerTeam);
                SetTeamPacket.Write(plyrr.whoAmI);
                SetTeamPacket.Write(team);
                SetTeamPacket.Send();
            }
        }
        */

        BlueTeam.UpdateTeam();
        RedTeam.UpdateTeam();
        foreach (Player player in Main.player)
        {
            if (!player.active) continue;

            var mod = ModContent.GetInstance<CTG2>();
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MessageType.ServerGameStart);
            packet.Send(toClient: player.whoAmI);

            if (player.team == 0)
            {
                SetPlayerSpectator(player.whoAmI, true);
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{player.name} is not on a team!"), Microsoft.Xna.Framework.Color.Olive);
                Console.WriteLine($"Auto-spectating player {player.name} (not on a team)");
                continue; // Use continue instead of return to handle all players
            }

            // Directly call startPlayerClassSelection instead of sending a packet
    
            startPlayerClassSelection(player.whoAmI, true);

        }

        if (isMapPicked)
        {
            ClearHoneyForMap();
            Map.LoadMap(result);
            if (result == MapTypes.Kraken || result == MapTypes.Stalactite)
            {
                FillHoneyForMap(result);
            }
            FillLavaInDesignatedArea();
        }
        else
        {
            var randomMap = (MapTypes)CTG2.randomGenerator.Next(0, 7);
            ClearHoneyForMap();
            Map.LoadMap(randomMap);
            FillLavaInDesignatedArea();
            if (randomMap == MapTypes.Kraken || randomMap == MapTypes.Stalactite)
            {
                FillHoneyForMap(randomMap);
            }
        }
    }

    // Pauses/Unpauses game
    public void PauseGame()
    {
        pause = true;
    }
    public void UnpauseGame()
    {
        pause = false;
    }

    public void EndGame()
    {
        isOvertime = false;
        GameInfo.overtime = false;
        overtimeTimer = 0;
        Console.WriteLine("GameManager: Starting EndGame sequence");

        IsGameActive = false;
        HasRoundStarted = false;
        MatchTime = 0;

        // Reset gems
        BlueGem.Reset();
        RedGem.Reset();
        Console.WriteLine("GameManager: Reset gems");

        var mod = ModContent.GetInstance<CTG2>();

        // Handle all active players
        foreach (Player player in Main.player)
        {
            if (!player.active) continue;

            Console.WriteLine($"GameManager: Processing player {player.whoAmI} ({player.name}) for game end");

            // Reset player state to None
            PlayerManager.GetPlayerManager(player.whoAmI).playerState = PlayerManager.PlayerState.None;

            ModPacket statePacket = mod.GetPacket();
            statePacket.Write((byte)MessageType.UpdatePlayerState);
            statePacket.Write(player.whoAmI);
            statePacket.Write((byte)PlayerManager.PlayerState.None);
            statePacket.Send(toClient: player.whoAmI);

            // Clear any spectator status
            player.ghost = false;
            player.respawnTimer = 0;

            // Remove from spectator tracking if they were spectating
            if (playerSpectatorStatus.GetValueOrDefault(player.whoAmI, false))
            {
                ModPacket spectatorPacket = mod.GetPacket();
                spectatorPacket.Write((byte)MessageType.ServerSpectatorUpdate);
                spectatorPacket.Write(player.whoAmI);
                spectatorPacket.Write(false);
                spectatorPacket.Send(toClient: player.whoAmI);
            }

            // Teleport all players to spectator area
            // web first
            CTG2.WebPlayer(player.whoAmI, 240);

            //can write out bool for player winner in the future to only tp losers to top
            ModPacket teleportPacket = mod.GetPacket();
            teleportPacket.Write((byte)MessageType.ServerTeleport);
            teleportPacket.Write(player.whoAmI);
            teleportPacket.Write((int)13317);
            teleportPacket.Write((int)10855);
            teleportPacket.Send(toClient: player.whoAmI);

            // Clear inventory and reset player stats
            player.statLife = player.statLifeMax;
            player.statMana = player.statManaMax;

            // Clear all player inventory and equipment
            ClearPlayerInventory(player);

            Console.WriteLine($"GameManager: Reset player {player.whoAmI} state and teleported to spectator area");
        }

        // Clear all tracking dictionaries
        playerSpectatorStatus.Clear();
        spectatorOriginalTeams.Clear();

        // Update team lists
        BlueTeam.UpdateTeam();
        RedTeam.UpdateTeam();

        Console.WriteLine("GameManager: Cleared all spectator tracking and updated teams");

        // Send ServerGameEnd packet to all clients
        ModPacket endPacket = mod.GetPacket();
        endPacket.Write((byte)MessageType.ServerGameEnd);
        endPacket.Send();

        // Broadcast game end message
        //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[GAME] Game has ended! New game starting in 15 seconds..."), Microsoft.Xna.Framework.Color.Yellow);
        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[GAME] Game has ended!"), Microsoft.Xna.Framework.Color.Yellow);

        Console.WriteLine("GameManager: EndGame sequence completed");

        // Start the 15-second timer for new game
        isWaitingForNewGame = true;
        newGameTimer = 15 * 60; // 15 seconds * 60 ticks per second
    }

    // Runs every frame while game running. Runs all gem checks, draws timer and gem status.
    public void UpdateGame()
    {
        if (pause)
        {
            return;
        }
        // force set team/pvp
        BlueTeam.EnforceTeam();
        RedTeam.EnforceTeam();
        
        // Additional PvP enforcement
        EnsureAllPlayersHavePvP();
        
        if (MatchTime == 1800)
        {
            // this code will handle class selection and stuff
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"class selection is ending"), Microsoft.Xna.Framework.Color.Olive);
                
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

            // fire works test?
    
            if (BlueGem.IsHeld)
            {
                blueGemFireworkTimer++;
                if (blueGemFireworkTimer >= FIREWORK_INTERVAL)
                {
                    blueGemFireworkTimer = 0;
                    var holder = Main.player[BlueGem.HeldBy];

                    int projectileIndex = Projectile.NewProjectile(
                        Entity.GetSource_NaturalSpawn(),
                        holder.Center,
                        new Vector2(0f, -8f),
                        ProjectileID.RocketFireworkBlue,
                        0,
                        0f,
                        Main.myPlayer
                    );

                    if (projectileIndex >= 0 && projectileIndex < Main.maxProjectiles)
                    {
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectileIndex);
                    }
                }
            }
            else
            {
                blueGemFireworkTimer = 0;
            }
            
            if (RedGem.IsHeld)
            {
                redGemFireworkTimer++;
                if (redGemFireworkTimer >= FIREWORK_INTERVAL)
                {
                    redGemFireworkTimer = 0;
                    var holder = Main.player[RedGem.HeldBy];

                    int projectileIndex = Projectile.NewProjectile(
                        Entity.GetSource_NaturalSpawn(), 
                        holder.Center,
                        new Vector2(0f, -8f), 
                        ProjectileID.RocketFireworkRed,
                        0, 
                        0f, 
                        Main.myPlayer 
                    );
                    
                    if (projectileIndex >= 0 && projectileIndex < Main.maxProjectiles)
                    {
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectileIndex);
                    }
                }
            }
            else
            {
                redGemFireworkTimer = 0; // Reset 
            }
        }
        // Send updated GameInfo to clients every 6 ticks (every 0.1s)
        if (MatchTime % 6 == 0)
        {
            var mod = ModContent.GetInstance<CTG2>();
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MessageType.ServerGameUpdate);
            
            foreach (Player p in Main.player) {
                    ForcePlayerStatSync(p.whoAmI);
            }

            // Determine match stage based on game state
            int matchStage;
            if (MatchTime < 1800)
            {
                matchStage = 1; // Class Selection phase
            }
            else
            {
                matchStage = 2; // Active Gameplay phase
            }
            
            packet.Write(matchStage);
            packet.Write((int)MatchTime);
            packet.Write(isOvertime); 
            packet.Write(isOvertime ? overtimeTimer : 0); 
            
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

        if (!isOvertime && MatchTime >= 60 * 60 * 15)
        {
         
            if (BlueGem.IsHeld || RedGem.IsHeld)
            {
                isOvertime = true;
                overtimeTimer = OVERTIME_DURATION;
                GameInfo.overtime = true; 
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("[OVERTIME] Overtime has started! Capture the gem or the game will end in 2 minutes."), Microsoft.Xna.Framework.Color.OrangeRed);
            }
            else
            {
                EndGame();
                return;
            }
        }

        if (isOvertime)
        {
            overtimeTimer--;

            if (BlueGem.IsCaptured || RedGem.IsCaptured)
            {
                EndGame();
                return;
            }

            if (!BlueGem.IsHeld && !RedGem.IsHeld)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("[OVERTIME] Overtime ended: No gem is being held."), Microsoft.Xna.Framework.Color.Yellow);
                EndGame();
                return;
            }

            if (overtimeTimer <= 0)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("[OVERTIME] Overtime expired! No capture."), Microsoft.Xna.Framework.Color.Red);
                EndGame();
                return;
            }
        }
         // Kill all mobs during class selection
        if (GameInfo.matchStage == 1 && killonce == true)
        {

            if (killonce)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (!npc.active)
                        continue;

                    npc.active = false;

                }
                killonce = false;
            }
        }
        if (GameInfo.matchStage == 2 && killonce == false)
        {
            killonce = true;
        }
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
            // drop gem if they have it 
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


            // Set spectator status (REMOVE THIS EVENTUALLY)
            playerSpectatorStatus[playerIndex] = true;

            // Send teleport packet to client
            CTG2.WebPlayer(player.whoAmI,120); //not sure if webbing is needed here

            var mod = ModContent.GetInstance<CTG2>();
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MessageType.ServerTeleport);
            packet.Write(playerIndex);
            packet.Write((int)spectatorSpawnPoint.X);
            packet.Write((int)spectatorSpawnPoint.Y);
            packet.Send(toClient: playerIndex);

            PlayerManager.GetPlayerManager(player.whoAmI).playerState = PlayerManager.PlayerState.Spectator;

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
            Console.WriteLine($"GameManager: SetPlayerSpectator called with isSpectator=false for player {playerIndex}");
            if (player.team == 0)
            {
                Console.WriteLine($"Player {player.name} cannot exit spectator mode - no team assigned");
                return;
            }
            // Check if player has original team
            if (60 * 15 * 60 - MatchTime < 45 * 60)
            {
                // Player is cooked 

                Console.WriteLine($"Player {player.name} cannot exit spectator mode - game is about to end soon");
                return;
            }

            playerSpectatorStatus[playerIndex] = false;
            int playerTeam = player.team;

            var mod = ModContent.GetInstance<CTG2>();
            NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, playerIndex, playerTeam);


            Console.WriteLine($"GameManager: Directly calling startPlayerClassSelection for player {playerIndex} (non-game-start)");
            startPlayerClassSelection(playerIndex, false);
            // TELL SERVER TO REMOVE GHOST AND NO LONGER A SPECTATOR
            ModPacket statusPacket2 = mod.GetPacket();
            statusPacket2.Write((byte)MessageType.ServerSpectatorUpdate);
            statusPacket2.Write(playerIndex);
            statusPacket2.Write(false);
            statusPacket2.Send();
        }
    }


    public void startPlayerClassSelection(int playerIndex, bool gameStarted)
    {
        // Don't try to access PlayerManager directly on server
        // Instead, get the team from the vanilla Player.team
        Player player = Main.player[playerIndex];
        int team = player.team;
        
        Console.WriteLine($"GameManager: startPlayerClassSelection called for player {playerIndex}, team {team}, gameStarted: {gameStarted}");
        
        
        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[DEBUG] {playerIndex} successfully entered class selection (non game)"), Color.Beige);
        // Send packet to set class selection time on client
        var mod = ModContent.GetInstance<CTG2>();
        ModPacket timePacket = mod.GetPacket();
        timePacket.Write((byte)MessageType.SetClassSelectionTime);
        timePacket.Write(playerIndex);
        timePacket.Write(1800.0); // class selection time
        timePacket.Send(toClient: playerIndex);
        Console.WriteLine($"GameManager: Sent SetClassSelectionTime packet to player {playerIndex}");
        
        if (!intToTeam.ContainsKey(team))
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[ERROR] Invalid team {team} for player {playerIndex}"), Color.Red);
            return;
        }
        
        GameTeam gameTeam = intToTeam[team];
        PlayerManager.GetPlayerManager(player.whoAmI).playerState = PlayerManager.PlayerState.ClassSelection;
        // Send packet to update client-side player state to ClassSelection
        ModPacket statePacket = mod.GetPacket();
        statePacket.Write((byte)MessageType.UpdatePlayerState);
        statePacket.Write(playerIndex);
        statePacket.Write((byte)PlayerManager.PlayerState.ClassSelection);
        statePacket.Send(toClient: playerIndex);
        Console.WriteLine($"GameManager: Sent UpdatePlayerState packet to player {playerIndex} (ClassSelection)");
        CTG2.WebPlayer(player.whoAmI, 240);
        ModPacket packet = mod.GetPacket();
        packet.Write((byte)MessageType.ServerTeleport);
        packet.Write(playerIndex);
        packet.Write((int)gameTeam.ClassLocation.X);
        packet.Write((int)gameTeam.ClassLocation.Y);
        packet.Send(toClient: playerIndex);
        Console.WriteLine($"GameManager: Sent ServerTeleport packet to player {playerIndex} to {gameTeam.BaseLocation}");
    }
    public void endPlayerClassSelection(int playerIndex)
    {
        // Use vanilla Player.team instead of PlayerManager on server
        Player player = Main.player[playerIndex];
        int team = player.team;
        
        if (!intToTeam.ContainsKey(team))
            return;
        
        GameTeam gameTeam = intToTeam[team];
        
        var mod = ModContent.GetInstance<CTG2>();
        
        PlayerManager.GetPlayerManager(player.whoAmI).playerState = PlayerManager.PlayerState.Active;
        ModPacket statePacket = mod.GetPacket();
        statePacket.Write((byte)MessageType.UpdatePlayerState);
        statePacket.Write(playerIndex);
        statePacket.Write((byte)PlayerManager.PlayerState.Active);
        statePacket.Send(toClient: playerIndex);
      
        NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, playerIndex);
        
        // Force sync player stats to ensure HP displays correctly to other players
        //ForcePlayerStatSync(playerIndex);

        // Send teleport packet to client
        CTG2.WebPlayer(player.whoAmI,240); //could be overkill
        ModPacket packet = mod.GetPacket();
        packet.Write((byte)MessageType.ServerTeleport);
        packet.Write(playerIndex);
        packet.Write((int)gameTeam.BaseLocation.X);
        packet.Write((int)gameTeam.BaseLocation.Y);
        packet.Send(toClient: playerIndex);

        ModPacket healpacket = mod.GetPacket(); //do heal server side
        healpacket.Write((byte)MessageType.RequestFullHeal); 
        healpacket.Write(playerIndex);
        healpacket.Send(toClient: playerIndex);
    }
    public override void PostUpdateWorld()
    {
        if (!Main.dedServ) return;
        if (pause)
        {
            return;
        }

        /*
        // Handle new game timer
        if (isWaitingForNewGame && newGameTimer > 0)
        {
            newGameTimer--;
            
            // Send game info updates during waiting phase every 6 ticks
            if (newGameTimer % 6 == 0)
            {
                var mod = ModContent.GetInstance<CTG2>();
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)MessageType.ServerGameUpdate);
                packet.Write((int)3); // Waiting for new game phase
                packet.Write(newGameTimer); // Send remaining time instead of match time
                packet.Write(isOvertime); 
                packet.Write(isOvertime ? overtimeTimer : 0); 
                // Send empty gem data during waiting phase
                packet.Write((int)0); // Blue gem position
                packet.Write((int)0); // Red gem position
                packet.Write("Waiting for new game..."); // Blue gem status
                packet.Write("Waiting for new game..."); // Red gem status
                packet.Send();
            }
            
            // Countdown announcements
            if (newGameTimer == 10 * 60) // 10 seconds left
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[GAME] New game starting in 10 seconds..."), Microsoft.Xna.Framework.Color.Orange);
            }
            else if (newGameTimer == 5 * 60) // 5 seconds left
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[GAME] New game starting in 5 seconds..."), Microsoft.Xna.Framework.Color.Red);
            }
            else if (newGameTimer == 3 * 60) // 3 seconds left
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[GAME] 3..."), Microsoft.Xna.Framework.Color.Red);
            }
            else if (newGameTimer == 2 * 60) // 2 seconds left
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[GAME] 2..."), Microsoft.Xna.Framework.Color.Red);
            }
            else if (newGameTimer == 1 * 60) // 1 second left
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[GAME] 1..."), Microsoft.Xna.Framework.Color.Red);
            }
            
            return; // Don't run game logic while waiting for new game
        }
        else if (isWaitingForNewGame && newGameTimer <= 0)
        {
            // Time to start new game
            isWaitingForNewGame = false;
            newGameTimer = 0;
            
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[GAME] Starting new game now!"), Microsoft.Xna.Framework.Color.LimeGreen);
            Console.WriteLine("GameManager: Starting new game after 15-second delay");
            
            StartGame();
            return;
        }
        */
        
        if (!IsGameActive) 
        {
            // Send game info updates when game is not active every 6 ticks
            if (Main.GameUpdateCount % 6 == 0)
            {
                var mod = ModContent.GetInstance<CTG2>();
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)MessageType.ServerGameUpdate);
                packet.Write((int)0); // No game active
                packet.Write((int)0); // No match time
                packet.Write(isOvertime); 
                packet.Write(isOvertime ? overtimeTimer : 0); 
                // Send empty gem data when no game is active
                packet.Write((int)0); // Blue gem position
                packet.Write((int)0); // Red gem position
                packet.Write("No game active"); // Blue gem status
                packet.Write("No game active"); // Red gem status
                packet.Send();
            }
            return;
        }

        UpdateGame();

        base.PostUpdateWorld();
    }
    
    private void ClearPlayerInventory(Player player)
    {
        // Clear main inventory
        for (int i = 0; i < player.inventory.Length; i++)
        {
            player.inventory[i].TurnToAir();
        }
        
        // Clear armor and accessories
        for (int i = 0; i < player.armor.Length; i++)
        {
            player.armor[i].TurnToAir();
        }
        
        // Clear dye slots
        for (int i = 0; i < player.dye.Length; i++)
        {
            player.dye[i].TurnToAir();
        }
        
        // Clear miscellaneous equipment
        for (int i = 0; i < player.miscEquips.Length; i++)
        {
            player.miscEquips[i].TurnToAir();
        }
        
        // Clear misc dyes
        for (int i = 0; i < player.miscDyes.Length; i++)
        {
            player.miscDyes[i].TurnToAir();
        }
        
        // Sync inventory changes to all clients
        NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, player.whoAmI);
        
        Console.WriteLine($"GameManager: Cleared inventory for player {player.whoAmI} ({player.name})");
    }

    public void HandlePlayerTeamChange(int playerIndex, int newTeam)
    {
        if (!Main.player[playerIndex].active) return;
        
        Player player = Main.player[playerIndex];
        int oldTeam = player.team;
        
        Console.WriteLine($"GameManager: HandlePlayerTeamChange called for player {playerIndex} from team {oldTeam} to team {newTeam}");
        
        // Check if too late to change teams during active game (similar to spectator logic)
        if (IsGameActive && MatchTime >= 60 * 60 * 15 - 45 * 60)
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"[TEAM] {player.name} cannot change teams - game ending soon"), Microsoft.Xna.Framework.Color.Red);
            Console.WriteLine($"Player {player.name} cannot change teams - game ending soon");
            return;
        }
        
        // Set the new team
        player.team = newTeam;
        
        // Update PlayerManager team
        var playerManager = player.GetModPlayer<PlayerManager>();
        playerManager.SetTeam(newTeam);
        
        // Send team update to all clients
        NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, playerIndex, newTeam);
        
        var mod = ModContent.GetInstance<CTG2>();
        
        // Send packet to update PlayerManager.team on client side
        ModPacket updatePacket = mod.GetPacket();
        updatePacket.Write((byte)MessageType.UpdatePlayerTeam);
        updatePacket.Write(playerIndex);
        updatePacket.Write(newTeam);
        updatePacket.Send(toClient: playerIndex);
        
        // Handle gem dropping if player was carrying one
        if (BlueGem.IsHeld && BlueGem.HeldBy == playerIndex)
        {
            BlueGem.Reset();
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{player.name} dropped the Blue Gem when changing teams"), Microsoft.Xna.Framework.Color.Blue);
            Console.WriteLine($"Player {player.name} dropped Blue Gem when changing teams");
        }
        
        if (RedGem.IsHeld && RedGem.HeldBy == playerIndex)
        {
            RedGem.Reset();
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{player.name} dropped the Red Gem when changing teams"), Microsoft.Xna.Framework.Color.Red);
            Console.WriteLine($"Player {player.name} dropped Red Gem when changing teams");
        }
        
        // Update team rosters
        BlueTeam.UpdateTeam();
        RedTeam.UpdateTeam();
        
        // Handle different scenarios based on new team and game state
        if (newTeam == 0)
        {
            // Player set to no team - make them spectator
            SetPlayerSpectator(playerIndex, true);
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{player.name} has been moved to spectator (no team)"), Microsoft.Xna.Framework.Color.Olive);
            Console.WriteLine($"Player {player.name} moved to spectator due to team change to 0");
        }
        else if (IsGameActive)
        {
            // Game is active - handle based on game phase
            

            
            // Remove from spectator if they were spectating
            if (playerSpectatorStatus.GetValueOrDefault(playerIndex, false))
            {
                playerSpectatorStatus[playerIndex] = false;
                ModPacket spectatorPacket = mod.GetPacket();
                spectatorPacket.Write((byte)MessageType.ServerSpectatorUpdate);
                spectatorPacket.Write(playerIndex);
                spectatorPacket.Write(false);
                spectatorPacket.Send(toClient: playerIndex);
            }
            
            // Start class selection for the player
            startPlayerClassSelection(playerIndex, MatchTime < 1800); // true if during initial game start phase
            
            // Force sync stats after team change
            //ForcePlayerStatSync(playerIndex);
            
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{player.name} has been moved to team {newTeam} and entered class selection"), Microsoft.Xna.Framework.Color.Green);
            Console.WriteLine($"Player {player.name} moved to team {newTeam} and started class selection");
        }
        else
        {
            // No game active - just update team assignment
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{player.name} has been assigned to team {newTeam}"), Microsoft.Xna.Framework.Color.Yellow);
            Console.WriteLine($"Player {player.name} assigned to team {newTeam} (no active game)");
        }
    }

    private void EnsureAllPlayersHavePvP()
    {
        foreach (Player player in Main.player)
        {
            if (player.active && player.team != 0 && !player.hostile)
            {
                player.hostile = true;
                NetMessage.SendData(MessageID.TogglePVP, -1, -1, null, player.whoAmI);
                Console.WriteLine($"GameManager: Forced PvP on for player {player.name}");
            }
        }
    }

    public static void ForcePlayerStatSync(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= Main.player.Length) return;
        
        Player player = Main.player[playerIndex];
        if (!player.active) return;
        
        var classSystem = player.GetModPlayer<ClassSystem>();
        classSystem.SyncPlayerStats();
        
        //Console.WriteLine($"GameManager: Force synced stats for player {player.name}");
    }
}
