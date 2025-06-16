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
    public class GameStartCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "startgame";
        public override string Description => "Creates a game (equivalent to hosting a game in PG)";

        public override void Action(CommandCaller caller, string input, string[] args)
        {

            var modPlayer = caller.Player.GetModPlayer<AdminPlayer>();
            if (!modPlayer.IsAdmin)
            {
                caller.Reply("You must be an admin to use this command.", Color.Red);
                return;
            }

            if (modPlayer.game != null)
            {
                caller.Reply("you are in a game end it to start a new one", Color.Red);
                return;
            }

            Game game = new Game();
            modPlayer.game = game;

            game.players.Add(caller.Player);
            var myPlayer = caller.Player.GetModPlayer<MyPlayer>();
            myPlayer.EnterSpectatorState(game);
            //TODO
            //SpawnPoints.TeleportToGameSpawn(caller.Player, game); redo this method 

        }
    }
}