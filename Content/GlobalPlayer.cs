
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;
public class GlobalPlayer : ModPlayer //this file will be used later for dash physics
{

    public bool dashFeather = false;


    public enum DashType
    {
        feather = 0,
        invalid = -1
    }

    public DashType dashType = DashType.feather;

    public const int dashDown = 0;
    public const int dashUp = 1;
    public const int dashRight = 2;
    public const int dashLeft = 3;

    public int dashCooldown = 45;
    public int dashDuration = 25;

    public float dashVelocity = 7.5f;

    public int dashDir = -1;

    public bool dashAccessoryEquipped = false;
    public int dashDelay = 0;
    public int dashTimer = 0;
}
