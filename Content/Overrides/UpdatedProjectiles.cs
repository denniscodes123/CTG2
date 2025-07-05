using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

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
    }
}
