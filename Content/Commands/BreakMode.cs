using Terraria.ModLoader;
using CTG2.Content;

namespace CTG2.Commands
{
    public class BreakMode : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "breakmode";
        public override string Description => "Toggles block breaking protection";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            UnbreakableTiles.AllowBreaking = !UnbreakableTiles.AllowBreaking;

            string status = UnbreakableTiles.AllowBreaking ? "enabled" : "disabled";
            caller.Reply($"[CTG] Break mode is now {status}.");
        }
    }
}
