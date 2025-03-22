using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content.Items.ModifiedWeps 
{
    public class Tabi : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Tabi) 
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
}
