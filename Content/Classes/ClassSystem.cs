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
    private bool hasHealed = false;
    private bool hasClearedInv = false;


    // CLASS HP
    int archerHP = 160;
    int ninjaHP = 160;
    int beastHP = 160;
    int gladiatorHP = 140;
    int paladinHP = 160;
    int jungleManHP = 140;
    int blackMageHP = 160;
    int psychicHP = 160;
    int whiteMageHP = 160;
    int minerHP = 180;
    int fishHP = 150;
    int clownHP = 120;
    int flameBunnyHP = 160;
    int tikiPriestHP = 160;
    int treeHP = 160;
    int mutantHP = 160;
    int leechHP = 140;

    public override void ResetEffects()
    {

        Player.AddBuff(BuffID.Shine, 54000);
        Player.AddBuff(BuffID.NightOwl, 54000);
        Player.AddBuff(BuffID.Builder, 54000);

        if (!hasClearedInv)
        {
            for (int i = 0; i < Player.inventory.Length; i++) Player.inventory[i] = new Item();
            hasClearedInv = true;
        }

        switch (playerClass) //permabuffs are put here
        {
            case 1: // Archer

                Player.statLifeMax2 = archerHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 2: // Ninja

                Player.statLifeMax2 = ninjaHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }

                GiveClassItems();
                break;

            case 3: // Beast

                Player.statLifeMax2 = beastHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }

                GiveClassItems();
                break;

            case 4: // Gladiator

                Player.statLifeMax2 = gladiatorHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 5: // Paladin
            
                Player.statLifeMax2 = paladinHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 6: // Jungle Man

                Player.statLifeMax2 = jungleManHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 7: // Black Mage

                Player.statLifeMax2 = blackMageHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 8: // Psychic

                Player.statLifeMax2 = psychicHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 9: // White Mage

                Player.statLifeMax2 = whiteMageHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 10: // Miner

                Player.statLifeMax2 = minerHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 11: // Fish

                Player.statLifeMax2 = fishHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 12: // Clown

                Player.statLifeMax2 = clownHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 13: // Flame Bunny

                Player.statLifeMax2 = flameBunnyHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 14: // Tiki Priest

                Player.statLifeMax2 = tikiPriestHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 15: // Tree

                Player.statLifeMax2 = treeHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 16: // Mutant

                Player.statLifeMax2 = mutantHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
                GiveClassItems();
                break;

            case 17: // Leech

                Player.statLifeMax2 = leechHP;

                if (!hasHealed)
                {
                    Player.statLife = Player.statLifeMax2;
                    hasHealed = true;
                }
                
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
                case 1: //archer
                    //format is (null, id, amount) or (null, ItemID.name, amount)

                    Player.QuickSpawnItem(null, ModContent.ItemType<CTG2.Content.Items.ShardstonePickaxe>(), 1); // pickaxe
                    Player.QuickSpawnItem(null, 215, 1); // whoopie cushion
                    Player.QuickSpawnItem(null, ModContent.ItemType<CTG2.Content.Items.Rancor>(), 1); // archer's bow
                    Player.QuickSpawnItem(null, 2, 50); // dirt

                    Player.QuickSpawnItem(null, ItemID.HellfireArrow, 9999);

                    Player.armor[0] = new Item();
                    Player.armor[0].SetDefaults(ItemID.VulkelfEar);

                    Player.armor[1] = new Item();
                    Player.armor[1].SetDefaults(ItemID.NecroBreastplate);

                    Player.armor[2] = new Item();
                    Player.armor[2].SetDefaults(ItemID.CactusLeggings);

                    Player.armor[3] = new Item();
                    Player.armor[3].SetDefaults(ItemID.LuckyHorseshoe);

                    Player.armor[4] = new Item();
                    Player.armor[4].SetDefaults(ItemID.AnkletoftheWind);

                    Player.armor[5] = new Item();
                    Player.armor[5].SetDefaults(ItemID.LuckyCoin);

                    Player.armor[6] = new Item();
                    Player.armor[6].SetDefaults(ItemID.TsunamiInABottle);

                    Player.armor[7] = new Item();
                    Player.armor[7].SetDefaults(ItemID.Toolbox);

                    Player.armor[10] = new Item();
                    Player.armor[10].SetDefaults(ItemID.HerosHat);

                    Player.armor[11] = new Item();
                    Player.armor[11].SetDefaults(ItemID.HerosShirt);

                    Player.armor[12] = new Item();
                    Player.armor[12].SetDefaults(ItemID.HerosPants);

                    Player.armor[13] = new Item();
                    Player.armor[13].SetDefaults(ItemID.JungleRose);

                    Player.armor[14] = new Item();
                    Player.armor[14].SetDefaults(ItemID.HunterCloak);

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
