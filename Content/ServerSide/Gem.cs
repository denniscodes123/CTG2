using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using CTG2.Content.ClientSide;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace CTG2.Content.ServerSide;

public class Gem
{
    public bool IsHeld { get; private set;}
    public int HeldBy {get; private set;}
    
    public bool IsCaptured {get; private set;}
    
    
    // X, Y Coords for the Top-Left Corner of Gem
    public Vector2 Position {get; private set;}
    public int Width {get;set;}
    public int Height {get;set;}
    private int team;
    public Rectangle GemHitbox {get; private set;}

    public Gem(Vector2 position, int team)
    {
        IsHeld = false;
        HeldBy = -1;
        IsCaptured = false;
        Position = position;
        Width = 6 * 16;
        Height = 9 * 16;
        GemHitbox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        this.team = team;

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
            TryDrop(Main.player[HeldBy]);
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
                NetworkText pickUpText = NetworkText.FromLiteral($"{ply.name} has picked up the gem!");
                // Broadcast to everyone
                var mod = ModContent.GetInstance<CTG2>();
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)MessageType.RequestAudio);
                packet.Write("CTG2/Content/ServerSide/GemPickup");
                packet.Send();
                ChatHelper.BroadcastChatMessage(pickUpText, Color.Aqua);
                break;
            }
        }
    }
    
    private void TryCapture(Gem otherGem, Player carrier)
    {
        PlayerManager playerManager = carrier.GetModPlayer<PlayerManager>();

        if (!carrier.active || carrier.dead) //|| (playerManager.playerState != PlayerManager.PlayerState.Active) || (carrier.team != this.team)
            return;

        if (carrier.Hitbox.Intersects(otherGem.GemHitbox))
        {
            IsCaptured = true;
            IsHeld     = false;
            // Optionally trigger capture event
            NetworkText captureText = NetworkText.FromLiteral($"{carrier.name} has captured the gem!");
            // Broadcast to everyone
            ChatHelper.BroadcastChatMessage(captureText, Color.Aqua);
        }
    }

    private void TryDrop(Player gemHolder)
    {
         PlayerManager playerManager = gemHolder.GetModPlayer<PlayerManager>();
        if (gemHolder.dead) // || (playerManager.playerState != PlayerManager.PlayerState.Active) || (gemHolder.team != this.team)
        {
            IsHeld = false;
            NetworkText dropText = NetworkText.FromLiteral($"{gemHolder.name} has dropped the gem!");
            // Broadcast to everyone
            var mod = ModContent.GetInstance<CTG2>();
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MessageType.RequestAudio);
            packet.Write("CTG2/Content/ServerSide/GemDrop");
            packet.Send();
            ChatHelper.BroadcastChatMessage(dropText, Color.Aqua);
        }
        if (gemHolder.ghost == true)
        {
            IsHeld = false;
            NetworkText dropText = NetworkText.FromLiteral($"{gemHolder.name} has dropped the gem!");
            // Broadcast to everyone
            ChatHelper.BroadcastChatMessage(dropText, Color.Aqua);
        }
    }
}
