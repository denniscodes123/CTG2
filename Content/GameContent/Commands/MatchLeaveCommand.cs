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
    public class MatchLeaveCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "leavematch";
        public override string Description => "Leave the current match to become a spectator.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            var myPlayer = caller.Player.GetModPlayer<MyPlayer>();

            if (myPlayer.currentGame?.match == null)
            {
                caller.Reply("You are not currently in an active match.", Color.Red);
                return;
            }

            if (myPlayer.currentState == PlayerState.Spectator)
            {
                caller.Reply("You are already spectating.", Color.Red);
                return;
            }

            myPlayer.currentGame.match.removePlayer(caller.Player);

            myPlayer.EnterSpectatorState(myPlayer.currentGame);

            caller.Reply("You have left the match and are now spectating.", Color.Yellow);

        }
    }
}