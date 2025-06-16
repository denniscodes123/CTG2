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
    public class MatchJoinCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "joinmatch";
        public override string Description => "Join the current match from spectator mode.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            var myPlayer = caller.Player.GetModPlayer<MyPlayer>();

            if (myPlayer.currentGame?.match == null)
            {
                caller.Reply("There is no active match to join.", Color.Red);
                return;
            }

            if (myPlayer.currentState != PlayerState.Spectator)
            {
                caller.Reply("You must be spectating to join the match.", Color.Red);
                return;
            }

            myPlayer.currentGame.match.addPlayer(caller.Player);

            myPlayer.EnterClassSelectionState();

            caller.Reply("You have joined the match. Please select a class.", Color.Green);
        }
    }
}