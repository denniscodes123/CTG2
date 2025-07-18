using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace CTG2.Content.Classes
{

    public class AllNpcs : GlobalNPC
    {
        public int team = 3;

        public override bool InstancePerEntity => true;
    }


    public class TikiTotem : ModNPC
    {

        private float healFrameGap = 30;
        private float frameCount = 0;
        private int totemTeam = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }


        public override void SetDefaults()
        {   
            totemTeam = (int)NPC.ai[0];
            NPC.width = 32;
            NPC.height = 48;
            NPC.damage = 0; 
            NPC.defense = 0;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0; //make this higher for more knockback

            NPC.aiStyle = -1; 
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.friendly = false;
        }


        public override void AI()
        {
            
            float friction = 0f; //update this to change friction

            if (NPC.velocity.X > 0f)
            {
                NPC.velocity.X -= friction;
                if (NPC.velocity.X < 0f)
                    NPC.velocity.X = 0f;
            }
            else if (NPC.velocity.X < 0f)
            {
                NPC.velocity.X += friction;
                if (NPC.velocity.X > 0f)
                    NPC.velocity.X = 0f;
            }

            float gravity = 0.3f;
            float maxFallSpeed = 10f;

            NPC.velocity.Y += gravity;
            if (NPC.velocity.Y > maxFallSpeed)
                NPC.velocity.Y = maxFallSpeed;

            foreach (Player player in Main.player)
            {
                if (!player.active || player.dead)
                    continue;
                // ai[0] stores tiki's team
                if (player.team != (int)NPC.ai[0])
                    continue;
                
                if (Vector2.Distance(NPC.Center, player.Center) <= 14 * 16 && frameCount % healFrameGap == 0) // 14 block radius
                {   
                    player.Heal(1);
                }
            }
            frameCount++;

            // Main.NewText(NPC.GetGlobalNPC<AllNpcs>().team);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Rectangle frame = NPC.frame;
            Vector2 origin = new Vector2(frame.Width / 2f, frame.Height / 2f);
            Color teamColar = Color.Gray;

            int tikiTeam = (int)NPC.ai[0];
            if (tikiTeam == 1){ teamColar = new Color(255, 0, 0, 180); }
            if (tikiTeam == 3){ teamColar = new Color(0, 0, 255, 180); }

                Vector2 drawPosition = NPC.Center - screenPos + new Vector2(0, 2.8f);

            spriteBatch.Draw(
                texture,
                drawPosition,
                frame,
                teamColar,
                NPC.rotation,
                origin,
                NPC.scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
