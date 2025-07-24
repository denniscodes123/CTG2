using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CTG2.Content.ClientSide;

namespace CTG2.Content
{
    public class UnbreakableTiles : GlobalTile
    {
        public override bool CanPlace(int i, int j, int type)
        {
            Tile existing = Main.tile[i, j];

            //prevent block replace
            if (existing.HasTile)
                return false;

            return base.CanPlace(i, j, type);
        }

        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            if (!Main.LocalPlayer.GetModPlayer<AdminPlayer>().IsAdmin && GameInfo.matchStage!= 2)
            {
            if (type != TileID.Dirt && type != TileID.Bubble && type != 127) //for wmage ice block)
    {
        return false;
    }
            }

                    return base.CanKillTile(i, j, type, ref blockDamaged);
        }

        public override bool CanExplode(int i, int j, int type)
        {

            if (type != TileID.Dirt)
                return false;

            return base.CanExplode(i, j, type);
        }
    }

    public class NoExplosionWall : GlobalWall
    {
        public override bool CanExplode(int i, int j, int type)
        {

            return false;
        }
    }
    
}
