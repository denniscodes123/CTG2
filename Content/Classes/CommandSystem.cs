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
                caller.Reply("You are now ninja!", Color.Green);
                break;

            case 3: 
                modPlayer.playerClass = 3;
                caller.Reply("You are now the beast!", Color.Green);
                break;

            case 4: 
                modPlayer.playerClass = 4;
                caller.Reply("You are now the gladiator!", Color.Green);
                break;

            case 5: 
                modPlayer.playerClass = 5;
                caller.Reply("You are now paladin!", Color.Green);
                break;

            case 6: 
                modPlayer.playerClass = 6;
                caller.Reply("You are now jman!", Color.Green);
                break;    

            case 7:
                modPlayer.playerClass = 7;
                caller.Reply("You are now bmage!", Color.Green);
                break;

            case 8:
                modPlayer.playerClass = 8;
                caller.Reply("You are now a psychic!", Color.Green);
                break;

            case 9:
                modPlayer.playerClass = 9;
                caller.Reply("You are now wmage!", Color.Green);
                break;

            case 10:
                modPlayer.playerClass = 10;
                caller.Reply("You are now miner!", Color.Green);
                break;

            case 11:
                modPlayer.playerClass = 11;
                caller.Reply("You are now fish!", Color.Green);
                break;

            case 12:
                modPlayer.playerClass = 12;
                caller.Reply("You are now clown!", Color.Green);
                break;

            case 13:
                modPlayer.playerClass = 13;
                caller.Reply("You are now fbunny!", Color.Green);
                break;

            case 14:
                modPlayer.playerClass = 14;
                caller.Reply("You are now tiki priest!", Color.Green);
                break;

            case 15:
                modPlayer.playerClass = 15;
                caller.Reply("You are now tree!", Color.Green);
                break;

            case 16:
                modPlayer.playerClass = 16;
                caller.Reply("You are now mutant!", Color.Green);
                break;

            case 17:
                modPlayer.playerClass = 17;
                caller.Reply("You are now leech!", Color.Green);
                break;


            default:
                caller.Reply("Invalid class. Available: 1 (Archer), 2 (Warrior), 3 (Mage)", Color.Red);
                break;
        }
    }
}
