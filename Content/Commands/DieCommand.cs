using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CTG2.Content.Commands
{
    public class DieCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "die";
        public override string Usage => "/die";
        public override string Description => "Kill yourself.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length > 0)
            {
                caller.Reply("Usage: /die", Color.Red);
                return;
            }

            // Send request to server to kill this player
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)MessageType.RequestDie);
            packet.Write(caller.Player.whoAmI);
            packet.Send();
            
            caller.Reply("Requesting death...", Color.Gray);
        }
    }
}