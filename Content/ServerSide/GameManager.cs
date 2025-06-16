using System;
using Terraria;
using Terraria.ModLoader;
using CTG2.Content;
using Microsoft.Xna.Framework;

namespace CTG2.Content.ServerSide;

public class GameManager : ModSystem
{   
    // True when 
    public bool IsGameActive { get; private set; }
    public bool HasRoundStarted { get; private set; }
    
    public int MatchTime { get; private set; }
    
    public Player[] Players { get; private set; }
        
    public Gem BlueGem { get; private set; }
    public Gem RedGem { get; private set; }

    public override void OnWorldLoad()
    {
        Players = Main.player;                     
        BlueGem  = new Gem(new Vector2(0,0));     
        RedGem   = new Gem(new Vector2(0,0));
        IsGameActive = false;
        MatchTime    = 0;
    }
    
    public void StartGame()
    {
        IsGameActive = true;
        var mod = ModContent.GetInstance<CTG2>();
        ModPacket packet = mod.GetPacket();
        packet.Write((byte)MessageType.ServerGameStart);
        packet.Send();
        // TODO: Map logic (map select?, load map from file, 
    }
    
    // Pauses/Unpauses game
    public void PauseGame()
    {
        IsGameActive = !IsGameActive;
        var mod = ModContent.GetInstance<CTG2>();
        ModPacket packet = mod.GetPacket();
        packet.Write((byte)MessageType.ServerGamePause);
        packet.Send();
        // TODO: Send a ModPacket to toggle 'Pause Mode' client-side. Add new packet type to HandlePacket()
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
        
        // Increase match duration by 1 tick
        MatchTime++;
        Console.WriteLine("Match Time: " + MatchTime);
        // Updates holding/capturing status of both gems.
        BlueGem.Update(RedGem, Players);
        RedGem.Update(BlueGem, Players);
        
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