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
    public class GameJoinCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "joingame";
        public override string Usage => "/joingame <hostName>";
        public override string Description => "Joins a game hosted by the specified player.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                caller.Reply("Usage: " + Usage, Color.Red);
                return;
            }
            string hostName = args[0];

            Game targetGame = caller.Player.GetModPlayer<AdminPlayer>().game;
            if (targetGame == null)
            {
                caller.Reply($"No active game hosted by '{hostName}' was found.", Color.Red);
                return;
            }

            var myPlayer = caller.Player.GetModPlayer<MyPlayer>();
            if (myPlayer.currentGame != null)
            {
                caller.Reply("You are already in a game. Use /leavegame first.", Color.Red);
                return;
            }

            targetGame.players.Add(caller.Player);

            if (targetGame.match == null)
            {
                myPlayer.EnterSpectatorState(targetGame);
                caller.Reply($"Joined game as a spectator. Wait for matches to start.", Color.Cyan);
            }
            else
            {
                myPlayer.EnterClassSelectionState();
                caller.Reply($"Joined ongoing match. You are now in class selection.", Color.Yellow);
            }
        }

    }
}