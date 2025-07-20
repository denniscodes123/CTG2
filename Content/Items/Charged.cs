using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Dawn.Items.Weapons.Ranger.Bows.Mechanics
{
    public class Charged : GlobalItem
    {
        public bool Affected;


        public override void SetDefaults(Item item)
        {
            base.SetDefaults(item);

            Affected = (item.type == 5551); // rancor item id

            if (item.useAmmo == AmmoID.Arrow && Affected)
            {
                item.channel = true;
                item.autoReuse = true;
                item.UseSound = null;
                item.noUseGraphic = true;
            }
        }


        public override bool CanBeConsumedAsAmmo(Item ammo, Item weapon, Player player)
        {
            if (weapon.useAmmo == AmmoID.Arrow && Affected) return !(player.ownedProjectileCounts[ModContent.ProjectileType<ChargedBowProjectile>()] >= 1);
            else return base.CanBeConsumedAsAmmo(ammo, weapon, player);
        }


        public override bool CanUseItem(Item item, Player player)
        {
            if (item.useAmmo == AmmoID.Arrow && Affected) return !(player.ownedProjectileCounts[ModContent.ProjectileType<ChargedBowProjectile>()] >= 1);
            else return base.CanUseItem(item, player);
        }


        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.useAmmo == AmmoID.Arrow && Affected)
            {
                if (player.ownedProjectileCounts[ModContent.ProjectileType<ChargedBowProjectile>()] == 0)
                {
                    Projectile.NewProjectile(Entity.GetSource_NaturalSpawn(), position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ChargedBowProjectile>(),
                        0, knockback, player.whoAmI, item.type, type);
                }
                return false;
            }
            else return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }


        public override bool InstancePerEntity => true;


        public override bool AppliesToEntity(Item entity, bool lateInstantiation) { return true; }
    }
}
