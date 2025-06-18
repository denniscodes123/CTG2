using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CTG2.Content.Items;
using CTG2.Content.Items.ModifiedWeps;
using Microsoft.Xna.Framework; 


namespace CTG2.Content
{
    public class DebuffGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            Main.NewText("hitplayerxd", Color.Red);
            if (projectile.type == 511)
            {
                Main.NewText("hitplayerlol", Color.Red);
                var mod1 = ModContent.GetInstance<CTG2>();
                ModPacket packet1 = mod1.GetPacket();
                packet1.Write((byte)MessageType.RequestAddBuff);
                packet1.Write((byte)target.whoAmI);
                packet1.Write((int)197);
                packet1.Write((int)30);
                packet1.Send();

                var mod2 = ModContent.GetInstance<CTG2>();
                ModPacket packet2 = mod2.GetPacket();
                packet2.Write((byte)MessageType.RequestAddBuff);
                packet2.Write((byte)target.whoAmI);
                packet2.Write((int)160);
                packet2.Write((int)30);
                packet2.Send();
                
                // target.AddBuff(197, 30);
                // target.AddBuff(160, 30);
            }
        }
    }
}
