using Terraria.ModLoader.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using CTG2.Content.Items;
using System.IO;
using CTG2.Content.Configs;



namespace CTG2.Content
{
    public class ColorCodedProjectiles : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {

            if (projectile.type == 496) { } //bmage prob already stores data in localai[0] so we have to do this
            else
            {
                projectile.localAI[0] = Main.player[projectile.owner].team;
                projectile.netUpdate = true;
            }
            
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)		
        {
            binaryWriter.Write(projectile.localAI[0]);
        }

        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            projectile.localAI[0] = binaryReader.ReadSingle();
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            var config = ModContent.GetInstance<CTG2Config>();
            if (!config.EnableProjectileTeamColoring){
                return true;}

            int team = (int)projectile.localAI[0];

            if (team == 0)
                return true;
            if (projectile.type == 153 || projectile.type == 699 || projectile.type == 228 || projectile.type == 480 || projectile.type == ModContent.ProjectileType<ChargedBowProjectile>() ||
                projectile.type == ModContent.ProjectileType<AmalgamatedHandProjectile1>() || projectile.type == ModContent.ProjectileType<AmalgamatedHandProjectile2>() || projectile.type == 80) // don't include in the color mask
                return true; //exclude rotted fork (gladiator, regen mutant), ghastly glaive (paladin), archer bow, jman cursed flame, ice rod (white mage), amalgamated hand 1/2 (rush mutant)

            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            Rectangle rectangle = new(0, 0, texture.Width, texture.Height);
            rectangle = new(0, 0, texture.Width, texture.Height);

            Vector2 origin = rectangle.Size() / 2f;
            Vector2 drawPosition = projectile.position - Main.screenPosition + new Vector2(projectile.width / 2f, projectile.height / 2f);

            Color teamColor = Color.Gray;

            if (team == 1)
                teamColor = new Color(255, 0, 0, 255);
            if (team == 3)
                if (projectile.type == ProjectileID.ThornChakram || projectile.type == ProjectileID.Flamarang || projectile.type == 15 || projectile.type == ProjectileID.Bananarang || projectile.type == 304)
                    teamColor = new Color(0, 0, 255, 255); // all these projectiles are bright shades of red/orange/yellow and require a gray sprite to properly color - black for now
                else
                    teamColor = new Color(50, 50, 255, 255);

            Main.EntitySpriteDraw(texture, drawPosition, rectangle, teamColor, projectile.rotation, origin, projectile.scale, SpriteEffects.None);

            return false;
        }
    }
}
