using System;
using System.Collections.Generic;
using System.Linq;
using CTG2.Content.ServerSide;
using CTG2.Content.Classes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.Chat;


namespace CTG2.Content.ClientSide;

public class UIManager : ModSystem
{   
    private UserInterface classInterface;
    private ClassUI classUIState;
    
    
    public override void OnWorldLoad()
    {
        // 1) Create your UIState
        classUIState = new ClassUI();

        // 2) Create a UserInterface and attach your state
        classInterface = new UserInterface();
        classInterface.SetState(classUIState);
    }
    
    public override void UpdateUI(GameTime gameTime)
    {
        // Only update the class interface when ShowClassUI is true
        if (Main.LocalPlayer.GetModPlayer<PlayerManager>().ShowClassUI)
        {
            classInterface?.Update(gameTime);
        }
    }
    
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
        if (index != -1)
        {
            
            if (Main.LocalPlayer.GetModPlayer<PlayerManager>().ShowClassUI)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "CTG2: Class Selection UI",
                    delegate
                    {
                        // Draw your UI
                        classInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
            
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
        Vector2 blueGemRow = new Vector2(Main.screenWidth - 320, 525);
        Vector2 blueGemRow2 = new Vector2(Main.screenWidth - 320, 575);
        Vector2 redGemRow = new Vector2(Main.screenWidth - 320, 625);
        Vector2 redGemRow2 = new Vector2(Main.screenWidth - 320, 675);
        
        Color textColor = Color.White;
        int matchStage = GameInfo.matchStage;
        if (matchStage == 0) return;
        
        int matchTime = GameInfo.matchTime;
        if (matchTime < 1800)
        {
            timeText = $"Class selection ends in: {30 - matchTime / 60}s";
        }
        else if (GameInfo.overtime)
        {
            int secondsLeft = GameInfo.overtimeTimer / 60;
            int minutesLeft = secondsLeft / 60;
            int remainder = secondsLeft % 60;
            timeText = $"Time left in match: {minutesLeft}:{remainder.ToString("D2")}";
        }
        else
        {
            int secondsElapsed = matchTime / 60 - 30;
            int secondsLeft = 900 - secondsElapsed;
            int minutesLeft = secondsLeft / 60;
            int remainder = secondsLeft % 60;
            timeText = $"Time left in match: {minutesLeft}:{remainder.ToString("D2")}";
        }
        var blueGemStatus = GameInfo.blueGemCarrier;
        var redGemStatus = GameInfo.redGemCarrier;
        var blueGemPosition = GameInfo.blueGemX;
        var redGemPosition = GameInfo.redGemX;

        var totalBars = 20;
        var blueBars = (int)Math.Clamp(Math.Round(blueGemPosition / 100f * totalBars), 0, totalBars);
        var redBars = (int)Math.Clamp(Math.Round(redGemPosition / 100f * totalBars), 0, totalBars);
        // Draw Gem Position like in CTG
        string blueGemIndicator  = "[c/0000FF:⬢"+ new string('▮', blueBars) + "]" + "[i:1524]" + "[c/FFFFFF:"+ new string('▮', totalBars-blueBars) + "⬢]";

        string redGemIndicator = "[c/FFFFFF:⬢"+ new string('▮', totalBars-redBars) + "]" + "[i:1526]" + "[c/FF0000:"+ new string('▮', redBars) + "⬢]";
        
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
        ChatManager.DrawColorCodedStringWithShadow(
            Main.spriteBatch,
            FontAssets.MouseText.Value,
            blueGemIndicator,
            blueGemRow2,
            Color.White,
            0,
            Vector2.Zero,
            Vector2.One
        );
        ChatManager.DrawColorCodedStringWithShadow(
            Main.spriteBatch,
            FontAssets.MouseText.Value,
            redGemIndicator,
            redGemRow2,
            Color.White,
            0,
            Vector2.Zero,
            Vector2.One
        );
        // Show gem carrier HP if gem is captured
        Vector2 carrierHpPos = new Vector2(Main.screenWidth - 320, 725);

        if (GameInfo.blueGemCarrier != "At Base" && !string.IsNullOrEmpty(GameInfo.blueGemCarrier))
        {
    
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player carrier = Main.player[i];
    
                if (carrier.active && carrier.name == GameInfo.blueGemCarrier)
                {
                
                    string hpText = $"{carrier.name}: {carrier.statLife}/{carrier.statLifeMax2}";
                    Utils.DrawBorderString(Main.spriteBatch, hpText, carrierHpPos, Color.Cyan);
                  
                    carrierHpPos.Y += 40; 
                    
                
                    break; 
                }
            }
        }


        if (GameInfo.redGemCarrier != "At Base" && !string.IsNullOrEmpty(GameInfo.redGemCarrier))
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player carrier = Main.player[i];
                if (carrier.active && carrier.name == GameInfo.redGemCarrier)
                {
                    string hpText = $"{carrier.name}: {carrier.statLife}/{carrier.statLifeMax2}";
                    Utils.DrawBorderString(Main.spriteBatch, hpText, carrierHpPos, Color.Red);
                    break; 
                }
            }
        }

        // draw ability timer
        int cooldown = Main.LocalPlayer.GetModPlayer<Abilities>().cooldown;
        string abilText = $"Ability cooldown: {cooldown / 60}s";
        Vector2 abilPos = new Vector2(Main.screenWidth - 320, 475);
        Color abilCol = Color.Yellow;

        if (cooldown == 0)
        {
            Utils.DrawBorderString(Main.spriteBatch, "Ability ready!", abilPos, abilCol);
        }
        else
        {
            Utils.DrawBorderString(Main.spriteBatch, abilText, abilPos, abilCol);
        }

        //draw map name
        string mapText = $"Map: {GameInfo.mapName}";
        Vector2 mapPos = new Vector2(Main.screenWidth - 320, 425);
        Color mapCol = Color.Pink;

        Utils.DrawBorderString(Main.spriteBatch, mapText, mapPos, mapCol);

        //draw team sizes
        string teamText = $"{GameInfo.blueTeamSize} (Blue) vs. {GameInfo.redTeamSize} (Red)";
        Vector2 teamTextPos = new Vector2(Main.screenWidth - 320, 350);
        Color teamTextCol = Color.Violet;

        Utils.DrawBorderString(Main.spriteBatch, teamText, teamTextPos, teamTextCol);
    }
}
