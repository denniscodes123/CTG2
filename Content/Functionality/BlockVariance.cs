using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using System;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;

public class DamageVariance : ModSystem
{

    public override void Load()
    {
        var damageVarMethodInfo = typeof(Main).GetMethod("DamageVar", new[] { typeof(float), typeof(float) });
        if (damageVarMethodInfo != null)
        {
            IL_Main.DamageVar_float_float += DisableDamageVariance_Hook;
        }
        else
        {
            Mod.Logger.Error("Could not find Main.DamageVar(float, float) method.");
        }
        
        //On_Main.DamageVar_float_int_float += RemoveVariance;

    }
    public override void Unload()
    {
        var damageVarMethodInfo = typeof(Main).GetMethod("DamageVar", new[] { typeof(float), typeof(float) });
        if (damageVarMethodInfo != null)
        {
            IL_Main.DamageVar_float_float -= DisableDamageVariance_Hook;
        }
        else
        {
            Mod.Logger.Error("Could not find Main.DamageVar(float, float) method.");
        }
        base.Unload();
    }
    private void DisableDamageVariance_Hook(ILContext il)
    {
        var c = new ILCursor(il);


        c.RemoveRange(c.Instrs.Count);
        c.Emit(OpCodes.Ldarg_0);
        c.Emit(OpCodes.Conv_R8);
        c.Emit(OpCodes.Call, typeof(Math).GetMethod("Round", new[] { typeof(double) })); 
        c.Emit(OpCodes.Conv_I4);
        c.Emit(OpCodes.Ret);
    }

    // private static int RemoveVariance(On_Main.orig_DamageVar_float_int_float orig, float dmg, int percent, float luck)
    // {
    //     return (int)Math.Round(dmg);
    // }
}