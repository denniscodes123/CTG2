using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;
using System;

namespace CTG2.Content.Items.ModifiedWeps
{
    public class OverloadedWeps : GlobalItem
    {

        private uint bananarangDelay = 55;
        private uint bananarangLastUsedCounter = 0;

        private uint blowgunDelay = 40;
        private uint blowgunLastUsedCounter = 0;

        private uint goldenShowerDelay = 50;
        private uint goldenShowerLastUsedCounter = 0;

        private uint chainKnifeDelay = 50;
        private uint chainKnifeLastUsedCounter = 0;

        private uint cursedFlamesDelay = 50;
        private uint cursedFlamesLastUsedCounter = 0;


        public override bool InstancePerEntity => true;


        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.WhoopieCushion:
                    item.useTime = 1;
                    item.useAnimation = 1;
                    break;
                case ItemID.GoldenShower: // Fish
                    item.useTime = 22;
                    item.useAnimation = 22;
                    item.shootSpeed = 6.2f;
                    item.damage = 35;
                    item.mana = 0;
                    item.scale = 0;
                    item.crit = 0;
                    item.UseSound = SoundID.Item109;
                    break;
                case ItemID.Blowgun: // Tiki Priest
                    item.useTime = 18;
                    item.useAnimation = 18;
                    item.shootSpeed = 13f;
                    item.autoReuse = false;
                    item.damage = 32;
                    item.crit = 0;
                    break;
                case ItemID.Bananarang: // Tree
                    item.useTime = 20;
                    item.useAnimation = 20;
                    item.shootSpeed = 11.4f;
                    item.knockBack = 6f;
                    item.autoReuse = false;
                    item.damage = 35;
                    item.crit = 0;
                    break;
                case ItemID.ChainKnife: // Leech
                    item.useTime = 4;
                    item.useAnimation = 4;
                    item.shootSpeed = 20;
                    item.damage = 31;
                    item.crit = 0;
                    break;
                case ItemID.CursedFlames: // Leech
                    item.useTime = 40;
                    item.useAnimation = 40;
                    item.shootSpeed = 15;
                    item.damage = 23;
                    item.mana = 0;
                    item.scale = 0;
                    item.crit = 0;
                    item.shoot = ProjectileID.VampireKnife;
                    break;
            }
        }


        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.Bananarang)
            {
                if (Main.GameUpdateCount - bananarangLastUsedCounter >= bananarangDelay)
                {
                    bananarangLastUsedCounter = Main.GameUpdateCount;
        
                    return true;
                }
                else
                    return false;
            }
            else if (item.type == ItemID.Blowgun)
            {
                if (Main.GameUpdateCount - blowgunLastUsedCounter >= blowgunDelay)
                {
                    blowgunLastUsedCounter = Main.GameUpdateCount;
        
                    return true;
                }
                else
                    return false;
            }
            else if (item.type == ItemID.GoldenShower)
            {
                if (Main.GameUpdateCount - goldenShowerLastUsedCounter >= goldenShowerDelay)
                {
                    goldenShowerLastUsedCounter = Main.GameUpdateCount;
        
                    return true;
                }
                else
                    return false;
            }
            else if (item.type == ItemID.ChainKnife)
            {
                if (Main.GameUpdateCount - chainKnifeLastUsedCounter >= chainKnifeDelay)
                {
                    chainKnifeLastUsedCounter = Main.GameUpdateCount;
        
                    return true;
                }
                else
                    return false;
            }
            else if (item.type == ItemID.CursedFlames)
            {
                if (Main.GameUpdateCount - cursedFlamesLastUsedCounter >= cursedFlamesDelay)
                {
                    cursedFlamesLastUsedCounter = Main.GameUpdateCount;
        
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
	 	}
    }
}
