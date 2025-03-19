using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Linq;
using Microsoft.Xna.Framework;


namespace CTG2.Content.Commands
{
    public class ItemCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat; // Allows the command in chat
        public override string Command => "item"; // The actual command, like "/item"
        public override string Description => "Gives an item by name or ID.";
        public override string Usage => "/item <itemNameOrID> [amount]";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                caller.Reply("Usage: /item <itemNameOrID> [amount]", Color.Red);
                return;
            }

            Player player = caller.Player;
            int amount = 1; // Default to giving 1 item

            // If a second argument exists, try to parse it as an amount
            if (args.Length > 1 && !int.TryParse(args[1], out amount))
            {
                caller.Reply("Invalid quantity. Please enter a valid number.", Color.Red);
                return;
            }

            int itemType = -1;

            // Check if input is a valid numeric ID
            if (int.TryParse(args[0], out int parsedID))
            {
                if (ContentSamples.ItemsByType.ContainsKey(parsedID))
                {
                    itemType = parsedID;
                }
            }
            else // Otherwise, try to match the name
            {
                string searchName = args[0].ToLower();
                var match = ContentSamples.ItemsByType.Values
                    .FirstOrDefault(item => item.Name.ToLower() == searchName);

                if (match != null)
                {
                    itemType = match.type;
                }
            }

            if (itemType != -1)
            {
                player.QuickSpawnItem(null, itemType, amount);
                caller.Reply($"Gave {amount}x {Lang.GetItemNameValue(itemType)}.", Color.Green);
            }
            else
            {
                caller.Reply("Item not found. Try using an item ID or exact name.", Color.Red);
            }
        }
    }
}
