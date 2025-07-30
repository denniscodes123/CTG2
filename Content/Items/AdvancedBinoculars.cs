using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;


namespace CTG2.Content.Items
{
    public class AdvancedBinoculars : ModItem
    {
        public override string Texture => "Terraria/Images/Item_1299";

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.None;
            Item.useTime = 1;
            Item.useAnimation = 0;
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }
    }


    public class CameraSystem : ModSystem
    {
        int state = 0;
        bool recentlyPressed = false;
        Vector2 mouseOffset;
        Vector2 offset;
        Vector2 targetPosition;

        public override void ModifyScreenPosition()
        {
            Player player = Main.LocalPlayer;

            for (int i = 0; i < player.armor.Length; i++)
            {
                Item accessory = player.armor[i];

                if (accessory?.ModItem is AdvancedBinoculars)
                {
                    AdvancedBinocularsAI(player);
                }
            }
        }


        public void AdvancedBinocularsAI(Player player)
        {
            switch(state)
            {
                case 0:
                    if (CTG2.AdvancedBinocularsKeybind.JustPressed && !recentlyPressed)
                        state = 1;
                    
                    break;
                
                case 1:
                    mouseOffset = Main.MouseWorld - player.Center;
                    offset = 0.9f * mouseOffset;
                    targetPosition = player.Center - new Vector2(Main.screenWidth, Main.screenHeight) / 2f + offset;

                    Main.SetCameraLerp(0, 0);
                    Main.screenPosition = Vector2.Lerp(Main.screenPosition, targetPosition, 1f);

                    if (CTG2.AdvancedBinocularsKeybind.JustPressed && !recentlyPressed)
                        state = 2;
                    
                    break;
                
                case 2:
                    offset = 0.9f * mouseOffset;
                    targetPosition = player.Center - new Vector2(Main.screenWidth, Main.screenHeight) / 2f + offset;

                    Main.SetCameraLerp(0, 0);
                    Main.screenPosition = Vector2.Lerp(Main.screenPosition, targetPosition, 1f);
                    
                    if (CTG2.AdvancedBinocularsKeybind.JustPressed && !recentlyPressed)
                        state = 0;
                    
                    break;
            }

            recentlyPressed = CTG2.AdvancedBinocularsKeybind.JustPressed;
        }
    }
}