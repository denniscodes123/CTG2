using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System;
using Terraria.GameInput;

namespace CTG2.Content.ClientSide;

public class TestPlayer : ModPlayer
{
    // if you found thsi file do not look down pls 
    // guess the password to use it tho 
  
    public bool playerAttribute = false;


    public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (playerAttribute && (item.DamageType == DamageClass.Ranged || item.DamageType == DamageClass.Magic))
        {
            if (TryFindTarget(position, 2500f, out Vector2 targetPos, out Vector2 targetVel))
            {
                float projectileSpeed = velocity.Length();
                Vector2 newVelocity = PredictAim(position, targetPos, targetVel, projectileSpeed);

                if (newVelocity != Vector2.Zero)
                {
                    Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, Player.whoAmI);
                    
                    Player.ChangeDir(newVelocity.X > 0 ? 1 : -1);
                    Player.itemRotation = (float)Math.Atan2(newVelocity.Y * Player.direction, newVelocity.X * Player.direction);

                    return false;
                }
            }
        }
        

        return true;
    }

    private bool TryFindTarget(Vector2 origin, float maxRange, out Vector2 targetPosition, out Vector2 targetVelocity)
    {
        targetPosition = Vector2.Zero;
        targetVelocity = Vector2.Zero;
        bool targetFound = false;
        float closestDistSq = maxRange * maxRange;

        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player potentialTarget = Main.player[i];

            if (potentialTarget.active && !potentialTarget.dead && i != Main.myPlayer && Player.InOpposingTeam(potentialTarget))
            {
                Vector2 targetCenter = potentialTarget.MountedCenter;
                float distSq = Vector2.DistanceSquared(origin, targetCenter);

            
                if (distSq < closestDistSq && Collision.CanHitLine(origin, 1, 1, targetCenter, 1, 1))
                {
                    closestDistSq = distSq;
                    targetPosition = targetCenter;
                    targetVelocity = potentialTarget.velocity;
                    targetFound = true;
                }
            }
        }
        return targetFound;
    }

    private Vector2 PredictAim(Vector2 shooterPos, Vector2 targetPos, Vector2 targetVel, float projSpeed)
    {
        Vector2 deltaPos = targetPos - shooterPos;

        float a = Vector2.Dot(targetVel, targetVel) - projSpeed * projSpeed;
        float b = 2 * Vector2.Dot(deltaPos, targetVel);
        float c = Vector2.Dot(deltaPos, deltaPos);

        float discriminant = b * b - 4 * a * c;

        if (discriminant >= 0)
        {
            float sqrtDiscriminant = (float)Math.Sqrt(discriminant);
            float t1 = (-b - sqrtDiscriminant) / (2 * a);
            float t2 = (-b + sqrtDiscriminant) / (2 * a);
            
            float time = (t1 > 0 && t2 > 0) ? Math.Min(t1, t2) : Math.Max(t1, t2);

            if (time > 0)
            {
                Vector2 predictedPosition = targetPos + targetVel * time;
                Vector2 fireDirection = predictedPosition - shooterPos;
                return Vector2.Normalize(fireDirection) * projSpeed;
            }
        }

        return Vector2.Normalize(deltaPos) * projSpeed;
    }
}