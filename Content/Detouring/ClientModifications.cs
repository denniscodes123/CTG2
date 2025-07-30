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
using Terraria.GameContent.UI.Chat;

namespace CTG2.Detouring;

public class ClientModifications : ModSystem
{
    public override void Load()
    {
        IL_Player.TeamChangeAllowed += TeamChangeFlag;
        //IL_ChatHelper.DisplayMessage += ChatHelper_DisplayMessage_Hook;
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
        var drawBubblesMethodInfo = typeof(Main).GetMethod("DrawPlayerChatBubbles", BindingFlags.NonPublic | BindingFlags.Instance);

        if (drawBubblesMethodInfo != null)
        {
            // Apply our hook to the method.
            IL_Main.DrawPlayerChatBubbles += DisableChatBubbles_Hook;
        }
        else
        {
            // This is a safety check in case the method name changes in a future Terraria update.
            Mod.Logger.Error("Could not find Main.DrawPlayerChatBubbles method. Chat bubble disabling will not work.");
        }

    }
    public override void Unload()
    {
        IL_Player.TeamChangeAllowed -= TeamChangeFlag;
        //IL_ChatHelper.DisplayMessage -= ChatHelper_DisplayMessage_Hook;
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

        var drawBubblesMethodInfo = typeof(Main).GetMethod("DrawPlayerChatBubbles", BindingFlags.NonPublic | BindingFlags.Instance);

        if (drawBubblesMethodInfo != null)
        {
            IL_Main.DrawPlayerChatBubbles -= DisableChatBubbles_Hook;
        }
        else
        {
            Mod.Logger.Error("Could not find Main.DrawPlayerChatBubbles method. Chat bubble disabling will not work.");
        }

        base.Unload();
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
    private void DisableChatBubbles_Hook(ILContext il)
    {
        var c = new ILCursor(il);
        c.Emit(OpCodes.Ret);
    }
        private void ChatHelper_DisplayMessage_Hook(ILContext il)
    {
        var c = new ILCursor(il);


        if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdfld<Player>("name")))
        {
            Mod.Logger.Error("Could not find injection point in ChatHelper.DisplayMessage. Team chat tags will not work.");
            return;
        }
        
        c.Emit(OpCodes.Ldarg_2); 
        
        c.EmitDelegate<System.Func<string, byte, string>>((originalName, messageAuthor) => {
            Player player = Main.player[messageAuthor];

            if (player.team > 0 && player.team < Main.teamColor.Length)
            {
                Mod.Logger.Info($"Hooked chat for '{originalName}' from author {messageAuthor}.");
                Color teamColor = Main.teamColor[player.team];

                string teamName = teamColor.ToString();
                string teamTag = $"[c/{teamColor.Hex3()}:[{teamName}]]";

                return $"{teamTag} {originalName}";
            }
            Mod.Logger.Info($"Failed");

            return originalName;
        });

    }
}
