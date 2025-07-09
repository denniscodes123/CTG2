using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CTG2.Content.Commands
{
    public class KillCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "kill";
        public override string Usage => "/kill <player_name>";
        public override string Description => "Kill a player by name (admin only).";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            var modPlayer = caller.Player.GetModPlayer<AdminPlayer>();
            if (!modPlayer.IsAdmin)
            {
                caller.Reply("You must be an admin to use this command.", Color.Red);
                return;
            }
            if (args.Length == 0)
            {
                caller.Reply("Usage: /kill <player_name>", Color.Red);
                return;
            }

            string targetPlayerName = string.Join(" ", args);
            
            if (string.IsNullOrWhiteSpace(targetPlayerName))
            {
                caller.Reply("Player name cannot be empty!", Color.Red);
                return;
            }

            // Send request to server to kill the target player
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)MessageType.RequestKill);
            packet.Write(caller.Player.whoAmI);
            packet.Write(targetPlayerName);
            packet.Send();
            
            caller.Reply($"Requesting to kill player '{targetPlayerName}'...", Color.Yellow);
        }
    }
}