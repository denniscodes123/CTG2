using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content
{
    public class Abilities : ModPlayer
    {
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

                    case 3: //not finished
                        Player.AddBuff(BuffID.ChaosState, 35*60); 
                        Player.AddBuff(BuffID.MagicPower, 600);
                        break;

                    case 4: //incomplete
                        Player.AddBuff(206, 300);
                        Player.AddBuff(195, 300);
                        Player.AddBuff(75, 300);
                        Player.AddBuff(320, 300);
                        Player.AddBuff(BuffID.ChaosState, 35*60);

                        //add 5 second interval here

                        Player.AddBuff(137, 180);
                        Player.AddBuff(32, 180);
                        Player.AddBuff(195, 180);
                        Player.AddBuff(5, 180);
                        Player.AddBuff(215, 180);
                        break;
                        
                    case 5: //not finished
                        Player.AddBuff(BuffID.ChaosState, 10*60);

                        break;

                    case 6: //not finished
                        Player.AddBuff(BuffID.ChaosState, 31*60);
                        break;

                    case 7: 
                        Player.AddBuff(BuffID.ChaosState, 42*60);
                        Player.AddBuff(176, 15);
                        Player.AddBuff(26, 420);
                        Player.AddBuff(137, 420);
                        Player.AddBuff(320, 420);
                        break;

                    case 8: //not finished
                        Player.AddBuff(BuffID.ChaosState, 40*60);
                        break;

                    case 9: //not finished
                        Player.AddBuff(BuffID.ChaosState, 30*60);
                        break;

                    case 10: //not finished
                        Player.AddBuff(BuffID.ChaosState, 15*60);
                        break;

                    case 11: 
                        Player.AddBuff(BuffID.ChaosState, 35*60);
                        Player.AddBuff(1, 180);
                        Player.AddBuff(104, 180);
                        Player.AddBuff(109, 180);
                        break;

                    case 12: //not finished
                        Player.AddBuff(BuffID.ChaosState, 11*60); 
                        break;

                    case 13: //not finished
                        Player.AddBuff(BuffID.ChaosState, 41*60);
                        break;

                    case 14: //not finished
                        Player.AddBuff(BuffID.ChaosState, 20*60);
                        break;

                    case 15: //not finished 
                        Player.AddBuff(BuffID.ChaosState, 27*60);
                        break;

                    case 16: //not finished
                        Player.AddBuff(BuffID.ChaosState, 1*60);
                        break;

                    case 17: //not finished
                        Player.AddBuff(BuffID.ChaosState, 40*60);
                        break;

                }
            }
        }
    }
}
