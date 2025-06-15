using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CTG2.Content.Commands
{
    public class LoginCommand : ModCommand
    {
        private const string Password = "neededuonthedefense"; 

        public override CommandType Type => CommandType.Chat;
        public override string Command => "login";
        public override string Description => "Log in as admin.";
        public override string Usage => "/login <password>";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length != 1)
            {
                caller.Reply("Usage: /login <password>", Color.Red);
                return;
            }

            if (args[0] == Password)
            {
                var modPlayer = caller.Player.GetModPlayer<AdminPlayer>();
                modPlayer.IsAdmin = true;
                caller.Reply("You are now logged in as admin.", Color.Green);
            }
            else
            {
                caller.Reply("Incorrect password.", Color.Red);
            }
        }
    }
}
