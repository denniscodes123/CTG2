using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CTG2.Content.Items;
using CTG2.Content.Items.ModifiedWeps;
using Microsoft.Xna.Framework; 
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Runtime.CompilerServices;
using ClassesNamespace;
using CTG2;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;

namespace CTG2.Content.ClientSide;


public class ClassUpgrade
{
    public string Name;
    public int  IconItemID;    // or buff ID, or a texture reference
    public Action<Player> Apply;
}

public class ClassUI : UIState
{
    private UIPanel mainPanel;
    private UIList classList;
    private UIList upgradeList;
    private UIImage previewImage;

    private ClassConfig selectedClass;
    private ClassUpgrade selectedUpgrade;

    public override void OnInitialize()
    {
        selectedClass = CTG2.config.Classes[0];
        // Main container
        mainPanel = new UIPanel
        {
            Width = { Percent = 0.5f },
            Height = { Percent = 0.5f },
            HAlign = 0.5f, VAlign = 0.5f,
            BackgroundColor = new Color(162, 69, 255)
        };
        Append(mainPanel);

        // Class List & Scroll Bar
        classList = new UIList
        {
            Width = { Pixels = 200 },
            Height = { Percent = 1f },
            ListPadding = 5f
        };
        var classScrollbar = new UIScrollbar
        {
            Height = { Percent = 1f },
            HAlign = 1f,
            VAlign = 0f
        };
        classList.SetScrollbar(classScrollbar);
        mainPanel.Append(classList);
        mainPanel.Append(classScrollbar);

        // Class Info (Dependent on which class selected atm)
        var className = new UITextPanel<string>(selectedClass.Name)
        {
            Width = { Pixels = 180 },
            Height = { Pixels = 40 },
            PaddingTop = 10
        };
        var classSummary = new UITextPanel<string>(selectedClass.Summary) {
            Width = { Pixels = 180 },
            Height = { Pixels = 40 },
            PaddingTop = 10
        };
        var classDesc = new UITextPanel<string>(selectedClass.Description) {
            Width = { Pixels = 180 },
            Height = { Pixels = 40 },
            PaddingTop = 10
        };
        mainPanel.Append(className);
        mainPanel.Append(classSummary);
        mainPanel.Append(classDesc);
        
        upgradeList = new UIList
        {
            Left = { Pixels = 210 },
            Width = { Percent = 0.5f },
            Height = { Percent = 1f },
            ListPadding = 5f
        };
        mainPanel.Append(upgradeList);

        // Preview area
        /*
        previewImage = new UIImage(ModContent.Request<Texture2D>("YourMod/Textures/PreviewBackground"))
        {
            Left = { Percent = 0.7f },
            Width = { Percent = 0.25f },
            Height = { Percent = 0.5f }
        };
        mainPanel.Append(previewImage); */

        PopulateClasses();
        PopulateUpgrades(GameClass.Archer);
    }

    private void PopulateClasses()
    {
        classList.Clear();
        foreach (GameClass ct in Enum.GetValues(typeof(GameClass)))
        {
            var button = new UITextPanel<string>(ct.ToString())
            {
                Width = { Pixels = 180 },
                Height = { Pixels = 40 },
                PaddingTop = 10
            };
            classList.Add(button);
        }
    }

    private void PopulateUpgrades(GameClass ct)
    {
        upgradeList.Clear();
        /*
        foreach (var up in UpgradesByClass[ct])
        {
            var icon = new UIImage(TextureAssets.Buff[1]); // tModLoader helper to show an item or buff icon
            var panel = new UIPanel
            {
                Width = { Pixels = 180 },
                Height = { Pixels = 40 }
            };
            icon.Left.Pixels = 4;
            panel.Append(icon);

            var label = new UIText(up.Name)
            {
                Left = { Pixels = 40 }, VAlign = 0.5f
            };
            panel.Append(label);

            upgradeList.Add(panel);
        }
        */
    }
}