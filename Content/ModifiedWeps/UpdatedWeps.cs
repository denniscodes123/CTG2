using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content.Items.ModifiedWeps
{
    public class OverloadedWeps : GlobalItem
    {

        private uint bananarangDelay = 55;
        private uint bananarangLastUsedCounter = 0;

        public override bool InstancePerEntity => true;


        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.WhoopieCushion:
                    item.useTime = 1;
                    item.useAnimation = 1;
                    break;

                case ItemID.Bananarang:
                    item.useTime = 20;
                    item.useAnimation = 20;
                    item.shootSpeed = 11.4f;
                    item.knockBack = 6f;
                    item.autoReuse = false;
                    item.damage = 35;
                    break;
            }
        }


        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.Bananarang)
            {
                if (Main.GameUpdateCount - bananarangLastUsedCounter >= bananarangDelay)
                {
                    bananarangLastUsedCounter = Main.GameUpdateCount;
        
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
	 	}
    }
}
