using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CTG2.Content.Items {
	
	public class Zen : ModItem { // Magic Dagger
	    
		public override void SetDefaults() {
	  		Item.CloneDefaults(ItemID.MagicDagger);

			Item.shoot = ModContent.ProjectileType<ZenProjectile>();
		}

	}

	public class ZenProjectile : ModProjectile {

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.MagicDagger);
		}

		public override string Texture => "CTG2/Content/Items/ZenProjectile"; // Set to the path of your custom texture
  	}
}
