using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTG2.Content.ClientSide;

public class GemDrawLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition()
    {
        return new AfterParent(PlayerDrawLayers.Head);
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player player = drawInfo.drawPlayer;
        var gameManager = ModContent.GetInstance<ServerSide.GameManager>();
        if (gameManager == null) return;

        Texture2D gemTexture = null;

        if (gameManager.BlueGem.IsHeld && gameManager.BlueGem.HeldBy == player.whoAmI)
        {
            gemTexture = Terraria.GameContent.TextureAssets.Item[ItemID.LargeSapphire].Value;
        }
        else if (gameManager.RedGem.IsHeld && gameManager.RedGem.HeldBy == player.whoAmI)
        {
            gemTexture = Terraria.GameContent.TextureAssets.Item[ItemID.LargeRuby].Value;
        }

        if (gemTexture != null)
        {
            float drawX = (int)(drawInfo.Position.X + player.width / 2f - Main.screenPosition.X);
            float drawY = (int)(drawInfo.Position.Y - gemTexture.Height - 4f - Main.screenPosition.Y); 

            var position = new Vector2(drawX, drawY);

    
            var data = new DrawData(
                gemTexture,
                position,
                null, 
                Color.White,
                0f, 
                gemTexture.Size() / 2f, 
                1f, 
                SpriteEffects.None,
                0
            );

            drawInfo.DrawDataCache.Add(data);
        }
    }
}