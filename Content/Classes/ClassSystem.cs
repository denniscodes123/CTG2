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


namespace ClassesNamespace
{
    public class ItemData
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public int Stack { get; set; }
        public int Prefix { get; set; }
        public int Slot { get; set; }
    }

    public enum GameClass : int
    {   
        None,
        Archer,
        Ninja,
        Beast,
        Gladiator,
        Paladin,
        JungleMan,
        BlackMage,
        Psychic,
        WhiteMage,
        Miner,
        Fish,
        Clown,
        FlameBunny,
        TikiPriest,
        Tree,
        RushMutant,
        RegenMutant,
        Leech
    }

    public class CtgClass
    {   
        public int HealthPoints { get; set; }
        public int ManaPoints { get; set; }
        public List<ItemData> InventoryItems { get; set; }
    }
    
    public class ClassSystem : ModPlayer
    {
        public GameClass playerClass = GameClass.None;
        private GameClass lastPlayerClass = GameClass.None;
        private int currentHP = 100;
        string path = "";
        string inventoryData = "";
        List<ItemData> classData;

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
        }
        
        private void SpawnCustomItem(int itemID, int? prefix = null, int? damage = null, int? useTime = null, int? useAnimation = null, float? scale = null, float? knockBack = null, int? shoot = null, float? shootSpeed = null, Color? colorOverride = null)
        {
            Item item = new Item();
            item.SetDefaults(itemID);

            if (prefix.HasValue) item.Prefix(prefix.Value);
            if (damage.HasValue) item.damage = damage.Value;
            if (useTime.HasValue) item.useTime = useTime.Value;
            if (useAnimation.HasValue) item.useAnimation = useAnimation.Value;
            if (scale.HasValue) item.scale = scale.Value;
            if (knockBack.HasValue) item.knockBack = knockBack.Value;
            if (shoot.HasValue) item.shoot = shoot.Value;
            if (shootSpeed.HasValue) item.shootSpeed = shootSpeed.Value;
            if (colorOverride.HasValue) item.color = colorOverride.Value;

            Player.QuickSpawnItem(null, item, 1);
        }


        public override void ResetEffects()
        {

            Player.AddBuff(BuffID.Shine, 54000);
            Player.AddBuff(BuffID.NightOwl, 54000);
            Player.AddBuff(BuffID.Builder, 54000);


            if (playerClass != lastPlayerClass)
            {   
                string selectedClass = playerClass.ToString().ToLower();
                using (var stream = Mod.GetFileStream($"Content/Classes/{selectedClass}.json"))
                using (var fileReader = new StreamReader(stream))
                {
                    var jsonData = fileReader.ReadToEnd();
                    try
                    {
                        var classInfo = JsonSerializer.Deserialize<CtgClass>(jsonData);
                        classData = classInfo.InventoryItems;
                        currentHP = classInfo.HealthPoints;
                        // currentMana = classInfo.ManaPoints;
                    }
                    catch
                    {
                        Main.NewText("Failed to load or parse inventory file.", Microsoft.Xna.Framework.Color.Red);
                        return;
                    }
                }

                SetInventory(classData);
            }

            Player.statLifeMax2 = currentHP;
            if (playerClass != lastPlayerClass) Player.statLife = currentHP;

            lastPlayerClass = playerClass;
        }
    }
}
