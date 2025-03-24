using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;


namespace CTG2.Content.Commands
{
    public class TeamSetCommand : ModCommand
    {
        private static readonly Dictionary<int, int> playerTeamAssignments = new();

        public override CommandType Type => CommandType.Chat;
        public override string Command => "teamset";
        public override string Usage => "/teamset <playerName> <teamColor>";
        public override string Description => "Set a player to a specific team.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 2)
            {
                caller.Reply("Usage: /teamset <playerName> <teamColor>", Color.Red);
                return;
            }

            string targetName = args[0].ToLower();
            string teamColor = args[1].ToLower();


            Player target = Main.player.FirstOrDefault(p => p.active && p.name.ToLower() == targetName);

            if (target == null)
            {
                caller.Reply($"Player '{targetName}' not found.", Color.Red);
                return;
            }


            int teamID = teamColor switch
            {
                "red" => 1,
                "green" => 2,
                "blue" => 3,
                "yellow" => 4,
                "pink" => 5,
                "none" => 0,
                _ => -1
            };

            if (teamID == -1)
            {
                caller.Reply($"Invalid team color '{teamColor}'. Valid: red, green, blue, yellow, pink, none.", Color.Red);
                return;
            }



target.team = teamID;
playerTeamAssignments[target.whoAmI] = teamID;
target.GetModPlayer<CTGPlayer>().LockTeam(teamID);



            if (Main.netMode == NetmodeID.Server)
            {
            NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, target.whoAmI);
            }

            caller.Reply($"Set player '{target.name}' to the {teamColor} team.", Color.Green);
        }
    }
}
