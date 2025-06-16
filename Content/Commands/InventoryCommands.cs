using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;


namespace CTG2.Content.Commands
{
    public class ItemData
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public int Stack { get; set; }
        public int Prefix { get; set; }
        public int Slot { get; set; }
    }


    public class SaveInventoryCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "saveinventory";
        public override string Usage => "/saveinventory";
        public override string Description => "Saves current inventory to a JSON file";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Player player = caller.Player;
            var inventoryData = new List<ItemData>();

            int count = 0;

            foreach (Item item in player.inventory)
            {
                if (item != null)
                {
                    inventoryData.Add(new ItemData
                    {
                        Name = item.Name,
                        Type = item.type,
                        Stack = item.stack,
                        Prefix = item.prefix,
                        Slot = count
                    });
                }

                count++;
            }

            foreach (Item item in player.armor)
            {
                if (item != null)
                {
                    inventoryData.Add(new ItemData
                    {
                        Name = item.Name,
                        Type = item.type,
                        Stack = item.stack,
                        Prefix = item.prefix,
                        Slot = count
                    });
                }

                count++;
            }

            string[] inputSplit = input.Split(' ');

            string json = JsonSerializer.Serialize(inventoryData, new JsonSerializerOptions { WriteIndented = true });
            string path = Path.Combine(Main.SavePath, "ModLoader", "InventorySaves");
            Directory.CreateDirectory(path);

            string filePath = Path.Combine(path, $"{inputSplit[1]}.json");
            File.WriteAllText(filePath, json);

            Main.NewText($"Inventory saved to {filePath}", Color.LightGreen);
        }
    }
    
    public class LoadInventoryCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "loadinventory";
        public override string Usage => "/loadinventory";
        public override string Description => "Loads saved inventory from a JSON file";

        public override void Action(CommandCaller caller, string input, string[] args) {

            string[] inputSplit = input.Split(' ');
            string modifiedInput = inputSplit[1];

            Player player = caller.Player;
            string path = Path.Combine(Main.SavePath, "ModLoader", "InventorySaves");
            string filePath = Path.Combine(path, $"{modifiedInput}.json");

            if (!File.Exists(filePath))
            {
                Main.NewText($"Inventory file not found.", Microsoft.Xna.Framework.Color.Red);
                return;
            }

            string json = File.ReadAllText(filePath);
            List<ItemData> inventoryData;

            try
            {
                inventoryData = JsonSerializer.Deserialize<List<ItemData>>(json);
            }
            catch
            {
                Main.NewText("Failed to load or parse inventory file.", Microsoft.Xna.Framework.Color.Red);
                return;
            }

            // Load items

            for (int b = 0; b < player.inventory.Length; b++)
            {
                var itemData = inventoryData[b];
                Item newItem = new Item();
                newItem.SetDefaults(itemData.Type);
                newItem.stack = itemData.Stack;
                newItem.Prefix(itemData.Prefix);

                player.inventory[b] = newItem;
            }

            for (int d = 0; d < player.armor.Length; d++)
            {
                var itemData = inventoryData[player.inventory.Length + d];
                Item newItem = new Item();
                newItem.SetDefaults(itemData.Type);
                newItem.stack = itemData.Stack;
                newItem.Prefix(itemData.Prefix);

                player.armor[d] = newItem;
            }

            Main.NewText("Inventory loaded successfully.", Microsoft.Xna.Framework.Color.LightGreen);
        }
    }
}
