using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace CTG2.Content
{
    public class Abilities : ModPlayer
    {
        private int class4BuffTimer = 0;
        private bool class4PendingBuffs = false;

        private int class6FlameTimer = 0;
        private int class6FlameDuration = 0;


        private void SetCooldown(int seconds)
        {
            Player.AddBuff(BuffID.ChaosState, seconds * 60);
        }


        private void ArcherOnUse() //not finished
        {

        }


        private void NinjaOnUse()
        {
            Player.AddBuff(BuffID.Invisibility, 60 * 60);
        }


        private void BeastOnUse()
        {
            Player.AddBuff(BuffID.MagicPower, 600);

            if (Main.myPlayer == Player.whoAmI && Main.netMode != NetmodeID.MultiplayerClient) // server-side or singleplayer
            {
                int npcIndex = NPC.NewNPC(Player.GetSource_Misc("Class3Ability"), (int)Player.Center.X, (int)Player.Center.Y, ModContent.NPCType<StationaryBeast>());
                if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
            }
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


        private void PaladinOnUse() //not finished
        {

        }


        private void JungleManOnUse()
        {
            Player.AddBuff(149, 42);
            Player.AddBuff(114, 42);
                        
            class6FlameDuration = 1;
        }


        private void JungleManPostStatus()
        {
            if (class6FlameDuration > 0)
            {
                class6FlameDuration--;

                if (Main.myPlayer == Player.whoAmI && Main.netMode != NetmodeID.MultiplayerClient)
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

                        if (Main.myPlayer == Player.whoAmI && Main.netMode != NetmodeID.MultiplayerClient)
                        {
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
            }
        }


        private void BlackMageOnUse()
        {
            Player.AddBuff(176, 15);
            Player.AddBuff(26, 420);
            Player.AddBuff(137, 420);
            Player.AddBuff(320, 420);
        }


        private void PsychicOnUse() //not finished
        {

        }


        private void WhiteMageOnUse() //not finished
        {

        }


        private void MinerOnUse() //not finished
        {

        }


        private void FishOnUse()
        {
            Player.AddBuff(1, 180);
            Player.AddBuff(104, 180);
            Player.AddBuff(109, 180);
        }


        private void ClownOnUse() //not finished
        {

        }


        private void FlameBunnyOnUse() //not finished
        {

        }


        private void TikiPriestOnUse() //not finished
        {

        }


        private void TreeOnUse() //not finished
        {

        }


        private void MutantOnUse() //not finished
        {

        }


        private void LeechOnUse() //not finished
        {

        }


        public override void PostItemCheck() // Upon activation
        {
            if (Player.HeldItem.type == ItemID.WhoopieCushion &&
                Player.controlUseItem &&
                Player.itemTime == 0 &&
                !Player.HasBuff(BuffID.ChaosState)) // Only activate if not on cooldown
            {
                
                int selectedClass = Player.GetModPlayer<ClassSystem>().playerClass;

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

                    case 4: //finished
                        SetCooldown(35);
                        GladiatorOnUse();

                        break;

                    case 5: //not finished
                        SetCooldown(10);
                        PaladinOnUse();

                        break;

                    case 6:
                        SetCooldown(31);
                        JungleManOnUse();

                        break;

                    case 7:
                        SetCooldown(42);
                        BlackMageOnUse();

                        break;

                    case 8: //not finished
                        SetCooldown(40);
                        PsychicOnUse();

                        break;

                    case 9: //not finished
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
                        SetCooldown(11);
                        ClownOnUse();

                        break;

                    case 13: //not finished
                        SetCooldown(41);
                        FlameBunnyOnUse();

                        break;

                    case 14: //not finished
                        SetCooldown(20);
                        TikiPriestOnUse();

                        break;

                    case 15: //not finished 
                        SetCooldown(27);
                        TreeOnUse();

                        break;

                    case 16: //not finished
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
        }
    }
}
