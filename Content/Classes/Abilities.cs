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


        public override void PostItemCheck()
        {
            if (Player.HeldItem.type == ItemID.WhoopieCushion &&
                Player.controlUseItem &&
                Player.itemTime == 0 &&
                !Player.HasBuff(BuffID.ChaosState)) // Only activate if not on cooldown
            {
                int selectedClass = Player.GetModPlayer<ClassSystem>().playerClass;

                switch (selectedClass)
                {
                    case 1: // not finished 
                        Player.AddBuff(BuffID.ChaosState, 36 * 60);
                        Player.AddBuff(BuffID.Swiftness, 600);

                        break;

                    case 2:
                        Player.AddBuff(BuffID.Invisibility, 3600);
                        Player.AddBuff(BuffID.ChaosState, 600);
                        break;

                    case 3:
                        Player.AddBuff(BuffID.ChaosState, 35 * 60);
                        Player.AddBuff(BuffID.MagicPower, 600);

                        if (Main.myPlayer == Player.whoAmI && Main.netMode != NetmodeID.MultiplayerClient) // server-side or singleplayer
                        {
                            int npcIndex = NPC.NewNPC(Player.GetSource_Misc("Class3Ability"), (int)Player.Center.X, (int)Player.Center.Y, ModContent.NPCType<StationaryBeast>());
                            if (Main.netMode == NetmodeID.Server)
                                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npcIndex);
                        }
                        break;


                    case 4: //finished
                        Player.AddBuff(206, 300);
                        Player.AddBuff(195, 300);
                        Player.AddBuff(75, 300);
                        Player.AddBuff(320, 300);
                        Player.AddBuff(BuffID.ChaosState, 35 * 60);

                        class4BuffTimer = 300;
                        class4PendingBuffs = true;

                        //5 second interval here (postupdateworld runs the interval)
                        break;

                    case 5: //not finished
                        Player.AddBuff(BuffID.ChaosState, 10 * 60);

                        break;

                    case 6: //not finished
                        Player.AddBuff(BuffID.ChaosState, 31 * 60);
                        Player.AddBuff(149, 42);
                        Player.AddBuff(114, 42);
                        
                        class6FlameDuration = 1;
                        break;

                    case 7:
                        Player.AddBuff(BuffID.ChaosState, 42 * 60);
                        Player.AddBuff(176, 15);
                        Player.AddBuff(26, 420);
                        Player.AddBuff(137, 420);
                        Player.AddBuff(320, 420);
                        break;

                    case 8: //not finished
                        Player.AddBuff(BuffID.ChaosState, 40 * 60);
                        break;

                    case 9: //not finished
                        Player.AddBuff(BuffID.ChaosState, 30 * 60);
                        break;

                    case 10: //not finished
                        Player.AddBuff(BuffID.ChaosState, 15 * 60);
                        break;

                    case 11:
                        Player.AddBuff(BuffID.ChaosState, 35 * 60);
                        Player.AddBuff(1, 180);
                        Player.AddBuff(104, 180);
                        Player.AddBuff(109, 180);
                        break;

                    case 12: //not finished
                        Player.AddBuff(BuffID.ChaosState, 11 * 60);
                        break;

                    case 13: //not finished
                        Player.AddBuff(BuffID.ChaosState, 41 * 60);
                        break;

                    case 14: //not finished
                        Player.AddBuff(BuffID.ChaosState, 20 * 60);
                        break;

                    case 15: //not finished 
                        Player.AddBuff(BuffID.ChaosState, 27 * 60);
                        break;

                    case 16: //not finished
                        Player.AddBuff(BuffID.ChaosState, 1 * 60);
                        break;

                    case 17: //not finished
                        Player.AddBuff(BuffID.ChaosState, 40 * 60);
                        break;

                }
            }
        }

        //All timer logic below
        public override void PostUpdate()
        {

            if (class4PendingBuffs) //runs 5 second interval for glad ability 
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




            if (class6FlameDuration > 0) //jman logic
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
        }


    }
    

