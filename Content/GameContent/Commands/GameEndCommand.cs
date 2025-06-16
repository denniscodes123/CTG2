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
using System.Security.Policy;




namespace CTG2.Content
{
    public class EndGame : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "endgame";
        public override string Usage => "/endgame";
        public override string Description => "Ends a game instance and removes it.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {

            ModPacket myPacket = Mod.GetPacket();
            myPacket.Write((byte)MessageType.RequestEndGame); // id
            myPacket.Send();

            Game gameToEnd = caller.Player.GetModPlayer<AdminPlayer>().game;

            if (gameToEnd == null)
            {
                caller.Reply($"No active game found with the name .", Color.Red);
                return;
            }

            // End the match if one is running
            if (gameToEnd.match != null)
            {
                gameToEnd.EndMatch();
            }

            // Remove the game from the handler
            caller.Player.GetModPlayer<AdminPlayer>().game = null;
            GameHandler.ActiveGames.Remove(gameToEnd);
            gameToEnd.endGame();
            caller.Reply($"Successfully removed game:", Color.Orange);
        }
    }
}