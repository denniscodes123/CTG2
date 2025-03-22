using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content.Items.ModifiedWeps
{
    public class TendonBowEdit : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.TendonBow)
            {
                item.useTime = 40;
                item.useAnimation = 30;
                item.damage = 40;
                item.knockBack = 3.5f;
                item.autoReuse = true;
                item.mana = 5;
                item.DamageType = DamageClass.Magic;
            }
        }
    }

    public class EnchantedSwordEdit : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.EnchantedSword)
            {
                item.damage = 100;
                item.knockBack = 8f;
                item.autoReuse = true;
            }
        }
    }

    public class StarfuryEdit : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Starfury)
            {
                item.useTime = 12;
                item.useAnimation = 12;
                item.damage = 60;
                item.shootSpeed = 10f;
            }
        }
    }
}
