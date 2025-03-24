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


namespace CTG2.Content
{
        public class GameCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "start";
        public override string Description => "Starts the match countdown with a preparation phase";



public override void Action(CommandCaller caller, string input, string[] args)
{
    var modPlayer = caller.Player.GetModPlayer<AdminPlayer>();
if (!modPlayer.IsAdmin)
{
    caller.Reply("You must be an admin to use this command.", Color.Red);
    return;
}

    
    GameUI.matchPrep=true;
    GameUI.matchTimer= (int)Main.GameUpdateCount/60+15;


Main.NewText("Arena region saved.");
Player player = caller.Player;
int px = (int)(player.position.X / 16);
int py = (int)(player.position.Y / 16);
caller.Reply($"You are at tile ({px}, {py})", Color.Yellow);

/* 
WorldProperties.savedX = 1493;
WorldProperties.savedY = 258;
WorldProperties.savedRegion = WorldProperties.SaveRegion(1493, 258, 2013, 337);
Main.NewText("Arena region saved.");


WorldProperties.savedX = Main.maxTilesX / 2 - 606;
WorldProperties.savedY = 10;
WorldProperties.savedRegion = WorldProperties.SaveRegion(
    WorldProperties.savedX,
    WorldProperties.savedY,
    Main.maxTilesX / 2 - 86,
    89
); */


    if (GameUI.matchStarted)
    {
        caller.Reply("A match is already in progress!", Color.Red);
        return;
    }

    else if(GameUI.matchPrep){
    caller.Reply("Match preparation started! You have 15 seconds to choose your class.", Color.Yellow);
    return;
    }
}


    }

public class StopCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;
    public override string Command => "stop";
    public override string Usage => "/stop";
    public override string Description => "Restore the map state after the game ends";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        WorldProperties.RestoreRegion(WorldProperties.savedX, WorldProperties.savedY, WorldProperties.savedRegion);

        Main.NewText("Region restored!");
        Main.NewText($"Restoring region at ({WorldProperties.savedX}, {WorldProperties.savedY})", Color.Orange); //logging for bad schema pasting
        Main.NewText($"Restoring size: {WorldProperties.savedRegion.GetLength(0)}x{WorldProperties.savedRegion.GetLength(1)}", Color.Orange);

    }
}

public class GameUI : ModSystem
    {
            public static bool matchStarted = false;
            public static bool matchPrep = false;

            public static int matchTimer = 0;


        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (index != -1)
            {
                layers.Insert(index + 1, new LegacyGameInterfaceLayer(
                    "CTG2: Match Timer",
                    delegate
                    {
                        if (GameUI.matchStarted || GameUI.matchPrep)
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
        Vector2 position = new Vector2(Main.screenWidth - 220, 400);
        Color textColor = Color.White;


        int remainingTime = GameUI.matchTimer - (int)(Main.GameUpdateCount / 60);

        int minutes = 15+remainingTime / 60; 
        int seconds = (59-((Math.Abs(remainingTime)) % 60)); 


        timeText = $"Class selection ends in: {remainingTime}"; 


        if (remainingTime < 0)
        {
            timeText = $"Time until a draw: {Math.Abs(minutes)}:{seconds:D2}";
        if(remainingTime==-1){
            SpawnPoints.TeleportAllPlayersToSpawn();
            
        }
        
        }

        else
        {
            textColor = Color.Yellow;
        }

        if (!string.IsNullOrEmpty(timeText))
        {
            Utils.DrawBorderString(Main.spriteBatch, timeText, position, textColor);
        }
    }
}
       
    }



