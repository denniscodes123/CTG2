using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using CTG2.Content.Items;
using Terraria.Audio;

public class ProjectileOverrides : GlobalProjectile
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
        if (projectile.type == ProjectileID.ThornChakram)
        {
            projectile.width = 30; //easier one blocking
            projectile.height = 30;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile barrier = Main.projectile[i];

                if (barrier.type == ModContent.ProjectileType<HexagonalBarrierProjectile>())
                {
                    HexagonalBarrierProjectile barrierProj = barrier.ModProjectile as HexagonalBarrierProjectile;
                    if (projectile.Hitbox.Intersects(barrier.Hitbox) && barrierProj.alive && barrierProj.teamCheck)
                    {
                        barrierProj.alive = false;
                        if (projectile.ai[1] > 40)
                        {
                            projectile.ai[0] = 1;
                            projectile.netUpdate = true;
                        }
                        else
                        {
                            projectile.velocity.X *= -1f;
                            projectile.velocity.Y *= -1f;
                        }

                        // Optional: play a sound or spawn dust for feedback
                        SoundEngine.PlaySound(SoundID.Dig, projectile.Center);
                        break;
                    }
                }
            }
        }
        if (projectile.type == ProjectileID.Flamarang || projectile.type == ProjectileID.Bananarang) 
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile barrier = Main.projectile[i];

                if (barrier.type == ModContent.ProjectileType<HexagonalBarrierProjectile>())
                {
                    HexagonalBarrierProjectile barrierProj = barrier.ModProjectile as HexagonalBarrierProjectile;
                    if (projectile.Hitbox.Intersects(barrier.Hitbox) && barrierProj.alive && barrierProj.teamCheck)
                    {
                        barrierProj.alive = false;
                        projectile.ai[0] = 1;
                        projectile.netUpdate = true;
                        projectile.velocity.X *= -0.8f;
                        projectile.velocity.Y *= -0.8f;

                        // Optional: play a sound or spawn dust for feedback
                        SoundEngine.PlaySound(SoundID.Dig, projectile.Center);
                        break;
                    }
                }
            }
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
