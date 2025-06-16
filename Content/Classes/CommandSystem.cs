using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CTG2.Content;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;


public class ClassCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;
    public override string Command => "class";
    public override string Description => "Select a player class";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (GameUI.matchTimer - (int)(Main.GameUpdateCount / 60) < 0) //!CTG2.Content.Game.preparationPhase
        {
            caller.Reply("You can only select a class after the match has started! Use /start first.", Color.Red);
            return;
        }

        if (args.Length < 1 || !int.TryParse(args[0], out int classType))
        {
            caller.Reply("Usage: /class [number]", Color.Red);
            return;
        }

        Player player = caller.Player;
        var modPlayer = player.GetModPlayer<ClassSystem>();

        switch (classType)
        {
            case 1:
                modPlayer.playerClass = 1;
                caller.Reply("You are now an Archer!", Color.Green);
                break;

            case 2:
                modPlayer.playerClass = 2;
                caller.Reply("You are now ninja!", Color.Green);
                break;

            case 3:
                modPlayer.playerClass = 3;
                caller.Reply("You are now the beast!", Color.Green);
                break;

            case 4:
                modPlayer.playerClass = 4;
                caller.Reply("You are now the gladiator!", Color.Green);
                break;

            case 5:
                modPlayer.playerClass = 5;
                caller.Reply("You are now paladin!", Color.Green);
                break;

            case 6:
                modPlayer.playerClass = 6;
                caller.Reply("You are now jman!", Color.Green);
                break;

            case 7:
                modPlayer.playerClass = 7;
                caller.Reply("You are now bmage!", Color.Green);
                break;

            case 8:
                modPlayer.playerClass = 8;
                caller.Reply("You are now a psychic!", Color.Green);
                break;

            case 9:
                modPlayer.playerClass = 9;
                caller.Reply("You are now wmage!", Color.Green);
                break;

            case 10:
                modPlayer.playerClass = 10;
                caller.Reply("You are now miner!", Color.Green);
                break;

            case 11:
                modPlayer.playerClass = 11;
                caller.Reply("You are now fish!", Color.Green);
                break;

            case 12:
                modPlayer.playerClass = 12;
                caller.Reply("You are now clown!", Color.Green);
                break;

            case 13:
                modPlayer.playerClass = 13;
                caller.Reply("You are now fbunny!", Color.Green);
                break;

            case 14:
                modPlayer.playerClass = 14;
                caller.Reply("You are now tiki priest!", Color.Green);
                break;

            case 15:
                modPlayer.playerClass = 15;
                caller.Reply("You are now tree!", Color.Green);
                break;

            case 16:
                modPlayer.playerClass = 16;
                caller.Reply("You are now mutant!", Color.Green);
                break;

            case 17:
                modPlayer.playerClass = 17;
                caller.Reply("You are now leech!", Color.Green);
                break;


            default:
                caller.Reply("Invalid class. Available: 1 (Archer), 2 (Warrior), 3 (Mage)", Color.Red);
                break;
        }
    }


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
