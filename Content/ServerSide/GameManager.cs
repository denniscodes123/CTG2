using System;
using Terraria;
using Terraria.ModLoader;
using CTG2.Content;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

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

    public override void OnWorldLoad()
    {
        BlueGem  = new Gem(new Vector2(13332, 11504));     
        RedGem   = new Gem(new Vector2(19316, 11504));

        BlueTeam = new GameTeam(new Vector2(12346, 10980), new Vector2(12346, 10980), 3);
        RedTeam = new GameTeam(new Vector2(20385, 10980), new Vector2(20385, 10980), 1);
        
        IsGameActive = false;
        MatchTime    = 0;
    }
    
    public void StartGame()
    {
        IsGameActive = true;
        
        MatchTime    = 0;
        BlueGem.Reset();
        RedGem.Reset();
        
        BlueTeam.UpdateTeam();
        RedTeam.UpdateTeam();
        
        BlueTeam.StartMatch();
        RedTeam.StartMatch();
        // TODO: Map logic (map select?, load map from file, teleport to class select)...
        
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
        
        // Updates holding/capturing status of both gems.
        BlueGem.Update(RedGem, RedTeam.Players);
        RedGem.Update(BlueGem, BlueTeam.Players);
        
        // Send updated GameInfo to clients
        var mod = ModContent.GetInstance<CTG2>();
        ModPacket packet = mod.GetPacket();
        packet.Write((byte)MessageType.ServerGameUpdate);
        packet.Write((int)2);
        packet.Write((int)MatchTime);
        // Blue and red Gem X positions
        var distBetweenGems = Math.Abs(RedGem.Position.X - BlueGem.Position.X);
        if (BlueGem.IsHeld)
        {   
            // send % of the way the gem is to enemy base as an integer
            var distFromGem = Main.player[BlueGem.HeldBy].position.X - BlueGem.Position.X;
            var intPercentage = (int)Math.Round(distFromGem/distBetweenGems * 100);
            intPercentage = Math.Clamp(intPercentage, 0, 100);
            packet.Write(intPercentage);
        }
        else packet.Write((int)0);
        if (RedGem.IsHeld)
        {   
            // send % of the way the gem is to enemy base as an integer
            // dist from gem is negative for red
            var distFromGem = -(Main.player[RedGem.HeldBy].position.X - RedGem.Position.X);
            var intPercentage = (int)Math.Round(distFromGem/distBetweenGems * 100);
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
        
        if (BlueGem.IsCaptured)
        {   
            Console.WriteLine("Blue gem captured!");
            Main.NewText("Blue gem captured! Red wins!");
            EndGame();
        }

        else if (RedGem.IsCaptured)
        {
            Console.WriteLine("Red gem captured!");
            Main.NewText("Red gem captured! Blue wins!");
            EndGame();
        }
            
        // if Match time exceeds a certain point, end the match
        if (MatchTime >= 60 * 60 * 15)
        {
            EndGame();
        }
        // TODO: 
    }
    
    public override void PostUpdateWorld()
    {
        if (!Main.dedServ) return;
        if (!IsGameActive) return;
        
        UpdateGame();
        
        base.PostUpdateWorld();
    }

}