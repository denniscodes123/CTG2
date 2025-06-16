using Microsoft.Xna.Framework;
using Terraria;

namespace CTG2.Content.ServerSide;

public class Gem
{
    public bool IsActive {get; private set;}
    public bool IsHeld { get; private set;}
    public int HeldBy {get; private set;}
    
    public bool IsCaptured {get; private set;}
    
    
    // X, Y Coords for the Top-Left Corner of Gem
    public Vector2 Position {get; private set;}
    public int Width {get;set;}
    public int Height {get;set;}

    public Gem(Vector2 position)
    {   
        IsActive = true;
        IsHeld = false;
        HeldBy = -1;
        IsCaptured = false;
        Position = position;
    }
    
    // Runs all Gem Logic - Check if Gem can be picked up/captured by any players
    public void Update(Gem otherGem, Player[] otherTeam)
    {
        if (IsHeld)
        {
            TryCapture(otherGem, Main.player[HeldBy]);
        }
        else
        {
            TryGetGem(otherTeam);
        }
    }
    
    private void TryGetGem(Player[] otherTeam)
    {
        // TODO: iterate over players, check if player hitbox overlaps this gem's area. set IsHeld=True
    }
    
    private void TryCapture(Gem otherGem, Player carrier)
    {
        // TODO: Check if gem carrier's hitbox overlaps enemy gem's area. set IsCaptured=True (game will end)
    }
    
}