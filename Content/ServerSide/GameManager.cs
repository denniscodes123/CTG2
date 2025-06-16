using Terraria;
using Terraria.ModLoader;

namespace CTG2.Content.ServerSide;

public class GameManager
{   
    // True when 
    public bool IsGameActive { get; private set; }
    public bool HasRoundStarted { get; private set; }
    
    public int MatchTime { get; private set; }
    
    public Player[] Players { get; private set; }
        
    public Gem BlueGem { get; private set; }
    public Gem RedGem { get; private set; }
    
    public GameManager()
    {
        // TODO: populate constructor
    }
    
    public void StartGame()
    {
        IsGameActive = true;
        
        // TODO: Map logic (map select?, load map from file, 
    }
    
    // Pauses/Unpauses game
    public void PauseGame()
    {
        IsGameActive = !IsGameActive;
        // TODO: Send a ModPacket to toggle 'Pause Mode' client-side. Add new packet type to HandlePacket()
    }
    
    public void EndGame()
    {
        IsGameActive = false;
        // TODO: Return all players to spectate area, clear inventories, etc. 
    }
    
    // Runs every frame while game running. Runs all gem checks, draws timer and gem status.
    public void UpdateGame()
    {
        // TODO: Check if each player has completed class selection (no == class select, yes == send to match)
        
        // Increase match duration by 1 tick
        MatchTime++;
        // Updates holding/capturing status of both gems.
        BlueGem.Update(RedGem, Players);
        RedGem.Update(BlueGem, Players);
        
        // TODO: 
    }

}