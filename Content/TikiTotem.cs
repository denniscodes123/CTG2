using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CTG2.Content
{
    public class TikiTotem : ModNPC
    {

        float healFrameGap = 40;
        float frameCount = 0;


        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }


        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 48;
            NPC.damage = 0; 
            NPC.defense = 0;
            NPC.lifeMax = 300;
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
                if (!player.active || player.dead || player.whoAmI == player.whoAmI)
                    continue;

                if (Vector2.Distance(NPC.Center, player.Center) <= 15 * 16 && frameCount % healFrameGap == 0 && Main.netMode != NetmodeID.MultiplayerClient) // 15 block radius
                {   
                    player.statLife += 1;
                    if (player.statLife > player.statLifeMax2)
                        player.statLife = player.statLifeMax2;

                    CombatText.NewText(player.Hitbox, CombatText.HealLife, 1);
                    player.HealEffect(1);
                    NetMessage.SendData(MessageID.PlayerLifeMana,      // 16
                        remoteClient: -1,         
                        ignoreClient: -1,  
                        text: null,
                        number: player.whoAmI);
                }
            }

            frameCount++;
        }
    }
}
