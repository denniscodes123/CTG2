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

            case 2: // Ninja
                Player.AddBuff(BuffID.Ironskin, 2);
                GiveClassItems();
                break;

            case 3: // Beast
                Player.AddBuff(BuffID.MagicPower, 2);
                GiveClassItems();
                break;

            case 4: // Gladiator
                GiveClassItems();
                break;
                
            case 5: // Paladin
                GiveClassItems();
                break;

            case 6: // Jungle Man
                GiveClassItems();
                break;
                
            case 7: // Black Mage
                GiveClassItems();
                break;
                
            case 8: // Psychic
                GiveClassItems();
                break;
                
            case 9: // White Mage
                GiveClassItems();
                break;
                
            case 10: // Miner
                GiveClassItems();
                break;
                
            case 11: // Fish
                GiveClassItems();
                break;
                
            case 12: // Clown
                GiveClassItems();
                break;
                
            case 13: // Flame Bunny
                GiveClassItems();
                break;
                
            case 14: // Tiki Priest
                GiveClassItems();
                break;
                
            case 15: // Tree
                GiveClassItems();
                break;
                
            case 16: // Mutant
                GiveClassItems();
                break;

            case 17: // Leech
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

                case 4:
                    break;

                case 5:
                    break;

                case 6:
                    break;

                case 7:
                    break;

                case 8:
                    break;

                case 9:
                    break;

                case 10:
                    break;

                case 11:
                    break;

                case 12:
                    break;

                case 13:
                    break;

                case 14:
                    break;

                case 15:
                    break;

                case 16:
                    break;

                case 17:
                    break;
            }

            hasReceivedItems = true;
        }
    }
}
