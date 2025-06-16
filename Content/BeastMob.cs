using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CTG2.Content
{
    public class StationaryBeast : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 27;
            NPC.damage = 40; 
            NPC.defense = 10;
            NPC.lifeMax = 100;
            NPC.knockBackResist = 2f; //make this higher for more knockback

            NPC.aiStyle = -1; 
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.friendly = false;
        }

        public override void AI()
        {
            
            float friction = 0.1f; //update this to change friction

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
        }
    }
}
