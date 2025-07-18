using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content.Items
{
	public class Rancor : ModItem // Archer bow
	{

	   	private uint useDelay = 36; // Time between shots
     	private uint lastUsedCounter = 0;
	    
		public override void SetDefaults()
		{
	  		Item.CloneDefaults(ItemID.TendonBow);
	     
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 18;
			Item.height = 40;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 5;
			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Red;
			Item.autoReuse = false;
   			Item.mana = 0;
		}
		
  		public override bool CanUseItem(Player player)
	    	{
		      	if (Main.GameUpdateCount - lastUsedCounter >= useDelay)
		      	{
		        	lastUsedCounter = Main.GameUpdateCount;
		
		        	return true;
		      	}
		      	else
		      	{
		        	return false;
		      	}
	 	}
	}
}
