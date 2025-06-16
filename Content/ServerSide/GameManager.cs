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
    
    public GameTeam BlueTeam { get; private set; }
    public GameTeam RedTeam { get; private set; }
        
    public Gem BlueGem { get; private set; }
    public Gem RedGem { get; private set; }

    public override void OnWorldLoad()
    {
        Players = Main.player;                     
        BlueGem  = new Gem(new Vector2(100, 100));     
        RedGem   = new Gem(new Vector2(100, 100));

        BlueTeam = new GameTeam(new Vector2(100, 100), new Vector2(100, 100), 3);
        RedTeam = new GameTeam(new Vector2(200, 100), new Vector2(200, 100), 1);
        
        IsGameActive = false;
        MatchTime    = 0;
    }
    
    public void StartGame()
    {
        IsGameActive = true;
        
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
        MatchTime = 0;
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

        if (MatchTime == 900)
        {
            BlueTeam.SendToBase();
            RedTeam.SendToBase();
        }
        // Increase match duration by 1 tick
        MatchTime++;
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