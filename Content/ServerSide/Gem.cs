using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;

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
    
    public Rectangle GemHitbox {get; private set;}

    public Gem(Vector2 position)
    {   
        IsActive = true;
        IsHeld = false;
        HeldBy = -1;
        IsCaptured = false;
        Position = position;
        Width = 6 * 16;
        Height = 9 * 16;
        GemHitbox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
    }

    public void Reset()
    {
        IsHeld = false;
        IsCaptured = false;
        HeldBy = -1;
    }
    // Runs all Gem Logic - Check if Gem can be picked up/captured by any players
    public void Update(Gem otherGem, List<Player> otherTeam)
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
    
    private void TryGetGem(List<Player> otherTeam)
    {

        foreach (var ply in otherTeam)
        {
            if (!ply.active || ply.dead)
                continue;

            if (ply.Hitbox.Intersects(GemHitbox))
            {
                IsHeld = true;
                HeldBy = ply.whoAmI;
                break;
            }
        }
    }
    
    private void TryCapture(Gem otherGem, Player carrier)
    {

        if (!carrier.active || carrier.dead)
            return;

        if (carrier.Hitbox.Intersects(otherGem.GemHitbox))
        {
            IsCaptured = true;
            IsHeld     = false;
            // Optionally trigger capture event
        }
    }
    
}