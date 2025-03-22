using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;


namespace CTG2.Content {
    
    public class MyModPlayer : ModPlayer
    {

        private void SpawnDust(Vector2 position)
            {
                // Spawn a dust at the specified position
                for (int i = 0; i < 5; i++)  // 5 dust particles
                {
                    Dust dust = Dust.NewDustDirect(position, 0, 0, DustID.ChlorophyteWeapon, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                    dust.noGravity = true;  // Makes the dust have no gravity (it won't fall)
                    dust.scale = 1.5f;  // Scale of the dust
                }
            }
        
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
        
            // Iterate through all projectiles
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile p = Main.projectile[i];
    
                // Check if the projectile is active and belongs to your mod (change the ID or type accordingly)
                if (p.active && p.type == ModContent.ProjectileType<Items.FoliageTendrilsProjectile>())
                {
                    p.Kill();
                    SpawnDust(p.Center);
                }
            }
    
            base.OnHurt(hurtInfo);
        }
    }
}
