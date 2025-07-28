using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;
using CTG2.Content.ServerSide;



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
                ModPacket packet = ModContent.GetInstance<CTG2>().GetPacket();
                packet.Write((byte)MessageType.RequestGamemodeChange);
                packet.Write("pubs");
                packet.Send();

            }
            if (args[0].ToLower() == "scrims")
            {
                ModPacket packet = ModContent.GetInstance<CTG2>().GetPacket();
                packet.Write((byte)MessageType.RequestGamemodeChange);
                packet.Write("scrims");
                packet.Send();
            }
            else
            {
                caller.Reply($"Unknown gamemode: {args[0]}", Color.Red);
            }
        }
    }
}

