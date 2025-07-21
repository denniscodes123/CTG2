using CTG2.Content.ServerSide;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content
{
    public class UpdatedItems : GlobalItem
    {
        GameManager gameManager = ModContent.GetInstance<GameManager>();
        public override bool CanPickup(Item item, Player player)
        {
            // Prevent picking up a second gem
            if ((item.type == ItemID.LargeSapphire && PlayerHasGem(player, ItemID.LargeSapphire)) ||
                (item.type == ItemID.LargeRuby && PlayerHasGem(player, ItemID.LargeRuby)))
                return false;
            return base.CanPickup(item, player);
        }

        public override bool CanRightClick(Item item)
        {
            // Prevent right-clicking to trash
            if (item.type == ItemID.LargeSapphire || item.type == ItemID.LargeRuby)
                return false;
            return base.CanRightClick(item);
        }

        private bool PlayerHasGem(Player player, int gemType)
        {
            for (int i = 0; i < player.inventory.Length; i++)
                if (player.inventory[i].type == gemType)
                    return true;
            return false;
        }
    }
}