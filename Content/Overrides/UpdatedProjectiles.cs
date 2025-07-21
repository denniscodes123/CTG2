using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

public class ToxicFlaskTimeOverride : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    public override void AI(Projectile projectile)
    {
        if (projectile.type == 511)
        {
            // Force a new timeLeft value (e.g., 120 ticks = 2 seconds)
            if (projectile.timeLeft > 120)
            {
                projectile.timeLeft = 120;
            }
        }
        if (projectile.type == ProjectileID.ThornChakram) //this code makes clown one blocking easier
        {
            projectile.width = 30;
            projectile.height = 30;

        }
        if (projectile.type == 700) //kill ghast projectiles
        {
            projectile.Kill();
        }
        if (projectile.type == ProjectileID.NebulaArcanumExplosionShot) //Might have to ovveride the subshot shards as well
        {
            projectile.damage = 0; //make explosion 0 damage
            projectile.scale = 0;
        }
        if (projectile.type == ProjectileID.NebulaArcanumSubshot)
        {
            projectile.damage = 0;
            projectile.scale = 0;
        }
        if (projectile.type == ProjectileID.NebulaArcanumExplosionShotShard)
        {
            projectile.damage = 0;
            projectile.scale = 0;
        } //If nebula code doesnt work we have to kill the projectiles onspawn

    }

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {

        if (projectile.type == 700) // Ghast
        {
            projectile.scale = 0f;
            projectile.damage = 0;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            projectile.timeLeft = 1; // dies almost instantly
        }
        if (projectile.type == ProjectileID.NebulaArcanumExplosionShot)
        {
            projectile.damage = 0; //second check in case first fails for nebula epxlosion
            projectile.knockBack = 0f;
            projectile.scale = 0.1f;
        }
    }   



}
public class ModifyHurtModPlayer : ModPlayer
{
    //should make weps not inflict debuffs?
    public override void OnHurt(Player.HurtInfo info)
    {
        if (info.DamageSource.SourceProjectileType == ProjectileID.Sunfury)
        {
            Player.ClearBuff(BuffID.OnFire);
        }
        else if (info.DamageSource.SourceProjectileType == ProjectileID.ThornChakram)
        {
            Player.ClearBuff(BuffID.Poisoned);
        }
        else if (info.DamageSource.SourceProjectileType == ProjectileID.CursedFlameFriendly)
        {
            Player.ClearBuff(BuffID.CursedInferno);
        }
        else if (info.DamageSource.SourceProjectileType == ProjectileID.CursedFlameHostile)
        {
            Player.ClearBuff(BuffID.CursedInferno);
        }
        else if (info.DamageSource.SourceProjectileType == 19) //flamebunny flamrang
        {
            Player.ClearBuff(24);
        }
        else if (info.DamageSource.SourceProjectileType == 15) //flamebunny fof
        {
            Player.ClearBuff(24);
        }
        else if (info.DamageSource.SourceProjectileType == 480) //Hardcodedjman proj as fallback
        {
            Player.ClearBuff(BuffID.CursedInferno);
        }
    }
}

