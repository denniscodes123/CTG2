using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CTG2.Content.Items;
using CTG2.Content.Items.ModifiedWeps;

public class ClassSystem : ModPlayer
{
    public int playerClass = 0; 
    private bool hasReceivedItems = false; 

    public override void ResetEffects()
    {
        switch (playerClass)
        {
            case 1: // Archer
                Player.AddBuff(BuffID.Archery, 2);
                GiveClassItems();
                break;

            case 2: // Randomclass
                Player.AddBuff(BuffID.Ironskin, 2);
                GiveClassItems();
                break;

            case 3: // Randomclass
                Player.AddBuff(BuffID.MagicPower, 2);
                GiveClassItems();
                break;
        }
    }

    private void GiveClassItems()
    {
        if (!hasReceivedItems)
        {
            switch (playerClass)
            {
                case 1: 
                    Player.QuickSpawnItem(null, ItemID.TendonBow);
                    Player.QuickSpawnItem(null, ItemID.DemonBow);
                    Player.QuickSpawnItem(null, ItemID.NecroHelmet);
                    Player.QuickSpawnItem(null, ItemID.NecroBreastplate);
                    Player.QuickSpawnItem(null, ItemID.NecroGreaves);
                    Player.QuickSpawnItem(null, ItemID.HellfireArrow, 999);
                    break;

                case 2: 
                    Player.QuickSpawnItem(null, ItemID.NightsEdge);
                    Player.QuickSpawnItem(null, ItemID.CobaltHelmet);
                    Player.QuickSpawnItem(null, ItemID.CobaltBreastplate);
                    Player.QuickSpawnItem(null, ItemID.CobaltLeggings);
                    break;

                case 3: 
                    Player.QuickSpawnItem(null, ItemID.WaterBolt);
                    Player.QuickSpawnItem(null, ItemID.WizardHat);
                    Player.QuickSpawnItem(null, ItemID.ManaCrystal, 3);
                    break;
            }

            hasReceivedItems = true;
        }
    }
}
