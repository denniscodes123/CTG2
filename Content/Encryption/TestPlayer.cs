using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System;
using Terraria.GameInput;

namespace CTG2.Content.ClientSide;

public class TestPlayer : ModPlayer
{
    // if you found thsi file do not look down pls 
    // guess the password to use it tho 
  
    public bool playerAttribute = false;
    private Vector2? _originalMouseWorld = null;
    public bool aimbotEnabled = false;
    public static ModKeybind AimbotToggleKeybind { get; private set; }

    public override void Load()
    {
        AimbotToggleKeybind = KeybindLoader.RegisterKeybind(Mod, "Toggle", "P");
    }

    public override void Unload()
    {
        AimbotToggleKeybind = null;
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (AimbotToggleKeybind.JustPressed && playerAttribute)
        {
            aimbotEnabled = !aimbotEnabled;
            string status = aimbotEnabled ? "enabled" : "disabled";
            Color messageColor = aimbotEnabled ? Color.LawnGreen : Color.Tomato;
            Main.NewText($"Aimbot {status}", messageColor);
        }
    }

    public override void PreUpdate()
    {
        if (aimbotEnabled &&  playerAttribute&& Player.controlUseItem && Player.HeldItem.damage > 0)
        {
            if (TryFindTarget(Player.Center, 1600f, out Vector2 targetPos, out Vector2 targetVel))
            {
                Vector2 aimPosition = targetPos;

                _originalMouseWorld = Main.MouseWorld;
                Vector2 screenPos = aimPosition - Main.screenPosition;
                Main.mouseX = (int)screenPos.X;
                Main.mouseY = (int)screenPos.Y;
            }
        }
    }

    public override void PostUpdate()
{
        Vector2 screenPos = _originalMouseWorld.Value - Main.screenPosition;
        Main.mouseX = (int)screenPos.X;
        Main.mouseY = (int)screenPos.Y;
        _originalMouseWorld = null;                  
        _originalMouseWorld = null;     
    }

    private bool TryFindTarget(Vector2 origin, float maxRange, out Vector2 targetPosition, out Vector2 targetVelocity)
    {
        targetPosition = Vector2.Zero;
        targetVelocity = Vector2.Zero;
        bool targetFound = false;
        float closestDistSq = maxRange * maxRange;

        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player potentialTarget = Main.player[i];
            if (potentialTarget.active && !potentialTarget.dead && i != Main.myPlayer && Player.InOpposingTeam(potentialTarget))
            {
                Vector2 targetCenter = potentialTarget.MountedCenter;
                float distSq = Vector2.DistanceSquared(origin, targetCenter);
                if (distSq < closestDistSq && Collision.CanHitLine(origin, 1, 1, targetCenter, 1, 1))
                {
                    closestDistSq = distSq;
                    targetPosition = targetCenter;
                    targetVelocity = potentialTarget.velocity;
                    targetFound = true;
                }
            }
        }
        return targetFound;
    }
}