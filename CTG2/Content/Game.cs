using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.Chat;
using System.IO;



namespace CTG2.Content
{
    
    public class Game : ModSystem
    {
        public static bool matchStarted = false;
        public static bool preparationPhase = false;
        public static int matchTimeLeft = 0; 
        public static int preparationTimeLeft = 15 * 60; 

        public static int preparationStartTime = 0;
public static int matchStartTime = 0;


public override void PostUpdateWorld()
{
    if (Main.netMode == NetmodeID.Server)
    {
        UpdateMatchTimers();

        if (Main.GameUpdateCount % 60 == 0) // Sync every second
        {
            SyncMatchState();
        }
    }
}



public static void UpdateMatchTimers()
{
    if (Main.netMode != NetmodeID.Server) return; // Ensure this runs only on the server

 

    if (preparationPhase)
    {
        int elapsedTime = (int)(Main.GameUpdateCount - preparationStartTime);
        preparationTimeLeft = Math.Max(0, 15 * 60 - elapsedTime); // Ensure it doesn't go below zero

        if (preparationTimeLeft <= 0)
        {
            preparationPhase = false;
            matchStarted = true;
            matchStartTime = (int)Main.GameUpdateCount;
            matchTimeLeft = 16 * 60 * 60;

            SpawnPoints.TeleportAllPlayersToSpawn();
            SyncMatchState();
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Class selection ended. The match has begun!"), Color.Green);
        }
    }
    else if (matchStarted)
    {
        int elapsedTime = (int)(Main.GameUpdateCount - matchStartTime);
        matchTimeLeft = Math.Max(0, 16 * 60 * 60 - elapsedTime);

        if (matchTimeLeft <= 0)
        {
            matchStarted = false;
            SyncMatchState();
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Match Draw! Time has run out."), Color.Red);
        }
    }
}


        public static void SyncMatchState()
        {
            if (Main.netMode == NetmodeID.Server) // Only the server sends the update
            {


                ModPacket packet = ModContent.GetInstance<CTG2>().GetPacket();
                packet.Write((byte)1); // Packet ID for match sync
                packet.Write(matchStarted);
                packet.Write(preparationPhase);
                packet.Write(matchTimeLeft);
                packet.Write(preparationTimeLeft);
                packet.Send(); // Sends to all clients
            }
        }
    }



    public class GameCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "start";
        public override string Description => "Starts the match countdown with a preparation phase";

public override void Action(CommandCaller caller, string input, string[] args)
{
    if (Game.matchStarted || Game.preparationPhase)
    {
        caller.Reply("A match is already in progress!", Color.Red);
        return;
    }

    Game.preparationPhase = true;
    Game.preparationStartTime = (int)Main.GameUpdateCount; 
    Game.preparationTimeLeft = 15 * 60;
    Game.matchStarted = false;
    Game.matchTimeLeft = 0;

    if (Main.netMode == NetmodeID.Server)
    {
        Game.SyncMatchState();
    }

    caller.Reply("Match preparation started! You have 15 seconds to choose your class.", Color.Yellow);
}

    }



public class GameUI : ModSystem
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
                        if (Game.preparationPhase || Game.matchStarted)
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

            if (Game.preparationPhase)
            {
                timeText = $"Class selection ends in: {Game.preparationTimeLeft / 60}";
                textColor = Color.Yellow;
            }
            else if (Game.matchStarted)
            {
                timeText = $"Time until draw: {Game.matchTimeLeft / 60 / 60}:{(Game.matchTimeLeft / 60 % 60).ToString("D2")}";
            }

            if (!string.IsNullOrEmpty(timeText))
            {
                Utils.DrawBorderString(Main.spriteBatch, timeText, position, textColor);
            }
        }
        
    }

}