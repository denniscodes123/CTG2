using Terraria.ModLoader;
using MonoMod.Cil;

using System;
using Terraria;
using CTG2.Content;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using System.Collections;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Chat;
using Terraria.Localization;
using CTG2.Content.ClientSide;
using Terraria.Enums;
using ClassesNamespace;
using Mono.Cecil.Cil;
using System.Reflection;

namespace CTG2.Detouring;

public class ClientModifications : ModSystem
{
    public override void Load()
    {
        IL_Player.TeamChangeAllowed += TeamChangeFlag;
        var drawPVPIconsMethodInfo = typeof(Main).GetMethod("DrawPVPIcons", BindingFlags.NonPublic | BindingFlags.Static);
                if (drawPVPIconsMethodInfo != null)
        {
            IL_Main.DrawPVPIcons += Main_DrawPVPIcons_Hook;
        }
        else
        {
            // Log an error if we can't find the method, which might happen if the game updates.
            Mod.Logger.Error("Could not find Main.DrawPVPIcons method. PvP admin check will not work.");
        }
        
    }
    public override void Unload()
    {
        IL_Player.TeamChangeAllowed -= TeamChangeFlag;
        var drawPVPIconsMethodInfo = typeof(Main).GetMethod("DrawPVPIcons", BindingFlags.NonPublic | BindingFlags.Static);
                if (drawPVPIconsMethodInfo != null)
        {
            IL_Main.DrawPVPIcons -= Main_DrawPVPIcons_Hook;
        }
        else
        {
            // Log an error if we can't find the method, which might happen if the game updates.
            Mod.Logger.Error("Could not find Main.DrawPVPIcons method. PvP admin check will not work.");
        }
    }
    private void TeamChangeFlag(ILContext iLContext)
    {
        var c = new ILCursor(iLContext);

        c.GotoNext(MoveType.Before, i => i.MatchLdcI4(1));
        c.RemoveRange(2);
        c.Emit(OpCodes.Ldarg_0);

        c.EmitDelegate<System.Func<Player, bool>>(player =>
        {
            if (player.TryGetModPlayer<AdminPlayer>(out var adminPlayer) && adminPlayer.IsAdmin)
            {
                return true;
            }
            return false;
        });
        c.Emit(OpCodes.Ret);
    }
    private void Main_DrawPVPIcons_Hook(ILContext il)
    {
        var c = new ILCursor(il);


        ILLabel endLabel = null;
        if (!c.TryGotoNext(MoveType.After,
            i => i.MatchLdsfld(typeof(Main), nameof(Main.mouseLeft)),
            i => i.MatchBrfalse(out _),
            i => i.MatchLdsfld(typeof(Main), nameof(Main.mouseLeftRelease)),
            i => i.MatchBrfalse(out _),
            i => i.MatchLdsfld(typeof(Main), nameof(Main.teamCooldown)),
            i => i.MatchBrtrue(out endLabel) 
            ))
        {
            Mod.Logger.Error("Could not find injection point in Main.DrawPVPIcons. PvP admin check will not work.");
            return;
        }

        c.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.player)));
        c.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.myPlayer)));
        c.Emit(OpCodes.Ldelem_Ref);
        c.Emit(OpCodes.Callvirt, typeof(Player).GetMethod(nameof(Player.TeamChangeAllowed)));
        c.Emit(OpCodes.Brfalse, endLabel);
    }

}