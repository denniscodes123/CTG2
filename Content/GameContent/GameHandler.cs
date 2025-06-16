using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI;
using Terraria.GameContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.Chat;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using CTG2.Content;
using System.Linq;

public class GameHandler : ModSystem
{
    // A list to hold all created game instances. Static for easy access from commands.
    public static HashSet<CTG2.Content.Game> ActiveGames = new HashSet<CTG2.Content.Game>();

    /// <summary>
    /// This is the main game loop that runs every tick.
    /// </summary>
    public override void PostUpdateEverything()
    {
     
        foreach (CTG2.Content.Game game in ActiveGames.ToList())
        {
        
            if (game.match == null)
                continue;

            var match = game.match;

   
            if (match.classSelection && DateTime.Now >= match.classSelectionEndTime)
            {
                match.EndClassSelection();
                Main.NewText($" Class selection has ended! The match begins now.", 255, 235, 59);
            }
            // Check if the match time is over (and class selection is finished)
            else if (!match.classSelection && DateTime.Now >= match.matchEndTime)
            {
                Main.NewText($" Match has ended!", 255, 235, 59);
                game.EndMatch();
            }
        }
    }

    /// <summary>
    /// This hook adds our custom UI layer for the timer to the screen.
    /// </summary>
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
        if (index != -1)
        {
            layers.Insert(index + 1, new LegacyGameInterfaceLayer(
                "CTG2: Match Timers",
                delegate
                {
                    // The DrawMatchTimer method will only draw if the local player is in a match.
                    DrawMatchTimer();
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }

    /// <summary>
    /// Draws the match timer UI on the screen.
    /// </summary>
    private void DrawMatchTimer()
    {
        // Find the game the local player is currently in.
        Player localPlayer = Main.LocalPlayer;
        CTG2.Content.Game localGame = ActiveGames.FirstOrDefault(g => g.match != null && g.match.players.Contains(localPlayer));

        // If the player isn't in an active match, do nothing.
        if (localGame == null)
            return;

        Match match = localGame.match;
        string timeText = "";
        Vector2 position = new Vector2(Main.screenWidth - 250, 40); // Adjusted position for visibility
        Color textColor = Color.White;

        if (match.classSelection)
        {
            TimeSpan remaining = match.classSelectionEndTime - DateTime.Now;
            if (remaining.TotalSeconds > 0)
            {
                timeText = $"Class Selection: {remaining.Seconds}";
                textColor = Color.Yellow;
            }
        }
        else // Match is in progress
        {
            TimeSpan remaining = match.matchEndTime - DateTime.Now;
            if (remaining.TotalSeconds > 0)
            {
                timeText = $"Time Remaining: {remaining.Minutes:D2}:{remaining.Seconds:D2}";
            }
        }

        if (!string.IsNullOrEmpty(timeText))
        {
            Utils.DrawBorderString(Main.spriteBatch, timeText, position, textColor, 1.2f);
        }
    }
}

