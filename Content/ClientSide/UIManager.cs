using System;
using System.Collections.Generic;
using CTG2.Content.ServerSide;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace CTG2.Content.ClientSide;

public class UIManager : ModSystem
{
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
        if (index != -1)
        {
            layers.Insert(index + 1, new LegacyGameInterfaceLayer(
                "CTG2: Match Timer",
                delegate
                {
                    if (true)
                    {
                        DrawMatchTimer();
                    }
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }


    private void DrawMatchTimer()
    {
        string timeText = "";
        Vector2 timeRow = new Vector2(Main.screenWidth - 320, 400);
        Vector2 blueGemRow = new Vector2(Main.screenWidth - 320, 500);
        Vector2 blueGemRow2 = new Vector2(Main.screenWidth - 320, 550);
        Vector2 redGemRow = new Vector2(Main.screenWidth - 320, 600);
        Vector2 redGemRow2 = new Vector2(Main.screenWidth - 320, 650);
        
        Color textColor = Color.White;
        int matchTime = GameInfo.matchTime;
        if (matchTime < 900)
        {
            timeText = $"Class selection ends in: {15-matchTime/60}s";
        }
        else
        {
            int secondsElapsed = matchTime / 60 - 15;
            int secondsLeft = 900 - secondsElapsed;
            int minutesLeft = secondsLeft / 60;
            int remainder = secondsLeft % 60;
            timeText = $"Time left in match: {minutesLeft}:{remainder}";
        }
        var blueGemStatus = GameInfo.blueGemCarrier;
        var redGemStatus = GameInfo.redGemCarrier;
        var bluegemPosition = GameInfo.blueGemX;
        var redgemPosition = GameInfo.redGemX;
        
        if (!string.IsNullOrEmpty(timeText))
        {
            Utils.DrawBorderString(Main.spriteBatch, timeText, timeRow, textColor);
        }
        if (!string.IsNullOrEmpty(blueGemStatus))
        {
            Utils.DrawBorderString(Main.spriteBatch, blueGemStatus, blueGemRow, Color.Blue);
        }
        if (!string.IsNullOrEmpty(redGemStatus))
        {
            Utils.DrawBorderString(Main.spriteBatch, redGemStatus, redGemRow, Color.Red);
        }
    }
}