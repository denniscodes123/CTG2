using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content.Items.ModifiedWeps // Update namespace to match new folder structure
{
    public class TendonBowEdit : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.TendonBow) // Modify Tendon Bow
            {
                item.useTime = 40;
                item.useAnimation = 30;
                item.damage = 40;
                item.knockBack = 3.5f;
                item.autoReuse = true;
                               
                item.mana = 5; // Costs 5 mana per shot

                // ✅ Change the damage type to magic
                item.DamageType = DamageClass.Magic;



            }
        }
    }
}
