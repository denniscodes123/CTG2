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
                    //fromat is (null, id, amount) or (null, ItemID.name, amount)
                   
                    //Defaults (Copy and paste these for new classes)
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);
                    


                    Player.QuickSpawnItem(null, 494);
                    Player.QuickSpawnItem(null, ItemID.TendonBow);
                    Player.QuickSpawnItem(null, ItemID.DemonBow);
                    Player.QuickSpawnItem(null, ItemID.HellfireArrow, 999);

                    Player.armor[0] = new Item();
                    Player.armor[0].SetDefaults(ItemID.NecroHelmet);

                    Player.armor[1] = new Item();
                    Player.armor[1].SetDefaults(ItemID.NecroBreastplate);

                    Player.armor[2] = new Item();
                    Player.armor[2].SetDefaults(ItemID.NecroGreaves);

                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    break;


                case 2: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);



                    Player.QuickSpawnItem(null, 64);
                    Player.QuickSpawnItem(null, 176, 999);
                    break;

                case 3: 

                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);



                    Player.QuickSpawnItem(null, 4347);
                    Player.QuickSpawnItem(null, 1836, 9999);
                    break;

                case 4: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 741);
                    break;

                case 5: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);


                    Player.QuickSpawnItem(null, 165);
                    Player.QuickSpawnItem(null, 4760);
                    break;
                case 6: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    break;
                case 7: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 272);
                    Player.QuickSpawnItem(null, 3103);

                    break;
                case 8: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 3476);

                    break;
                case 9: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 1446);
                    Player.QuickSpawnItem(null, 1266);

                    break;
                case 10: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 235, 20); //need to create increasing bomb logic left at 20 for now
                    Player.QuickSpawnItem(null, 1313);

                    break;
                case 11: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 165);

                    break;
                case 12: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 5147);

                    break;
                case 13: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 119);

                    break;
                case 14: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 788);

                    break;

                case 15: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 740);

                    break;

                case 16: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 220);

                    break;

                case 17: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 272);
                    Player.QuickSpawnItem(null, 670);

                    break;

                /*case 18: 
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);
//disabled right now
                    Player.QuickSpawnItem(null, 113); */
            }

            hasReceivedItems = true;
        }
    }
}
