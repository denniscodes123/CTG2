using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using System;

public class DamageVariance : ModSystem
{
    public override void Load()
    {
        On_Main.DamageVar_float_int_float += RemoveVariance;
    }

    private static int RemoveVariance(On_Main.orig_DamageVar_float_int_float orig, float dmg, int percent, float luck)
    {
        return (int)Math.Round(dmg);
    }
}