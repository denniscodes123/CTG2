using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures; 
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Runtime.CompilerServices;
using ClassesNamespace;
using CTG2.Content.ClientSide;


namespace CTG2.Content
{
    public class Abilities : ModPlayer
    {
        private int class4BuffTimer = 0;
        private bool class4PendingBuffs = false;

        private int class6ReleaseTimer = -1;

        public int class7HitCounter = 0;

        private int class8HP = 0;

        private int class12SwapTimer = -1;
        private int class12ClosestDist = 99999;
        private Player class12ClosestPlayer = null;

        private int class15AbilityTimer = -1;

        private CtgClass class16RushData;
        private CtgClass class16RegenData;
        private bool initializedMutant;
        private int mutantState = 1;


        private void SetInventory(CtgClass classData)
        {
            Player.statLifeMax2 = classData.HealthPoints;
            Player.statManaMax2 = classData.ManaPoints;

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

            for (int d = 0; d < Player.armor.Length; d++)
            {
                var itemData = classItems[Player.inventory.Length + d];
                Item newItem = new Item();
                newItem.SetDefaults(itemData.Type);
                newItem.stack = itemData.Stack;
                newItem.Prefix(itemData.Prefix);

                Player.armor[d] = newItem;
            }
        }


        public override void OnHurt(Player.HurtInfo info)
        {
            int projectileType = info.DamageSource.SourceProjectileType;
            int attackerIndex = info.DamageSource.SourcePlayerIndex;
            
            if (attackerIndex >= 0 && attackerIndex < Main.maxPlayers) {
                Player attacker = Main.player[attackerIndex];
                int damage = info.Damage;

                switch (projectileType)
                {
                    case ProjectileID.HellfireArrow: // Archer ability
                        if (attacker.HasBuff(320) && attacker.team != Player.team)
                        {
                            Player.AddBuff(24, 60);
                            Player.AddBuff(30, 60);
                            Player.AddBuff(32, 60);
                            Player.AddBuff(323, 60);
                        }
                        break;
                    
                    case 15: case 19: // Flame Bunny ability
                        if (attacker.HasBuff(320) && attacker.HasBuff(137) && attacker.team != Player.team)
                        {
                            Player.AddBuff(20, 30);
                            Player.AddBuff(39, 30);
                            Player.AddBuff(44, 30);
                            Player.AddBuff(48, 30);
                        }
                        break;

                    case 273: case 304: // Leech ability
                        if (attacker.HasBuff(320) && attacker.team != Player.team)
                        {
                            int healAmount = damage / 3;
                            attacker.statLife += healAmount;
                            attacker.HealEffect(healAmount);
                        }
                        break;
                    case 496: // Black Mage ability
                        if (!attacker.HasBuff(206) && attacker.team != Player.team)
                        {
                            attacker.GetModPlayer<Abilities>().class7HitCounter++;

                            if (attacker.GetModPlayer<Abilities>().class7HitCounter < 10)
                                Main.NewText($"{attacker.GetModPlayer<Abilities>().class7HitCounter}/10 hits");
                            else {
                                Main.NewText("10/10 hits");
                            }
                        }
                        break;
                }
            }
        }


        private void SetCooldown(int seconds)
        {
            Player.AddBuff(BuffID.ChaosState, seconds * 60);
        }


        private void ArcherOnUse()
        {
            Player.AddBuff(320, 6 * 60);
        }


        private void NinjaOnUse()
        {
            Player.AddBuff(BuffID.Invisibility, 60 * 60);
        }


        private void BeastOnUse()
        {
            var mod = ModContent.GetInstance<CTG2>();
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MessageType.RequestSpawnNpc);
            packet.Write((int)Player.Center.X);
            packet.Write((int)Player.Center.Y);
            packet.Write(ModContent.NPCType<StationaryBeast>());
            packet.Write(Player.team);
            packet.Write(0f);
            packet.Send();
        }
        

        private void GladiatorOnUse()
        {
            Player.AddBuff(206, 300);
            Player.AddBuff(195, 300);
            Player.AddBuff(75, 300);
            Player.AddBuff(320, 300);

            class4BuffTimer = 300;
            class4PendingBuffs = true;
        }


        private void GladiatorPostStatus()
        {
            if (class4PendingBuffs) // runs 5 second interval
            {
                class4BuffTimer--;

                if (class4BuffTimer <= 0)
                {

                    Player.AddBuff(137, 180);
                    Player.AddBuff(32, 180);
                    Player.AddBuff(195, 180);
                    Player.AddBuff(5, 180);
                    Player.AddBuff(215, 180);

                    class4PendingBuffs = false;
                }
            }
        }


        private void PaladinOnUse()
        {   
            foreach (Player other in Main.player)
            {
                if (!other.active || other.dead || other.whoAmI == Player.whoAmI)
                    continue;

                if (Vector2.Distance(Player.Center, other.Center) <= 20 * 16 && Player.team == other.team) // 20 block radius
                {
                    other.AddBuff(58, 200);
                    other.AddBuff(119, 200);
                    other.AddBuff(2, 200);
                    
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 58, 200);
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 119, 200);
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 2, 200);
                }
            }

            Player.AddBuff(58, 100);
            Player.AddBuff(119, 100);
            Player.AddBuff(2, 100);
        }


        private void JungleManOnUse()
        {
            Player.AddBuff(149, 60);
            Player.AddBuff(114, 60);

            class6ReleaseTimer = 60;
        }


        private void JungleManPostStatus()
        {
            class6ReleaseTimer = (class6ReleaseTimer > -1) ? class6ReleaseTimer - 1 : -1;
            
            if (class6ReleaseTimer == 0)
            {
                Vector2 spawnPos = Player.Center + new Vector2(0, Player.height / 2);

                for (int i = 0; i < 25; i++)
                {
                    // Horizontal speed, Vertical
                    float speed = Main.rand.NextFloat(0f, 5f);

                    // might be too fast currently
                    float direction = Main.rand.NextBool() ? 0f : 180f;
                    Vector2 velocity = direction.ToRotationVector2() * speed;

                    //horizontal offset
                    float xOffset = Main.rand.NextFloat(-32f, 32f); // 1 tile = 16px 
                    float yOffset = Main.rand.NextFloat(-32f, 10f);

                    Vector2 spawnPoss = Player.Center + new Vector2(xOffset, Player.height / 2f + yOffset);

                    Projectile.NewProjectile(
                        Player.GetSource_Misc("Class6GroundFlames"),
                        spawnPoss,
                        velocity,
                        480, // cursed flame
                        26,
                        1f,
                        Player.whoAmI
                    );
                }
            }
        }


        private void BlackMageOnUse()
        {
            if (Player.GetModPlayer<Abilities>().class7HitCounter >= 10)
            {
                Player.AddBuff(176, 15);
                Player.AddBuff(206, 420);
                Player.AddBuff(137, 420);
                Player.AddBuff(320, 420);

                Player.GetModPlayer<Abilities>().class7HitCounter = 0;

                Main.NewText("Ability activated!");
            }
            else
            {
                Main.NewText($"{Player.GetModPlayer<Abilities>().class7HitCounter}/10 hits");
            }
        }


        private void PsychicOnUse()
        {
            Player.AddBuff(196, 54000);
            Player.AddBuff(178, 54000);
            Player.AddBuff(181, 54000);
            
            class8HP = Player.statLife;
        }


        private void PsychicPostStatus()
        {
            if (class8HP != 0)
            {
                if (Player.HeldItem.type == ItemID.NebulaArcanum && Player.controlUseItem && Player.itemAnimation == 30) class8HP = (class8HP <= 20) ? 0 : class8HP - 20;

                if (Player.statLife < class8HP) class8HP = Player.statLife;
                Player.statLife = class8HP;
            }

            if (Player.statLife <= 0) Player.KillMe(PlayerDeathReason.ByCustomReason(""), 1, 0);
        }


        private void WhiteMageOnUse()
        {
            foreach (Player other in Main.player)
            {
                if (!other.active || other.dead || other.whoAmI == Player.whoAmI)
                    continue;

                if (Vector2.Distance(Player.Center, other.Center) <= 25 * 16 && Player.team == other.team) // 25 block radius
                {
                    other.AddBuff(103, 480);
                    other.AddBuff(26, 480);
                    other.AddBuff(2, 480);
                    
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 2, 480);
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 26, 480);
                    NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 103, 480);
                }
            }
        }


        private void MinerOnUse() //not finished
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Point playerTile = Player.Center.ToTileCoordinates();

                for (int offsetX = -13; offsetX <= 13; offsetX++)
                {
                    for (int offsetY = -4; offsetY <= 1; offsetY++)
                    {
                        int x = playerTile.X + offsetX;
                        int y = playerTile.Y + offsetY;

                        if (WorldGen.InWorld(x, y))
                        {
                            Tile tile = Main.tile[x, y];
                            if (tile.HasTile && tile.TileType == TileID.Dirt)
                            {
                                WorldGen.KillTile(x, y, false, false, true);
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y);
                            }
                        }
                    }
                }
            }
        }


        private void FishOnUse()
        {
            Player.AddBuff(1, 180);
            Player.AddBuff(104, 180);
            Player.AddBuff(109, 180);
        }


        private void ClownOnUse() //not finished
        {
            Player.AddBuff(320, 60);
            
            class12SwapTimer = 60;

            Main.NewText("Swapping...");
        }


        private void ClownPostStatus()
        {
            if (class12SwapTimer != -1) class12SwapTimer--;

            if (class12SwapTimer == 0)
            {
                foreach (Player other in Main.player)
                {  
                    if (!other.active || other.dead || other.whoAmI == Player.whoAmI)
                        continue;

                    if (Vector2.Distance(Player.Center, other.Center) <= 22 * 16 && Vector2.Distance(Player.Center, other.Center) < class12ClosestDist && Player.team != other.team) // 22 block radius
                    {
                        class12ClosestDist = (int) Vector2.Distance(Player.Center, other.Center);
                        class12ClosestPlayer = other;
                    }
                }

                if (class12ClosestPlayer != null)
                {
                    Vector2 tempPosition = Player.position;
                    Vector2 tempPosition2 = class12ClosestPlayer.position;

                    var mod = ModContent.GetInstance<CTG2>();
                    
                    //class12ClosestPlayer.Teleport(tempPosition);
                    ModPacket packet1 = mod.GetPacket();
                    packet1.Write((byte)MessageType.RequestTeleport);
                    packet1.Write(Player.whoAmI);
                    packet1.Write((int)tempPosition2.X);
                    packet1.Write((int)tempPosition2.Y);
                    packet1.Send();

                    ModPacket packet2 = mod.GetPacket();
                    packet2.Write((byte)MessageType.RequestTeleport);
                    packet2.Write(class12ClosestPlayer.whoAmI);
                    packet2.Write((int)tempPosition.X);
                    packet2.Write((int)tempPosition.Y);
                    packet2.Send();
                    
                    Player.AddBuff(BuffID.ChaosState, 26 * 60);

                    Main.NewText("Successfully swapped!");
                }
                else
                {
                    Main.NewText("Swap was unsuccessful!");
                }
                
                class12ClosestDist = 99999;
                class12ClosestPlayer = null;
            }
        }


        private void FlameBunnyOnUse()
        {
            Player.AddBuff(137, 6 * 60);
            Player.AddBuff(320, 6 * 60);
        }


        private void TikiPriestOnUse()
        {   
            var mod1 = ModContent.GetInstance<CTG2>();
            ModPacket packet1 = mod1.GetPacket();
            packet1.Write((byte)MessageType.RequestSpawnNpc);
            packet1.Write((int)Player.Center.X);
            packet1.Write((int)Player.Center.Y);
            packet1.Write(ModContent.NPCType<TikiTotem>());
            packet1.Write(Player.team);
            packet1.Write(0f);
            packet1.Send();

            /*var mod2 = ModContent.GetInstance<CTG2>();
            ModPacket packet2 = mod2.GetPacket();
            packet2.Write((byte)MessageType.SetNpcTeam);
                Main.NewText($"{CTG2.requestedNpcIndex}, {Player.team}");
            packet2.Write(CTG2.requestedNpcIndex);
            packet2.Write(Player.team);
            packet2.Send();
            Main.npc[CTG2.requestedNpcIndex].GetGlobalNPC<AllNpcs>().team = Player.team;*/
        }


        private void TreeOnUse() //not done
        {
            for (int i = 0; i < 7; i++)
            {
                Projectile.NewProjectile(
                    Player.GetSource_Misc("Class15Ability"),
                    Player.Center,
                    Vector2.Zero,
                    511, 
                    0,
                    0f,
                    Player.whoAmI
                );
            }
        }


        private void TreePostStatus()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj.active && proj.type == 511 && proj.owner == Player.whoAmI)
                {
                    foreach (Player other in Main.player)
                    {
                        if (Player.whoAmI == other.whoAmI || Player.team == other.team) continue;
                        if (proj.Hitbox.Intersects(other.Hitbox))
                        {
                            other.AddBuff(160, 30);
                            other.AddBuff(197, 30);
                                    
                            NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 160, 30);
                            NetMessage.SendData(MessageID.AddPlayerBuff, other.whoAmI, -1, null, other.whoAmI, 197, 30);
                        }
                    }
                }
            }
        }

        
        private void MutantInitialize()
        {
            using (var stream = Mod.GetFileStream($"Content/Classes/rushmutant.json"))
            using (var fileReader = new StreamReader(stream))
            {
                var jsonData = fileReader.ReadToEnd();
                try
                {
                    class16RushData = JsonSerializer.Deserialize<CtgClass>(jsonData);
                }
                catch
                {
                    Main.NewText("Failed to load or parse inventory file.", Microsoft.Xna.Framework.Color.Red);
                    return;
                }
            }
            using (var stream = Mod.GetFileStream($"Content/Classes/regenmutant.json"))
            using (var fileReader = new StreamReader(stream))
            {
                var jsonData = fileReader.ReadToEnd();
                try
                {
                    class16RegenData = JsonSerializer.Deserialize<CtgClass>(jsonData);
                }
                catch
                {
                    Main.NewText("Failed to load or parse inventory file.", Microsoft.Xna.Framework.Color.Red);
                    return;
                }
            }
        }


        private void MutantOnUse()
        {
            Player.AddBuff(149, 90);

            switch (mutantState)
            {
                case 1:
                    SetInventory(class16RegenData);
                    mutantState = 2;

                    break;

                case 2:
                    SetInventory(class16RushData);
                    mutantState = 1;

                    break;
            }
        }


        private void LeechOnUse() //not finished
        {
            Player.AddBuff(320, 5 * 60);
        }


        public override void PostItemCheck() // Upon activation
        {
            if (Main.netMode == NetmodeID.Server) return;
            
            if (!initializedMutant)
            {
                MutantInitialize();
                initializedMutant = true;
            }

            if (Player.HeldItem.type == ItemID.WhoopieCushion && Player.controlUseItem && Player.itemTime == 0 && !Player.HasBuff(BuffID.ChaosState)) // Only activate if not on cooldown
            {
                int selectedClass = PlayerManager.currentClass.AbilityID;

                switch (selectedClass)
                {
                    case 1:
                        SetCooldown(36);
                        ArcherOnUse();

                        break;

                    case 2:
                        SetCooldown(10);
                        NinjaOnUse();
                        
                        break;

                    case 3:
                        SetCooldown(35);
                        BeastOnUse();

                        break;

                    case 4:
                        SetCooldown(35);
                        GladiatorOnUse();

                        break;

                    case 5:
                        SetCooldown(10);
                        PaladinOnUse();

                        break;

                    case 6:
                        SetCooldown(31);
                        JungleManOnUse();

                        break;

                    case 7:
                        SetCooldown(1);
                        BlackMageOnUse();

                        break;

                    case 8:
                        SetCooldown(40);
                        PsychicOnUse();

                        break;

                    case 9:
                        SetCooldown(30);
                        WhiteMageOnUse();

                        break;

                    case 10: //not finished
                        SetCooldown(15);
                        MinerOnUse();

                        break;

                    case 11:
                        SetCooldown(35);
                        FishOnUse();

                        break;

                    case 12: //not finished
                        SetCooldown(14);
                        ClownOnUse();

                        break;

                    case 13: //not finished
                        SetCooldown(41);
                        FlameBunnyOnUse();

                        break;

                    case 14: //not finished
                        SetCooldown(1); //20
                        TikiPriestOnUse();

                        break;

                    case 15: //not finished 
                        SetCooldown(27);
                        TreeOnUse();

                        break;

                    case 16:
                        SetCooldown(1);
                        MutantOnUse();

                        break;

                    case 17: //not finished
                        SetCooldown(40);
                        LeechOnUse();

                        break;
                }
            }
        }

        //All timer logic below
        public override void PostUpdate()
        {
            GladiatorPostStatus();
            JungleManPostStatus();
            PsychicPostStatus();
            ClownPostStatus();
            TreePostStatus();
        }
    }
}
