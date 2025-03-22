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
                    case 1: // Archer
                        Player.AddBuff(BuffID.ChaosState, 2400); // 40 seconds
                        Player.AddBuff(BuffID.Swiftness, 600);    // 10 seconds
                        break;

                    case 2: // Warrior
                        Player.AddBuff(BuffID.ChaosState, 1800); // 30 seconds
                        Player.AddBuff(BuffID.Ironskin, 600);    // 10 seconds
                        break;

                    case 3: // Mage
                        Player.AddBuff(BuffID.ChaosState, 3600); // 60 seconds
                        Player.AddBuff(BuffID.MagicPower, 600);  // 10 seconds
                        break;
                }
            }
        }
    }
}
