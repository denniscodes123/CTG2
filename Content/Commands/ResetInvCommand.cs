using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CTG2.Content.ClientSide;
using ClassesNamespace;
using Microsoft.Xna.Framework;

namespace CTG2.Content.Commands
{
    public class ResetInvCommand : ModCommand
    {
        public override string Command => "resetinv";
        public override CommandType Type => CommandType.Chat;
        public override string Description => "Resets your inventory to your current class loadout";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Player player = caller.Player;
            

            try
            {
                var classSystem = player.GetModPlayer<ClassSystem>();
                classSystem.ForceClassReset();
                
                caller.Reply($"Inventory will reset to {PlayerManager.currentClass.Name} class loadout!", Color.Green);
            }
            catch (System.Exception ex)
            {
                caller.Reply($"Failed to reset inventory: {ex.Message}", Color.Red);
            }
        }
    }
}