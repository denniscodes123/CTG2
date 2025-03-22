using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CTG2.Content;


public class ClassCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat; 
    public override string Command => "class"; 
    public override string Description => "Select a player class"; 

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (GameUI.matchTimer - (int)(Main.GameUpdateCount / 60)<0) //!CTG2.Content.Game.preparationPhase
        {
            caller.Reply("You can only select a class after the match has started! Use /start first.", Color.Red);
            return;
        }

        if (args.Length < 1 || !int.TryParse(args[0], out int classType))
        {
            caller.Reply("Usage: /class [number]", Color.Red);
            return;
        }

        Player player = caller.Player;
        var modPlayer = player.GetModPlayer<ClassSystem>();

        switch (classType)
        {
            case 1: 
                modPlayer.playerClass = 1;
                caller.Reply("You are now an Archer!", Color.Green);
                break;

            case 2: 
                modPlayer.playerClass = 2;
                caller.Reply("You are now a Warrior!", Color.Green);
                break;

            case 3: 
                modPlayer.playerClass = 3;
                caller.Reply("You are now a Mage!", Color.Green);
                break;

            default:
                caller.Reply("Invalid class. Available: 1 (Archer), 2 (Warrior), 3 (Mage)", Color.Red);
                break;
        }
    }
}
