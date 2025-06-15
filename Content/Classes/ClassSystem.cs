using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CTG2.Content.Items;
using CTG2.Content.Items.ModifiedWeps;
using Microsoft.Xna.Framework; 


public class ClassSystem : ModPlayer
{
    public int playerClass = 0;
    private bool hasReceivedItems = false;

    public override void ResetEffects()
    {
        switch (playerClass) //permabuffs are put here
        {
            case 1: // Archer
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;

            case 2: // Ninja
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;

            case 3: // Beast
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;
                
            case 4: // Gladiator
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;
                
            case 5: // Paladin
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;

            case 6: // Jungle Man
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;

            case 7: // Black Mage
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;
                
            case 8: // Psychic
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;

            case 9: // White Mage
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;

            case 10: // Miner
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;

            case 11: // Fish
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;

            case 12: // Clown
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;

            case 13: // Flame Bunny
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;
                
            case 14: // Tiki Priest
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;

            case 15: // Tree
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;
                
            case 16: // Mutant
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
                GiveClassItems();
                break;

            case 17: // Leech
                Player.AddBuff(BuffID.Shine, 54000);
                Player.AddBuff(BuffID.NightOwl, 54000);
                Player.AddBuff(BuffID.Builder, 54000);
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
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
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
                    SpawnCustomItem(
                    itemID: 64,
                    prefix: 39,            
                    damage: 56,
                    useTime: 10,
                    useAnimation: 10,
                    scale: 0f,
                    knockBack: 3.75f,
                    shoot: 93,
                    shootSpeed: 10f,
                    colorOverride: new Color(0, 0, 255) 
                    );

                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);



                    Player.QuickSpawnItem(null, 64);
                    Player.QuickSpawnItem(null, 176, 999);
                    break;

                case 3:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);



                    Player.QuickSpawnItem(null, 4347);
                    Player.QuickSpawnItem(null, 1836, 9999);
                    break;

                case 4:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 741);
                    break;

                case 5:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);


                    Player.QuickSpawnItem(null, 165);
                    Player.QuickSpawnItem(null, 4760);
                    break;
                case 6:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    break;
                case 7:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 272);
                    Player.QuickSpawnItem(null, 3103);

                    break;
                case 8:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 3476);

                    break;
                case 9:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 1446);
                    Player.QuickSpawnItem(null, 1266);

                    break;
                case 10:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 235, 20); //need to create increasing bomb logic left at 20 for now
                    Player.QuickSpawnItem(null, 1313);

                    break;
                case 11:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 165);

                    break;
                case 12:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 5147);

                    break;
                case 13:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 119);

                    break;
                case 14:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 788);

                    break;

                case 15:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 740);

                    break;

                case 16:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
                    Player.QuickSpawnItem(null, 1202); //titanium pickaxe
                    Player.QuickSpawnItem(null, 2, 999); //dirt
                    Player.QuickSpawnItem(null, 215, 1); //whoopie cushion
                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.QuickSpawnItem(null, 220);

                    break;

                case 17:
                    SpawnCustomItem(ItemID.TitaniumPickaxe, 39, 8, 7, 14, 0.75f);
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

   
private void SpawnCustomItem(
    int itemID,
    int? prefix = null,
    int? damage = null,
    int? useTime = null,
    int? useAnimation = null,
    float? scale = null,
    float? knockBack = null,
    int? shoot = null,
    float? shootSpeed = null,
    Color? colorOverride = null
)
{
    Item item = new Item();
    item.SetDefaults(itemID);

    if (prefix.HasValue)
        item.Prefix(prefix.Value);
    if (damage.HasValue)
        item.damage = damage.Value;
    if (useTime.HasValue)
        item.useTime = useTime.Value;
    if (useAnimation.HasValue)
        item.useAnimation = useAnimation.Value;
    if (scale.HasValue)
        item.scale = scale.Value;
    if (knockBack.HasValue)
        item.knockBack = knockBack.Value;
    if (shoot.HasValue)
        item.shoot = shoot.Value;
    if (shootSpeed.HasValue)
        item.shootSpeed = shootSpeed.Value;
    if (colorOverride.HasValue)
        item.color = colorOverride.Value;

    Player.QuickSpawnItem(null, item, 1);
}



}
