using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content.Items
{
	public class StaffOfWinter : ModItem // White Mage staff
	{
		public override void SetStaticDefaults()
		{
			Item.staff[Type] = true;
		}
		public override void SetDefaults()
		{
	  		Item.CloneDefaults(ItemID.SpectreStaff);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item28;
		}
	}
}