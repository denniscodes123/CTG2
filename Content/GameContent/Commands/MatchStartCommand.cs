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
    public class MatchStartCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "matchstart";
        public override string Description => "starts a match (equivalent to hosting a game in PG)";

        public override void Action(CommandCaller caller, string input, string[] args)
        {

            var modPlayer = caller.Player.GetModPlayer<AdminPlayer>();
            if (!modPlayer.IsAdmin)
            {
                caller.Reply("You must be an admin to use this command.", Color.Red);
                return;
            }

            if (modPlayer.game == null)
            {
                caller.Reply("Game has not been created", Color.Red);
                return;
            }
            if (modPlayer.game.match != null)
            {
                caller.Reply("Match in progress", Color.Red);
                return;
            }

            modPlayer.game.StartMatch();
        }
    }
}