using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

/*

namespace CTG2.Content.Commands
{
    public class GamemodeCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "gamemode";
        public override string Description => "Set game mode (e.g., /gamemode pubs)";
        public override string Usage => "/gamemode <mode>";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            var modPlayer = caller.Player.GetModPlayer<AdminPlayer>();
            if (!modPlayer.IsAdmin)
            {
                caller.Reply("You must be an admin to use this command.", Color.Red);
                return;
            }

            if (args.Length != 1)
            {
                caller.Reply("Usage: /gamemode <mode>", Color.Red);
                return;
            }

            if (args[0].ToLower() == "pubs")
            {
                ModContent.GetInstance<ServerSide.GameManager>().EnablePubsMode();
                caller.Reply("Pubs mode has been enabled.", Color.Green);
            }
            else
            {
                caller.Reply($"Unknown gamemode: {args[0]}", Color.Red);
            }
        }
    }
}
*/
