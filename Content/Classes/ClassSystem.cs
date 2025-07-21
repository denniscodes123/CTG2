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
using CTG2;
using CTG2.Content.ClientSide;
using Terraria.Localization;
using Terraria.Chat;


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
        private string lastPlayerClass = "";
        private string lastUpgrade = "";
        private int bonusHP = 0;
        private int bonusRegen = 0;
        private int bonusDef = 0;
        private float bonusMoveSpeed = 0;
        public int clownSwapCaller = -1; //Gets updated by clownonuse to store who the caller is

        private int currentHP = 100;
        private int currentMana = 20;

        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);

            health = new StatModifier(0f, 0f, 0f, 0f);
            health.Flat = currentHP;

            mana = new StatModifier(0f, 0f, 0f, 0f);
            mana.Flat = currentMana;
        }

        public override void OnEnterWorld()
        {
            base.OnEnterWorld();
            Player.ConsumedLifeFruit = 0;

            for (int i = Player.buffType.Length - 1; i >= 0; i--)
            {
                Player.DelBuff(i);
            }



            //Comment this out if you want this to not clear all inventory onenterwolrd
            //Maybe makes this check if the player is in the current game before doing this in case of disconnects 
            for (int i = 0; i < Player.inventory.Length; i++)
                Player.inventory[i] = new Item();

            for (int i = 0; i < Player.armor.Length; i++)
                Player.armor[i] = new Item();

            for (int i = 0; i < Player.miscEquips.Length; i++)
                Player.miscEquips[i] = new Item();

            for (int i = 0; i < Player.dye.Length; i++)
                Player.dye[i] = new Item();

            for (int i = 0; i < Player.miscDyes.Length; i++)
                Player.miscDyes[i] = new Item();

            Player.trashItem = new Item();

            NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, Player.whoAmI); //sync state 
        }

        private void SetInventory(CtgClass classData)
        {
            for (int i = Player.buffType.Length - 1; i >= 0; i--)
            {
                Player.DelBuff(i);
            }
            ApplyPermBuffs();
            
            //Player.statLife = classData.HealthPoints; set this after modifymax
            //Player.statLife = classData.HealthPoints; set this after modifymax
            Player.statManaMax2 = classData.ManaPoints;
            Player.statManaMax = classData.ManaPoints;
            Player.statMana = classData.ManaPoints;

            currentHP = classData.HealthPoints;
            currentMana = classData.ManaPoints;

            currentHP = classData.HealthPoints;
            

            Console.WriteLine($"Player hp is {currentHP}");
            Console.WriteLine($" {Player.name} - HP: {Player.statLife}/{Player.statLifeMax}, Mana: {Player.statMana}/{Player.statManaMax2}");
            List<ItemData> classItems = classData.InventoryItems;

            for (int b = 0; b < Player.inventory.Length; b++)
            {
                var itemData = classItems[b];
                Item newItem = new Item();
                newItem.SetDefaults(itemData.Type);
                newItem.stack = itemData.Stack;
                newItem.Prefix(itemData.Prefix);

                Player.inventory[b] = newItem;
            }

            for (int c = 0; c < Player.armor.Length; c++)
            {
                var itemData = classItems[Player.inventory.Length + c];
                Item newItem = new Item();
                newItem.SetDefaults(itemData.Type);
                newItem.stack = itemData.Stack;
                newItem.Prefix(itemData.Prefix);

                Player.armor[c] = newItem;
            }

            for (int d = 0; d < Player.miscEquips.Length; d++)
            {
                var itemData = classItems[Player.inventory.Length + Player.armor.Length + d];
                Item newItem = new Item();
                newItem.SetDefaults(itemData.Type);
                newItem.stack = itemData.Stack;
                newItem.Prefix(itemData.Prefix);

                Player.miscEquips[d] = newItem;
            }

            for (int e = 0; e < Player.miscDyes.Length; e++)
            {
                var itemData = classItems[Player.inventory.Length + Player.armor.Length + Player.miscEquips.Length + e];
                Item newItem = new Item();
                newItem.SetDefaults(itemData.Type);
                newItem.stack = itemData.Stack;
                newItem.Prefix(itemData.Prefix);

                Player.miscDyes[e] = newItem;
            }
            /*this may not work lol
            */
            
            currentMana = classData.ManaPoints;
            Player.statMana = currentMana;
            

            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{currentHP}, {Player.statLife}"), Microsoft.Xna.Framework.Color.Olive);
            NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, Player.whoAmI);
            SyncPlayer(-1, Player.whoAmI);
            
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

        private void ApplyPermBuffs()
        {
            var playerManager = Player.GetModPlayer<PlayerManager>();
            var classData = playerManager?.currentClass;

            if (classData?.Buffs != null)
            {
                foreach (int buffId in classData.Buffs)
                {
                    if (!Player.HasBuff(buffId))
                        Player.AddBuff(buffId, 54000);
                }
            }
        }


        public void ApplyUpgrade(UpgradeConfig upgrade)
        {
            // reset all bonus stats
            bonusHP = 0;
            bonusRegen = 0;
            bonusDef = 0;
            bonusMoveSpeed = 0;

            switch (upgrade.Id)
            {
                case "charge_bow":
                    Item newItem = new Item();
                    var itemType = 5549;
                    newItem.SetDefaults(itemType);
                    newItem.stack = 1;
                    newItem.prefix = 0;
                    Player.inventory[2] = newItem;
                    break;
                case "bonus_regen":
                    bonusRegen += 2 * upgrade.Value;
                    break;
                case "bonus_speed":
                    bonusMoveSpeed += upgrade.Value / 100f;
                    break;
                case "bonus_health":
                    bonusHP += upgrade.Value;
                    break;
                case "bonus_def":
                    bonusDef += upgrade.Value;
                    break;
                default:
                    Main.NewText("upgrade id not found");
                    break;
            }
            
        }

        public void setClass()
        {
            CtgClass classInfo;
            var playerManager = Player.GetModPlayer<PlayerManager>();
            if (playerManager.currentClass.Inventory != lastPlayerClass && GameInfo.matchStage != 0) //make this run only during matchstages or defaults to archer.json and onenterworld can never be run
            {

            for (int i = Player.buffType.Length - 1; i >= 0; i--)
            {
                Player.DelBuff(i);
            }


                string selectedClass = playerManager.currentClass.Inventory;
                using (var stream = Mod.GetFileStream($"Content/Classes/{selectedClass}.json"))
                using (var fileReader = new StreamReader(stream))
                {
                    var jsonData = fileReader.ReadToEnd();
                    try
                    {
                        classInfo = JsonSerializer.Deserialize<CtgClass>(jsonData);
                    }
                    catch
                    {
                        Main.NewText("Failed to load or parse inventory file.", Microsoft.Xna.Framework.Color.Red);
                        return;
                    }
                }

                SetInventory(classInfo);

                // Apply First upgrade by default when new class selected
                //ApplyUpgrade(playerManager.currentClass.Upgrades[0]);
            }
        }


        public override void ResetEffects()
        {
            // THIS METHOD IS BROKEN NEED TO FIX 
            Player.AddBuff(BuffID.Shine, 54000);
            Player.AddBuff(BuffID.NightOwl, 54000);
            Player.AddBuff(BuffID.Builder, 54000);


            Player.statManaMax2 = currentMana;
            // Set base stats first
            // Player.statLifeMax2 = currentHP;
            // Player.statManaMax2 = currentMana;

            // if (playerManager.currentUpgrade.Name != lastUpgrade)
            // {
            //     // apply upgrades here
            //     ApplyUpgrade(playerManager.currentUpgrade);
            // }

            // Player.lifeRegen += bonusRegen;
            // Player.moveSpeed += bonusMoveSpeed;
            // Player.statDefense += bonusDef;
            // Player.statLifeMax2 += bonusHP;

            // // Update currentHP to include bonuses for proper sync
            // currentHP = Player.statLifeMax2;
            // currentMana = Player.statManaMax2;

            // Add player buffs here instead (delete switch once config is populated with the required buffs)
           /* try
            {
                var playerManager = Player.GetModPlayer<PlayerManager>();
                if (!Main.dedServ) ApplyPermBuffs(playerManager.currentClass.Buffs);
            }
            catch
            {
                Console.WriteLine("Failed to apply permanent buffs.");
            }
*/

            
        }
        public override void OnRespawn()
        {
            ApplyPermBuffs();
            Player.Heal(600);
        }

        public override void UpdateEquips()
        {
            if (Main.expertMode || Main.masterMode)
            {
                Player.extraAccessory = true;
            }
        }

        public override void PostUpdate()
        {
            if (Main.GameUpdateCount % 240 != 0) //replace dye after removal every 4 seconds
                return;
            //this is where dyes are set and forced on 
            int redDyeType = 1031;
            int blueDyeType = 1035;

            var playerManager = Player.GetModPlayer<PlayerManager>();
            // Check if player has a class selected and is on a team
            if (playerManager.currentClass != null && !string.IsNullOrEmpty(playerManager.currentClass.Name) && Player.team != 0)
            {
                int dyeID = Player.team switch
                {
                    1 => redDyeType,
                    3 => blueDyeType,
                    _ => 0
                };
                for (int i = 0; i <= 9; i++) //i<=3 just sets the armor for not can switch to i<=9 for all accessory slots later
                {
                    if (Player.dye[i] == null || Player.dye[i].type != dyeID)
                    {
                        Player.dye[i] = dyeID == 0 ? new Item() : new Item(dyeID);
                    }
                }
            }
        }

        public void SyncPlayerStats()
        {
            // First ensure the player's max stats are updated
            // Player.statLifeMax = currentHP;
            // Player.statLifeMax2 = currentHP;


            // // Set current life/mana to max if they're above the new max
            // if (Player.statLife > currentHP) Player.statLife = currentHP;
            // if (Player.statMana > currentMana) Player.statMana = currentMana;

            // Sync to ALL clients (including self)


            //NetMessage.SendData(MessageID.PlayerLifeMana, -1, -1, null, Player.whoAmI, Player.statLife, Player.statLifeMax, Player.statMana, Player.statManaMax);
            NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, Player.whoAmI);

            //Console.WriteLine($"ClassSystem: Synced stats for {Player.name} - HP: {Player.statLife}/{Player.statLifeMax}, Mana: {Player.statMana}/{Player.statManaMax}");
        }
        public void SyncPlayer(int toWho, int fromWho)
        {
            ModPacket packet = ModContent.GetInstance<global::CTG2.CTG2>().GetPacket(); ;
            packet.Write((byte)MessageType.RequestSyncStats);
            packet.Write((byte)Player.whoAmI);
            packet.Write(currentHP);
            packet.Write(currentMana);
            packet.Write(bonusHP);
            packet.Write(bonusRegen);
            packet.Write(bonusDef);
            packet.Write(bonusMoveSpeed);
            var playerManager = Player.GetModPlayer<PlayerManager>();
            packet.Write((byte)playerManager.playerState);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveSync(BinaryReader reader)
        {
            currentHP = reader.ReadInt32();
            currentMana = reader.ReadInt32();
            bonusHP = reader.ReadInt32();
            bonusRegen = reader.ReadInt32();
            bonusDef = reader.ReadInt32();
            bonusMoveSpeed = reader.ReadSingle();
            var playerManager = Player.GetModPlayer<PlayerManager>();
            playerManager.playerState = (PlayerManager.PlayerState)reader.ReadInt32(); // new
        }
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
        var clone = (ClassSystem)clientPlayer;
        var clientPlayerManager = clientPlayer.Player.GetModPlayer<PlayerManager>();
        var localPlayerManager = this.Player.GetModPlayer<PlayerManager>();

        if (currentHP != clone.currentHP || currentMana != clone.currentMana || bonusHP != clone.bonusHP || clientPlayerManager.playerState != localPlayerManager.playerState)
        {
            SyncPlayer(-1, Main.myPlayer); //this is causing a lot of lag
        }

        }
        public override void CopyClientState(ModPlayer targetCopy)
        {
            ClassSystem clone = (ClassSystem)targetCopy;
            clone.currentHP = currentHP;
            clone.currentMana = currentMana;
            clone.bonusHP = bonusHP;
            clone.bonusRegen = bonusRegen;
            clone.bonusDef = bonusDef;
            clone.bonusMoveSpeed = bonusMoveSpeed;
            PlayerManager playerManager = targetCopy.Player.GetModPlayer<PlayerManager>();
            playerManager.playerState = this.Player.GetModPlayer<PlayerManager>().playerState; 
        }



        


    }

}
