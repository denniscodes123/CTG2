using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CTG2.Content.Items;
using CTG2.Content.Items.ModifiedWeps;
using Microsoft.Xna.Framework; 
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Runtime.CompilerServices;


public class ItemData
{
    public string Name { get; set; }
    public int Type { get; set; }
    public int Stack { get; set; }
    public int Prefix { get; set; }
    public int Slot { get; set; }
}


public class ClassSystem : ModPlayer
{
    public int playerClass = 0;
    private bool hasReceivedItems = false;
    private bool hasHealed = false;
    private bool hasClearedInv = false;


    // CLASS HP
    int archerHP = 160;
    int ninjaHP = 160;
    int beastHP = 160;
    int gladiatorHP = 140;
    int paladinHP = 160;
    int jungleManHP = 140;
    int blackMageHP = 160;
    int psychicHP = 160;
    int whiteMageHP = 160;
    int minerHP = 180;
    int fishHP = 150;
    int clownHP = 120;
    int flameBunnyHP = 160;
    int tikiPriestHP = 160;
    int treeHP = 160;
    int mutantHP = 160;
    int leechHP = 140;

    public override void ResetEffects()
    {

        Player.AddBuff(BuffID.Shine, 54000);
        Player.AddBuff(BuffID.NightOwl, 54000);
        Player.AddBuff(BuffID.Builder, 54000);

        if (!hasClearedInv)
        {
            for (int i = 0; i < Player.inventory.Length; i++) Player.inventory[i] = new Item();
            hasClearedInv = true;
        }

        switch (playerClass) //permabuffs are put here
        {
            case 1: // Archer
                Player.statLifeMax2 = archerHP;
                GiveClassItems();
                
                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }

                break;

            case 2: // Ninja
                Player.statLifeMax2 = ninjaHP;
                GiveClassItems();

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                break;

            case 3: // Beast
                Player.statLifeMax2 = beastHP;
                GiveClassItems();

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                break;

            case 4: // Gladiator
                Player.statLifeMax2 = gladiatorHP;
                GiveClassItems();

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                break;

            case 5: // Paladin
                Player.statLifeMax2 = paladinHP;
                GiveClassItems();

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                break;

            case 6: // Jungle Man
                Player.statLifeMax2 = jungleManHP;
                GiveClassItems();

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                break;

            case 7: // Black Mage
                Player.statLifeMax2 = blackMageHP;
                GiveClassItems();

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                break;

            case 8: // Psychic
                Player.statLifeMax2 = psychicHP;
                GiveClassItems();
                break;

            case 9: // White Mage
                Player.statLifeMax2 = whiteMageHP;
                GiveClassItems();
                break;

            case 10: // Miner
                Player.statLifeMax2 = minerHP;
                GiveClassItems();
                break;

            case 11: // Fish
                Player.statLifeMax2 = fishHP;
                GiveClassItems();
                break;

            case 12: // Clown
                Player.statLifeMax2 = clownHP;
                GiveClassItems();
                break;

            case 13: // Flame Bunny
                Player.statLifeMax2 = flameBunnyHP;
                GiveClassItems();
                break;

            case 14: // Tiki Priest
                Player.statLifeMax2 = tikiPriestHP;
                GiveClassItems();
                break;

            case 15: // Tree
                Player.statLifeMax2 = treeHP;
                GiveClassItems();
                break;

            case 16: // Mutant
                Player.statLifeMax2 = mutantHP;
                GiveClassItems();
                break;

            case 17: // Leech
                Player.statLifeMax2 = leechHP;
                GiveClassItems();
                break;
        }
    }

    private void SetInventory(List<ItemData> classData)
    {
        for (int b = 0; b < Player.inventory.Length; b++)
        {
            var itemData = classData[b];
            Item newItem = new Item();
            newItem.SetDefaults(itemData.Type);
            newItem.stack = itemData.Stack;
            newItem.Prefix(itemData.Prefix);

            Player.inventory[b] = newItem;
        }

        for (int d = 0; d < Player.armor.Length; d++)
        {
            var itemData = classData[Player.inventory.Length + d];
            Item newItem = new Item();
            newItem.SetDefaults(itemData.Type);
            newItem.stack = itemData.Stack;
            newItem.Prefix(itemData.Prefix);

            Player.armor[d] = newItem;
        }

        Player.inventory[27] = new Item(1567);
    }

    static string GetPathRelativeToSource(string fileName, [CallerFilePath] string sourceFilePath = "")
    {
        string folder = Path.GetDirectoryName(sourceFilePath);
        return Path.Combine(folder, fileName);
    }

    private void GiveClassItems()
    {
        if (!hasReceivedItems)
        {

            Player.inventory[0] = new Item(ModContent.ItemType<CTG2.Content.Items.ShardstonePickaxe>()); // pickaxe
            Player.inventory[1] = new Item(215); // whoopie cushion
            Player.inventory[9] = new Item(1299); // binoculars
            Player.inventory[11] = new Item(215); // whoopie cushion
            Player.inventory[12] = new Item(507); // bell
            Player.inventory[19] = new Item(1299); // binoculars
            Player.inventory[39] = new Item(271); // familiar wig
            Player.inventory[49] = new Item(2216); // paint sprayer

            switch (playerClass)
            {
                case 1: //archer

                    string path = GetPathRelativeToSource("archer.json");
                    string inventoryData = File.ReadAllText(path);
                    List<ItemData> archerData;

                    try
                    {
                        archerData = JsonSerializer.Deserialize<List<ItemData>>(inventoryData);
                    }
                    catch
                    {
                        Main.NewText("Failed to load or parse inventory file.", Microsoft.Xna.Framework.Color.Red);
                        return;
                    }

                    SetInventory(archerData);

                    break;


                case 2:

                    Player.QuickSpawnItem(null, ModContent.ItemType<CTG2.Content.Items.ShardstonePickaxe>(), 1); // pickaxe
                    Player.QuickSpawnItem(null, 215, 1); // whoopie cushion
                    Player.QuickSpawnItem(null, ModContent.ItemType<CTG2.Content.Items.Zen>(), 1); // ninja's dagger
                    Player.QuickSpawnItem(null, 2, 50); // dirt

                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);



                    Player.QuickSpawnItem(null, 64);
                    Player.QuickSpawnItem(null, 176, 999);
                    break;

                case 3:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);



                    Player.QuickSpawnItem(null, 4347);
                    Player.QuickSpawnItem(null, 1836, 9999);
                    break;

                case 4:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 741);
                    break;

                case 5:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);


                    Player.QuickSpawnItem(null, 165);
                    Player.QuickSpawnItem(null, 4760);
                    break;
                case 6:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    break;
                case 7:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 272);
                    Player.QuickSpawnItem(null, 3103);

                    break;
                case 8:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 3476);

                    break;
                case 9:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 1446);
                    Player.QuickSpawnItem(null, 1266);

                    break;
                case 10:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 235, 20); //need to create increasing bomb logic left at 20 for now
                    Player.QuickSpawnItem(null, 1313);

                    break;
                case 11:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 165);

                    break;
                case 12:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 5147);

                    break;
                case 13:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 119);

                    break;
                case 14:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 788);

                    break;

                case 15:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 740);

                    break;

                case 16:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 220);

                    break;

                case 17:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 272);
                    Player.QuickSpawnItem(null, 670);

                    break;
            }

            hasReceivedItems = true;
        }
    }


    private void SpawnCustomItem(
        int itemID,
        int? prefix = null,
        int? damage = null,
        int? useTime = null,
        int? useAnimation = null,
        float? scale = null,
        float? knockBack = null,
        int? shoot = null,
        float? shootSpeed = null,
        Color? colorOverride = null
    )
    {
        Item item = new Item();
        item.SetDefaults(itemID);

        if (prefix.HasValue)
            item.Prefix(prefix.Value);
        if (damage.HasValue)
            item.damage = damage.Value;
        if (useTime.HasValue)
            item.useTime = useTime.Value;
        if (useAnimation.HasValue)
            item.useAnimation = useAnimation.Value;
        if (scale.HasValue)
            item.scale = scale.Value;
        if (knockBack.HasValue)
            item.knockBack = knockBack.Value;
        if (shoot.HasValue)
            item.shoot = shoot.Value;
        if (shootSpeed.HasValue)
            item.shootSpeed = shootSpeed.Value;
        if (colorOverride.HasValue)
            item.color = colorOverride.Value;

        Player.QuickSpawnItem(null, item, 1);
    }



}
