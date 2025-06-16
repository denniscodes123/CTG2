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
    public class GameLeaveCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "leavegame";
        public override string Description => "Leaves the game you are currently in.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {

            
            var modPlayer = caller.Player.GetModPlayer<AdminPlayer>();
            if (modPlayer.IsAdmin)
            {
                caller.Reply("You must end the game to leave it", Color.Red);
                return;
            }


            var myPlayer = caller.Player.GetModPlayer<MyPlayer>();
            
            if (myPlayer.currentGame == null)
            {
                caller.Reply("You are not currently in a game.", Color.Red);
                return;
            }

            Game gameToLeave = myPlayer.currentGame;
            myPlayer.EnterLobbyState();
            // Remove player from the game's lists
            gameToLeave.players.Remove(caller.Player);
            gameToLeave.match?.removePlayer(caller.Player); // Safely remove from match if it exists

            // Transition the player back to the lobby state.
            

            caller.Reply($"You have left the game.", Color.Orange);
        }
    

    }
}