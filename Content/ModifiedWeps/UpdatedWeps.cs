using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content.Items.ModifiedWeps
{
    public class OverloadedWeps : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.TendonBow:
                    item.useTime = 40;
                    item.useAnimation = 30;
                    item.damage = 40;
                    item.knockBack = 3.5f;
                    item.autoReuse = true;
                    item.mana = 5;
                    item.DamageType = DamageClass.Magic;
                    break;

                case ItemID.EnchantedSword:
                    item.damage = 100;
                    item.knockBack = 8f;
                    item.autoReuse = true;
                    break;

                case ItemID.Starfury:
                    item.useTime = 12;
                    item.useAnimation = 12;
                    item.damage = 60;
                    item.shootSpeed = 10f;
                    break;
            }
        }
    }
}
